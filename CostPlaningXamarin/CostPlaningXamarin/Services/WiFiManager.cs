using Android.Content;
using Android.Net.Wifi;
using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Services;
using System;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(WiFiManager))]
namespace CostPlaningXamarin.Services
{
    public class WiFiManager : IWiFiManager
    {
        private const string BSSID = "f8:5b:3b:5e:e1:bb";
        IOrderService orderService = DependencyService.Get<IOrderService>();
        ISQLiteService SQLiteService = DependencyService.Get<ISQLiteService>();
        

        public bool IsHomeWifiConnected()
        {
            WifiManager wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
            WifiInfo info = wifiManager.ConnectionInfo;
            if (!String.IsNullOrEmpty(info.BSSID))
            {
                if (info.BSSID.Equals(BSSID))
                {
                    return true;
                }
            }
            return false;
            //return true;
        }
        //TODO very expencive sync not good solution. must change in second version !!!!!
        public async void SyncData()
        {
            
                await orderService.PostOrders();
                SQLiteService.DeleteAllOrders();
                SQLiteService.SaveAllOrders(await orderService.GetAllOrders());
            
        }
    }
}
