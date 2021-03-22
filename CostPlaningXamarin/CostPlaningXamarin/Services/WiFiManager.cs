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
            var deviceId = SQLiteService.GetCurrentDeviceInfo().DeviceId;

            if (SQLiteService.GetLastServerId<User>() != await userService.GetLastUserServerId())
            {
                await ss.WaitAsync();
                await synchronizationService.SyncUsers(SQLiteService.GetLastServerId<User>());
                ss.Release();
            }
            if (SQLiteService.GetLastServerId<Category>() != await categoryService.GetLastCategoryServerId() )
            {
                await ss.WaitAsync();
                await synchronizationService.SyncCategoies(SQLiteService.GetLastServerId<Category>());
                ss.Release();
            }
            if (SQLiteService.GetLastServerId<Order>() != await orderService.GetLastOrderServerId() || SQLiteService.IsSyncData<Order>())
            {
                await ss.WaitAsync();
                await synchronizationService.SyncOrders(SQLiteService.OrderForSync().Result, deviceId);
                ss.Release();
            }
            await synchronizationService.SyncVisible<Category>(deviceId);
            await synchronizationService.SyncVisible<Order>(deviceId);
        }
        public async Task FristSyncData()
        {
            if (SQLiteService.IsFirstSyncNeed())
            {
                var users = await userService.GetAllUsers();

                await SQLiteService.SaveItems(users);
            }
        }
        public async Task FirstSyncOrders()
        { 
            await SQLiteService.SaveItems(await orderService.GetAllOrders(SQLiteService.GetCurrentDeviceInfo().DeviceId));
        }
        public async Task FirstSyncCategories()
        {
            await SQLiteService.SaveItems(await categoryService.GetCategories(SQLiteService.GetCurrentDeviceInfo().DeviceId));
            
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
