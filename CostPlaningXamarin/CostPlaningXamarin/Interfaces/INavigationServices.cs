using CostPlaningXamarin.Models;

namespace CostPlaningXamarin.Interfaces
{
    public interface INavigationServices
    {
        void NavigateToMainPage();
        void NavigateToAddItem();
        void NavigateBack();
        void NavigateToTableOrders();
        void NavigateToEditCategory();
        void NavigateToOrdersOptions();
        void NavigateToAddUser();
        void NavigateToEditOrderAsync(Order order);
        void NavigateToAddCategoryAsync();
    }
}
