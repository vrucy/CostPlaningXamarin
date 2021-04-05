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
        private readonly NLog.ILogger _logger = NLog.LogManager.GetCurrentClassLogger();

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
            SemaphoreSlim ss = new SemaphoreSlim(1);
            var deviceId = SQLiteService.GetCurrentDeviceInfo().DeviceId;
            await ss.WaitAsync();
            var users = await userService.GetUnsyncUsers(SQLiteService.GetLastServerId<User>());
            if (users.Any())
            {
                await SQLiteService.SaveItems(users);
            }
            ss.Release();

            await ss.WaitAsync();
            var categories = await categoryService.GetUnsyncCategories(deviceId);
            if (categories.Any())
            {
                await SQLiteService.SaveItems(categories);
            }
            ss.Release();

            await ss.WaitAsync();
            var orders = await orderService.GetUnsyncOrders(deviceId);
            if (orders.Any() || SQLiteService.IsSyncData<Order>())
            {
                await synchronizationService.SyncOrders(await SQLiteService.OrderForSync(),orders ,deviceId);
            }
            ss.Release();
        }
        public async void FristSyncData()
        {
            if (SQLiteService.IsFirstSyncNeed())
            {
                var users = userService.GetAllUsers().GetAwaiter().GetResult();

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
            try
            {
                TcpClient tcpClient = new TcpClient();
                if (tcpClient.ConnectAsync("192.168.1.88", 80).Wait(2000))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception e)
            {
                _logger.Error("IsServerAvailable error: " + e.Message + "Inner: " + e.InnerException);
                return false;
            }

        }

    }
}
