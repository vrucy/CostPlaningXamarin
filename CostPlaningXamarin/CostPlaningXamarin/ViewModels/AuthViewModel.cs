using CostPlaningXamarin.Command;
using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using System.Windows.Input;
using Xamarin.Forms;

namespace CostPlaningXamarin.ViewModels
{
    public class AuthViewModel : BaseViewModel
    {
        ISQLiteService _sqliteService = DependencyService.Get<ISQLiteService>();
        INavigationServices _navigateService = DependencyService.Get<INavigationServices>();
        private ICommand _ApplyUser;
        public AuthViewModel()
        {
            
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
    }
}
