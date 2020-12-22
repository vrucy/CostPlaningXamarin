
using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Services;
using CostPlaningXamarin.Views;
using System.Linq;
using Xamarin.Forms;
[assembly: Xamarin.Forms.Dependency(typeof(NavigationService))]
namespace CostPlaningXamarin.Services
{
    public class NavigationService : INavigationServices
    {
        //TODO: make mathod to recive parameter and push page what write in these par. 
        public async void NavigateToAddItem()
        {
            var currentPage = GetCurrentPage();
            await currentPage.Navigation.PushAsync(new AddItemPage());
        }
        public async void NavigateToTableOrders()
        {
            var currentPage = GetCurrentPage();
            await currentPage.Navigation.PushAsync(new SortTable());
            //await currentPage.Navigation.PushAsync(new TableOrdersMasterPage());
        }
        public async void NavigateBack()
        {
            var currentPage = GetCurrentPage();

            await currentPage.Navigation.PopAsync();
        }

        private Page GetCurrentPage()
        {
            var currentPage = Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault();

            return currentPage;
        }

        public void NavigateToAddUser()
        {
            var currentPage = GetCurrentPage();
            currentPage.Navigation.PushAsync(new AuthPage());
        }

        public async void NavigateToMainPage()
        {
            await GetCurrentPage().Navigation.PushAsync(new MainPage());
        }
    }
}
