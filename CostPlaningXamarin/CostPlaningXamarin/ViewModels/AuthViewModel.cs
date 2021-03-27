using CostPlaningXamarin.Command;
using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CostPlaningXamarin.ViewModels
{
    public class AuthViewModel : BaseViewModel
    {
        ISQLiteService _sqliteService = DependencyService.Get<ISQLiteService>();
        IWiFiManager _WiFiManager = DependencyService.Get<IWiFiManager>();
        IDeviceService _deviceService = DependencyService.Get<IDeviceService>();
        IUserService _userService = DependencyService.Get<IUserService>();
        INavigationServices _navigateService = DependencyService.Get<INavigationServices>();
        private ICommand _ApplyUser;
        private ICommand _IsVisible;
        private bool _isVisible;
        private User _selectedUser;
        private Lazy<List<User>> _users;
        //https://blog.stephencleary.com/2013/01/async-oop-2-constructors.html
        private readonly NLog.ILogger _logger = NLog.LogManager.GetCurrentClassLogger();
        public AuthViewModel()
        {
            //Initialization = InitializeAsync();
            //Init();
            Task.Run(() =>
            {
                _logger.Info("first sync");
                _WiFiManager.FristSyncData();
            }).Wait();
            _users = new Lazy<List<User>>(GetUsers);
        }
        private List<User> GetUsers()
        {
            _logger.Info("GetUser");
            return _sqliteService.GetUsers().GetAwaiter().GetResult();
        }

        public List<User> Users
        {
            get
            {
                return _users.Value;
            }
            //set { _users = value; OnPropertyChanged("Users"); }
        }
        private bool _isOnHomeWiFi;

        public bool IsOnHomeWiFi
        {
            get
            {
                if (_WiFiManager.IsHomeWifiConnected() && _WiFiManager.IsServerAvailable())
                {
                    return true;
                }

                return _isOnHomeWiFi;
            }
            set
            {
                _isOnHomeWiFi = value;
                OnPropertyChanged("IsConnected");
            }
        }

        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                if (_selectedUser != null)
                {
                    var x = Application.Current.MainPage.DisplayAlert("Alert", string.Format("You chose {0} {1}. Are you sure?", _selectedUser.FirstName, _selectedUser.LastName), "Yes", "No");
                    Task.Run(() =>
                    {
                        Task.Delay(500);
                        ConfirmationUser(x.GetAwaiter().GetResult());

                    });

                }
                OnPropertyChanged("SelectedUser");
            }
        }
        private void ConfirmationUser(bool confirmation)
        {
            if (confirmation)
            {
                _sqliteService.CreateAppUser(_selectedUser);
                _sqliteService.SaveAsync(_deviceService.PostCurrentDevice(_selectedUser.Id));

                _userService.PostDevice(_sqliteService.GetCurrentDeviceInfo());
                _WiFiManager.FirstSyncOrders();
                _WiFiManager.FirstSyncCategories();
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    _navigateService.NavigateToMainPage();
                });
            }
            else
            {
                SelectedUser = null;
                OnPropertyChanged("SelectedUser");
            }

        }
        
        


        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
                OnPropertyChanged("IsVisible");
            }
        }
        private User _user = new User();

        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged("User");
            }
        }

        public ICommand ApplyUserCommand
        {
            get
            {
                if (_ApplyUser == null)
                {
                    _ApplyUser = new RelayCommand(ApplyUser);
                }
                return _ApplyUser;
            }
        }
        //TODO: Nije jasno kako ovo radi sihrono? da li se moze desiti da createAppUser dobije ne dovrsenog serivceUser??
        private async void ApplyUser(object x)
        {

            var serviceUser = await _userService.PostUser(_user);

            await _sqliteService.CreateAppUser(serviceUser);
            await _sqliteService.SaveAsync(_deviceService.PostCurrentDevice(serviceUser.Id));

            await _userService.PostDevice(_sqliteService.GetCurrentDeviceInfo());
            await _WiFiManager.FirstSyncOrders();
            await _WiFiManager.FirstSyncCategories();

            _navigateService.NavigateToMainPage();
        }
        public ICommand IsVisibleCommand
        {
            get
            {
                if (_IsVisible == null)
                {
                    _IsVisible = new RelayCommand(IsVisibleAction, IsOnHomeWiFiPredicate);
                }
                return _IsVisible;
            }
        }


        private bool IsOnHomeWiFiPredicate(object x)
        {
            return IsOnHomeWiFi;
        }
        private void IsVisibleAction(object x)
        {
            _isVisible = true;
            OnPropertyChanged("IsVisible");
        }
    }
}
