using CostPlaningXamarin.Command;
using System.Windows.Input;
using CostPlaningXamarin.Interfaces;
using Xamarin.Forms;

namespace CostPlaningXamarin.ViewModels
{
    public class UserViewModel:BaseViewModel
    {
        
        private ICommand _NavigateToAddItemCommand;
        private ICommand _NavigateToTableOrders;
        INavigationServices _navigationService = DependencyService.Get<INavigationServices>();
               
        
        public ICommand NavigateToAddOrderCommand
        {
            get
            {
                if (_NavigateToAddItemCommand == null)
                {
                    _NavigateToAddItemCommand = new RelayCommand(NavigateToAddItemCommand);
                }
                return _NavigateToAddItemCommand;
            }
        }
        public void NavigateToAddItemCommand(object x)
        {
            _navigationService.NavigateToAddItem();
        }

        public ICommand NavigateToTableOrders
        {
            get
            {
                if (_NavigateToTableOrders == null)
                {
                    _NavigateToTableOrders = new RelayCommand(NavigateToTableOrder);
                }
                return _NavigateToTableOrders;
            }
        }
        public void NavigateToTableOrder(object x)
        {
            _navigationService.NavigateToTableOrders();
        }
    }
}
