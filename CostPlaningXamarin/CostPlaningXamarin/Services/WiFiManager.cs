using Android.Content;
using Android.Net.Wifi;
using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Services;
using Xamarin.Forms;
using CostPlaningXamarin.Models;
using System;
using System.Net.Sockets;

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
        }
        private bool CheckFirstAppUser(User appUser)
        {
            var lastServerId = userService.GetLastUserServerId();
            if (lastServerId == 0)
            {
                return true;
            }
            else if (appUser.Id == 1 && lastServerId != 1)
            {
                return true;
            }

            return false;
        }
        public async void SyncData()
        {
            var appUser = SQLiteService.GetAppUser();

            if (CheckFirstAppUser(appUser))
            {
                await synchronizationService.FirstSyncUserOwner(appUser);
                await SQLiteService.SaveItems(categoryService.GetCategories());
            }
            if (SQLiteService.GetLastServerId<User>() != userService.GetLastUserServerId())
            {
                await synchronizationService.SyncUsers(SQLiteService.GetLastServerId<User>());
            }
            if (SQLiteService.GetLastServerId<Category>() != categoryService.GetLastCategoryServerId() || SQLiteService.IsSyncData<Category>())
            {
                await synchronizationService.SyncCategoies(SQLiteService.CategoriesForSync().Result, appUser.Id);
            }
            if (SQLiteService.GetLastServerId<Order>() != orderService.GetLastOrderServerId() || SQLiteService.IsSyncData<Order>())
            {
                await synchronizationService.SyncOrders(SQLiteService.OrderForSync().Result);
            }
            await synchronizationService.SyncVisible<Category>(appUser.Id);
            await synchronizationService.SyncVisible<Order>(appUser.Id);
        }
        public bool IsServerAvailable()
        {

            TcpClient tcpClient = new TcpClient();
            if (!tcpClient.ConnectAsync("192.168.1.88", 80).Wait(3000))
            {
                return false;
            }
            else
            {
                return true;
            }

        }

    }
}
