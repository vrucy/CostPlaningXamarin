﻿
using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
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
            await GetCurrentPage().Navigation.PushAsync(new AddItemPage());
        }
        public async void NavigateToTableOrders()
        {
            await GetCurrentPage().Navigation.PushAsync(new SortTable());
            //await currentPage.Navigation.PushAsync(new TableOrdersMasterPage());
        }
        public async void NavigateBack()
        {
            await GetCurrentPage().Navigation.PopAsync();
        }

        private Page GetCurrentPage()
        {
            var currentPage = Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault();

            return currentPage;
        }

        public void NavigateToAddUser()
        {
            GetCurrentPage().Navigation.PushAsync(new AuthPage());
        }

        public async void NavigateToMainPage()
        {
            await GetCurrentPage().Navigation.PushAsync(new MainPage());
        }

        public async void NavigateToEditCategory()
        {
            await GetCurrentPage().Navigation.PushAsync(new EditPage(new Category()));
        }

        public async void NavigateToOrdersOptions()
        {
            await GetCurrentPage().Navigation.PushAsync(new OrdersPage());
        }

        public async void NavigateToEditOrderAsync(Order order)
        {
            await GetCurrentPage().Navigation.PushAsync(new EditPage(order));
        }

        public async void NavigateToAddCategoryAsync()
        {
            await GetCurrentPage().Navigation.PushAsync(new AddCategory());
        }
    }
}
