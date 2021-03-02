﻿using CostPlaningXamarin.Command;
using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
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
        INavigationServices _navigateService = DependencyService.Get<INavigationServices>();
        private ICommand _ApplyUser;
        private ICommand _IsVisible;
        private bool _isVisible;
        private List<User> _users;
        private User _selectedUser;

        public AuthViewModel()
        {
            _users = _sqliteService.GetUsers().GetAwaiter().GetResult();
        }

        private bool _isDiconnected;

        public bool IsDiconnected
        {
            get { return _isDiconnected; }
            set 
            {
                _isDiconnected = value;
                if (!_WiFiManager.IsHomeWifiConnected() || !_WiFiManager.IsServerAvailable())
                {
                    _isDiconnected = false;
                }
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
                Device.BeginInvokeOnMainThread(() =>
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

        public List<User> Users
        {
            get { return _users; }
            set { _users = value; OnPropertyChanged("Users"); }
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
        private void ApplyUser(object x)
        {
            _sqliteService.CreateAppUser(_user);
            _navigateService.NavigateToMainPage();
        }
        public ICommand IsVisibleCommand
        {
            get
            {
                if (_IsVisible == null)
                {
                    _IsVisible = new RelayCommand(IsVisibleAction);
                }
                return _IsVisible;
            }
        }
        private void IsVisibleAction(object x)
        {
            _isVisible = true;
            OnPropertyChanged("IsVisible");
        }
    }
}
