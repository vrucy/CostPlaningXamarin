using Android.Content;
using Android.Net.Wifi;
using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Services;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;
using CostPlaningXamarin.Models;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(WiFiManager))]
namespace CostPlaningXamarin.Services
{
    public class WiFiManager : IWiFiManager
    {
        private const string BSSID = "f8:5b:3b:5e:e1:bb";
        IOrderService orderService = DependencyService.Get<IOrderService>();
        ICategoryService categoryService = DependencyService.Get<ICategoryService>();
        IUserService userService = DependencyService.Get<IUserService>();
        ISQLiteService SQLiteService = DependencyService.Get<ISQLiteService>();

        public string GetCurrentSSID()
        {
            WifiManager wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
            WifiInfo info = wifiManager.ConnectionInfo;
            return info.BSSID;

        }
        public bool IsHomeWifiConnected()
        {
            //if (!String.IsNullOrEmpty(GetCurrentSSID()))
            //{
            //    if (GetCurrentSSID().Equals(BSSID))
            //    {
            //        return true;
            //    }
            //}
            //return false;
            return true;
        }
        public async void SyncData()
        {
            var appUser = SQLiteService.GetAppUser();

            if (appUser.Id == 1)
            
            {
                FirstSyncUserOwner(appUser);
                SQLiteService.SaveItems(categoryService.GetCategories());
            }
            if (SQLiteService.GetLastServerId<User>() != userService.GetLastUserServerId())
            {
                SyncUsers(SQLiteService.GetLastServerId<User>());
            }
            if (SQLiteService.GetLastServerId<Order>() != orderService.GetLastOrderServerId() || SQLiteService.IsSyncData<Order>())
            {
                SyncOrders(SQLiteService.OrderForSync().Result);
            }
            if (SQLiteService.GetLastServerId<Category>() != categoryService.GetLastCategoryServerId() || SQLiteService.IsSyncData<Category>())
            {
                SyncCategoies(SQLiteService.CategoriesForSync().Result);
            }

            //TODO: Da li treba da se proveri ukoliko nema ordera na serveru da se o5 pozeove FirstSyncUserOwner ili sta vec?
        }
        public bool IsServerAvailable()
        {
            return orderService.IsServerAvailable();
        }
        private async void FirstSyncUserOwner(User appUser)
        {
            var serverUser = userService.PostAppUser().GetAwaiter().GetResult();
            SQLiteService.DropTable<User>();
            SQLiteService.CreateTable<User>();
            SyncNewUsers(serverUser.Id);   
            var orders = await SQLiteService.GetAllOrdersForUserById(appUser.Id);
            if (orders.Count > 0)
            {
                //TODO: need some rollback if crash conn?
                PostOrders(orders);
            }
        }
        private void SyncNewUsers(int userId)
        {
            var allUsers = userService.GetAllUsers().Result;
            SQLiteService.PostNewUsers(allUsers);
            SQLiteService.UpdateDeviceUser(userId);
        }
        private void SyncUsers(int lastUserId)
        {
            var users = userService.GetUnsyncUsers(lastUserId);
            SQLiteService.PostNewUsers(users.Result);
        }
        private void PostOrders(List<Order> orders)
        {
            orderService.PostOrdersSync(orders);
        }
        //TODO: refactor
        private void SyncOrders(List<Order> orders)
        {
            if (orders.Count != 0)
            {
                var ids = orderService.UpdateOrder(orders);
                SQLiteService.SyncOrders(ids);
            }
            var ordersSync = SQLiteService.GetAllSyncIds<Order>().ToList();

            var ordersFromServer = orderService.GetOrdersByIds(ordersSync);

            if (ordersFromServer.Count != 0)
            {
                SQLiteService.SaveItems(ordersFromServer);
            }
        }
        private void SyncCategoies(List<Category> categories)
        {
            if (categories.Count != 0)
            {
                var ids = categoryService.UpdateCategories(categories);
                SQLiteService.SyncCategories(ids);
            }
            var categoriesSync = SQLiteService.GetAllSyncIds<Category>().ToList();
            var categoriesFromServer = categoryService.GetAllCategoriesByIds(categoriesSync);

            if (categoriesFromServer.Count != 0)
            {
                SQLiteService.SaveItems(categoriesFromServer);
            }
        }

    }
}
