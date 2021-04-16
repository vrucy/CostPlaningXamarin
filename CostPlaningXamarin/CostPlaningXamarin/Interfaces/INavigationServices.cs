using CostPlaningXamarin.Models;

namespace CostPlaningXamarin.Interfaces
{
    public interface INavigationServices
    {
        void NavigateToMainPage();
        void NavigateToAddItem();
        void NavigateToTableOrders();
        void NavigateToEditCategory();
        void NavigateToOrdersOptions();
        void NavigateToEditOrderAsync(Order order);
        void NavigateToAddCategoryAsync();
        void NavigateToChartOrders();
    }
}
