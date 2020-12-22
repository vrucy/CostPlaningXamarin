using Android.Content;
using Android.Net.Wifi;
using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Services;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;
using CostPlaningXamarin.Models;

[assembly: Xamarin.Forms.Dependency(typeof(WiFiManager))]
namespace CostPlaningXamarin.Services
{
    public class WiFiManager : IWiFiManager
    {
        private const string BSSID = "f8:5b:3b:5e:e1:bb";
        IOrderService orderService = DependencyService.Get<IOrderService>();
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
            if (appUser.WriteInDb == false)
            {
                FirstSyncUserOwner(appUser);
            }
            if (SQLiteService.GetLastUserServerId() != userService.GetLastUserServerId())
            {
                SyncUser();
            }
            if (SQLiteService.GetLastOrderServerId() != orderService.GetLastOrderServerId() || SQLiteService.CheckIfHaveOrderForSync())
            {
                SyncOrders(SQLiteService.OrderForSync().Result);

            }
            //potreban je jos jedan ya proveru dali postoje novi ord na serveru
        }
        private async void FirstSyncUserOwner(User appUser)
        {
            var serverUser = userService.PostAppUser().GetAwaiter().GetResult();

            SyncNewUsers(serverUser.Id);

            var orders = await SQLiteService.GetAllOrdersForUserById(appUser.Id);
            if (orders.Count > 0)
            {
                //potreban neki rollback ukiloko pukne veza
                PostOrders(orders);
            }
        }
        private void SyncNewUsers(int userId)
        {
            var allUsers = userService.GetAllUsers().Result;
            SQLiteService.PostNewUsers(allUsers);
            SQLiteService.UpdateDeviceUser(userId);
        }
        private void SyncUser()
        {

        }
        private void PostOrders(List<Order> orders)
        {
            orderService.PostOrdersSync(orders);
        }
        private void SyncOrders(List<Order> orders)
        {
            if (orders.Count != 0)
            {
                var ids = orderService.UpdateOrder(orders);
                SQLiteService.SyncOrders(ids);
            }
            if (SQLiteService.CheckIfHaveOrderForSync())
            {
                var ordersFromServer = orderService.GetOrdersByIds(SQLiteService.GetAllSyncOrdersIds().ToList());

                if (ordersFromServer.Count != 0)
                {
                    SQLiteService.SyncOrders(ordersFromServer);
                    SQLiteService.SaveAllOrders(ordersFromServer);
                }

            }
        }

    }
}
