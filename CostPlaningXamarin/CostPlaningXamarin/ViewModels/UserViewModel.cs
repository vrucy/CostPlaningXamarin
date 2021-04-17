using CostPlaningXamarin.Command;
using System.Windows.Input;
using CostPlaningXamarin.Interfaces;
using Xamarin.Forms;

namespace CostPlaningXamarin.ViewModels
{
    public class UserViewModel:BaseViewModel
    {
        
        private ICommand _NavigateToAddItemCommand;
        private ICommand _NavigateToChartOrdersCommand;
        private ICommand _NavigateToTableOrders;
        private ICommand _NavigateToAddCategory;
        private ICommand _NavigateToEditCategory;
        private ICommand _NavigateToOrdersOptions;
        INavigationServices _navigationService = DependencyService.Get<INavigationServices>();
        IWiFiManager _wiFiManager = DependencyService.Get<IWiFiManager>();

        private bool _isOnHomeWiFi;
        //TODO: add notification if recive order which is change, and when recive new order. where put in view
        public bool IsOnHomeWiFi
        {
            get 
            {
                if (_wiFiManager.IsHomeWifiConnected() && _wiFiManager.IsServerAvailable())
                {
                    return true; 
                }
                return _isOnHomeWiFi;
            }
            set 
            {
                _isOnHomeWiFi = value;
                if (_wiFiManager.IsHomeWifiConnected() && _wiFiManager.IsServerAvailable())
                {
                    _isOnHomeWiFi = true ;
                }
                OnPropertyChanged(nameof(IsOnHomeWiFi));
            }
        }

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
        public ICommand NavigateToChartOrdersCommand
        {
            get
            {
                if (_NavigateToChartOrdersCommand == null)
                {
                    _NavigateToChartOrdersCommand = new RelayCommand(NavigateToChartOrders);
                }
                return _NavigateToChartOrdersCommand;
            }
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
        public ICommand NavigateToEditCategoryCommand
        {
            get
            {
                if (_NavigateToEditCategory == null)
                {
                    _NavigateToEditCategory = new RelayCommand(NavigateToEditCategory);
                }
                return _NavigateToEditCategory;
            }
        }
        public ICommand NavigateToOrdersOptionsCommand
        {
            get
            {
                if (_NavigateToOrdersOptions == null)
                {
                    _NavigateToOrdersOptions = new RelayCommand(NavigateToOrdersOptions);
                }
                return _NavigateToOrdersOptions;
            }
        }

        public ICommand NavigateToAddCategoryCommand
        {
            get
            {
                if (_NavigateToAddCategory == null)
                {
                    _NavigateToAddCategory = new RelayCommand(NavigateToAddCategory);
                }
                return _NavigateToAddCategory;
            }
        }
        public void NavigateToAddCategory(object x)
        {
            _navigationService.NavigateToAddCategoryAsync();
        }
        public void NavigateToChartOrders(object x)
        {
            _navigationService.NavigateToChartOrders();
        }
        public void NavigateToAddItemCommand(object x)
        {
            _navigationService.NavigateToAddItem();
        }
        public void NavigateToTableOrder(object x)
        {
            _navigationService.NavigateToTableOrders();
        }
        public void NavigateToEditCategory(object x)
        {
            _navigationService.NavigateToEditCategory();
        }
        public void NavigateToOrdersOptions(object x)
        {
            _navigationService.NavigateToOrdersOptions();
        }
    }
}
