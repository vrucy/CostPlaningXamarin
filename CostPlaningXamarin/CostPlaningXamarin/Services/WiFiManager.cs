using Android.Content;
using Android.Net.Wifi;
using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Services;
using Xamarin.Forms;
using CostPlaningXamarin.Models;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

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
        public async void SyncData()
        {
            SemaphoreSlim ss = new SemaphoreSlim(1);
            var appUser = SQLiteService.GetAppUser();

            if (SQLiteService.GetLastServerId<User>() != userService.GetLastUserServerId())
            {
                await ss.WaitAsync();
                await synchronizationService.SyncUsers(SQLiteService.GetLastServerId<User>());
                ss.Release();
            }
            if (SQLiteService.GetLastServerId<Category>() != categoryService.GetLastCategoryServerId() || SQLiteService.IsSyncData<Category>())
            {
                await ss.WaitAsync();
                await synchronizationService.SyncCategoies(SQLiteService.CategoriesForSync().Result, appUser.Id);
                ss.Release();
            }
            if (SQLiteService.GetLastServerId<Order>() != orderService.GetLastOrderServerId() || SQLiteService.IsSyncData<Order>())
            {
                await ss.WaitAsync();
                await synchronizationService.SyncOrders(SQLiteService.OrderForSync().Result);
                ss.Release();
            }
            await synchronizationService.SyncVisible<Category>(appUser.Id);
            await synchronizationService.SyncVisible<Order>(appUser.Id);
        }
        public void FristSyncData()
        {
            if (SQLiteService.IsFirstSyncNeed())
            {
                Task.Run(async () =>
                {
                    var users = userService.GetAllUsers().GetAwaiter().GetResult();

                    await SQLiteService.SaveItems(users);
                    await SQLiteService.SaveItems(categoryService.GetCategories());
                    await SQLiteService.SaveItems(orderService.GetAllOrders());
                }).Wait();
            }
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
