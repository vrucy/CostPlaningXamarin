using Xamarin.Forms;
using CostPlaningXamarin.Views;
using System.IO;
using System;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using System.Linq;
using CostPlaningXamarin.Interfaces;

namespace CostPlaningXamarin
{
    public partial class App : Application
    {
            IWiFiManager wiFiManager = DependencyService.Get<IWiFiManager>();
        public App()
        {
            
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzIxMTI4QDMxMzgyZTMyMmUzMGd3V0RLcis2Q0RmdTlhejE2Z1JnNUlObEh2aHZkMndGanpucjZKRjVvZ3M9");
            ISQLiteService SQLiteService = DependencyService.Get<ISQLiteService>();
            Device.SetFlags(new string[] { "Expander_Experimental" });
            InitializeComponent();
            SQLiteService.CreateDBAsync(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "XamarinSQLite.db3"));
            MainPage = new NavigationPage(new MainPage());
            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                if (CrossConnectivity.Current.ConnectionTypes.Contains(ConnectionType.WiFi))
                {
                    wiFiManager.SyncData();
                }
            };
        }
        
        protected override void OnStart()
        {
            if (wiFiManager.IsHomeWifiConnected())
            {
                wiFiManager.SyncData();
            }
        }

        protected override void OnSleep()
        {

        }
        protected override void OnResume()
        {
            if (wiFiManager.IsHomeWifiConnected())
            {
                wiFiManager.SyncData();
            }
        }
        
    }
}
