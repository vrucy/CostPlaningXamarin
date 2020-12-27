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
            if (appUser.ServerId == 0)
            {
                FirstSyncUserOwner(appUser);
            }
            if (SQLiteService.GetLastUserServerId() != userService.GetLastUserServerId())
            {
                SyncUsers(SQLiteService.GetLastUserServerId());
            }
            if (SQLiteService.GetLastOrderServerId() != orderService.GetLastOrderServerId() || SQLiteService.CheckIfHaveOrderForSync())
            {
                SyncOrders(SQLiteService.OrderForSync().Result);
            }

            //TODO: Da li treba da se proveri ukoliko nema ordera na serveru da se o5 pozeove FirstSyncUserOwner ili sta vec?
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
                //TODO: potreban neki rollback ukiloko pukne veza
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
            //TODO: nije dobro treba kao i orderi da salje Id-eve i da uporedi i da vrati novog jer se moze izbrisati
            //uraditi proveru zadnjeg id jer ako se izbrise iz baze dolazi novi +1
            var users = userService.GetUnsyncUsers(lastUserId);
            SQLiteService.PostNewUsers(users.Result);
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
            var ordersSync = SQLiteService.GetAllSyncOrdersIds().ToList();
            //nije dobar uslov kad se bude brisalo mogu biti isti brojevi potrebnno proveriti zadnji id!
            if (ordersSync.Count != orderService.GetOrdersCountFromServer())
            {
                var ordersFromServer = orderService.GetOrdersByIds(ordersSync);

                if (ordersFromServer.Count != 0)
                {
                    SQLiteService.SaveOrders(ordersFromServer);
                }

            }
        }

    }
}
