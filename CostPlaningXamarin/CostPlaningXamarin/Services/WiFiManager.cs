using Android.Content;
using Android.Net.Wifi;
using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Services;
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
        ISynchronizationService synchronizationService = DependencyService.Get<ISynchronizationService>();

        public string GetCurrentSSID()
        {
            WifiManager wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
            WifiInfo info = wifiManager.ConnectionInfo;
            return info.BSSID;

        }
        public bool IsHomeWifiConnected()
        {
            if (!String.IsNullOrEmpty(GetCurrentSSID()))
            {
                if (GetCurrentSSID().Equals(BSSID))
                {
                    return true;
                }
            }
            return false;
            //return true;
        }
        private bool CheckFirstAppUser(User appUser)
        {
            if ((appUser.Id == 1) && (userService.GetLastUserServerId() == 1))
            {
                return false;
            }
            if (appUser.Id == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async void SyncData()
         {
            var appUser = SQLiteService.GetAppUser();
            //TODO: bilo je jedan alli sam stavio sad nula provari sta treba!
            //ne valja jer kad se prvi user instalira on svakako bude 1 na serveru treba drugi uslov
            if (CheckFirstAppUser(appUser))
            {
                synchronizationService.FirstSyncUserOwner(appUser);
                SQLiteService.SaveItems(categoryService.GetCategories());
            }
            if (SQLiteService.GetLastServerId<User>() != userService.GetLastUserServerId())
            {
                synchronizationService.SyncUsers(SQLiteService.GetLastServerId<User>());
            }
            if (SQLiteService.GetLastServerId<Category>() != categoryService.GetLastCategoryServerId() || SQLiteService.IsSyncData<Category>())
            {
                synchronizationService.SyncCategoies(SQLiteService.CategoriesForSync().Result, appUser.Id);
            }
            if (SQLiteService.GetLastServerId<Order>() != orderService.GetLastOrderServerId() || SQLiteService.IsSyncData<Order>())
            {
                synchronizationService.SyncOrders(SQLiteService.OrderForSync().Result);
            }
            synchronizationService.SyncVisible<Category>(appUser.Id);

            //TODO: Da li treba da se proveri ukoliko nema ordera na serveru da se o5 pozeove FirstSyncUserOwner ili sta vec?
        }
        public bool IsServerAvailable()
        {
            return orderService.IsServerAvailable();
        }
        
    }
}
