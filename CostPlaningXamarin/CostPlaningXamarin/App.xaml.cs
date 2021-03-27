using Xamarin.Forms;
using CostPlaningXamarin.Views;
using System.IO;
using System;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using System.Linq;
using CostPlaningXamarin.Interfaces;
using Android;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;
using System.Threading;

namespace CostPlaningXamarin
{
    public partial class App : Application
    {
        IWiFiManager wiFiManager = DependencyService.Get<IWiFiManager>();
        ISQLiteService SQLiteService = DependencyService.Get<ISQLiteService>();

        public App()
        {

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzkyOTk2QDMxMzgyZTM0MmUzMEpkeHU4SVVVRHdwWkZYSnp2blRwcVBZWUZIa0pLRWpHdGU4d3BES3pCQ3c9");
            Device.SetFlags(new string[] { "Expander_Experimental" });
            InitializeComponent();
            SQLiteService.CreateDBAsync(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "XamarinSQLite.db3")).Wait();
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
                if (CrossConnectivity.Current.ConnectionTypes.Contains(ConnectionType.WiFi) && wiFiManager.IsServerAvailable())
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
