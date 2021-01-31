using Xamarin.Forms;
using CostPlaningXamarin.Views;
using System.IO;
using System;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using System.Linq;
using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Services;
using System.Threading.Tasks;

namespace CostPlaningXamarin
{
    public partial class App : Application
    {
        IWiFiManager wiFiManager = DependencyService.Get<IWiFiManager>();
        public App()
        {

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzYzMTEzQDMxMzgyZTMzMmUzME1kUlJ4QVpjZUNjODFDRDhqcG5QbVFlV1V2L3R6M2thTzh6aVR0KytCODg9");
            ISQLiteService SQLiteService = DependencyService.Get<ISQLiteService>();
            Device.SetFlags(new string[] { "Expander_Experimental" });
            InitializeComponent();
            SQLiteService.CreateDBAsync(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "XamarinSQLite.db3")).Wait();
            // SQLiteService.DeleteAllUsers();
            if (SQLiteService.CheckIfExistUser())
            {
                MainPage = new NavigationPage(new AuthPage());
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());

            }
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
            ISQLiteService SQLiteService = DependencyService.Get<ISQLiteService>();
            if (wiFiManager.IsHomeWifiConnected() && !SQLiteService.CheckIfExistUser() && wiFiManager.IsServerAvailable())
            {
                wiFiManager.SyncData();
            }
        }

        protected override void OnSleep()
        {

        }
        protected override void OnResume()
        {
            ISQLiteService SQLiteService = DependencyService.Get<ISQLiteService>();

            if (wiFiManager.IsHomeWifiConnected() && !SQLiteService.CheckIfExistUser() && wiFiManager.IsServerAvailable())
            {
                wiFiManager.SyncData();
            }
       }

    }
}
