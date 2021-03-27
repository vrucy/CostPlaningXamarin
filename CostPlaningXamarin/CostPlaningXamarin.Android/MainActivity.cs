using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Reflection;
using CostPlaningXamarin.Services;
using Android;
using AndroidX.Core.App;

namespace CostPlaningXamarin.Droid
{
    [Activity(Label = "CostPlaningXamarin", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            if (ActivityCompat.ShouldShowRequestPermissionRationale(this,
                        Manifest.Permission.WriteExternalStorage))
            {
            }
            else
            {
                ActivityCompat.RequestPermissions(this,
                                       new String[] { Manifest.Permission.WriteExternalStorage },
                                       100);
            }

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            InitializeNLog();
            LoadApplication(new App());

        }

        private void InitializeNLog()
        {
            Assembly assembly = this.GetType().Assembly;
            string assemblyName = assembly.GetName().Name;
            new LogService().Initialize(assembly, assemblyName);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}