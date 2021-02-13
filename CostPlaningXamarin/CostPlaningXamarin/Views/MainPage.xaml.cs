using CostPlaningXamarin.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CostPlaningXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
     
            InitializeComponent();
        }
        void settingButton_Clicked(object sender, EventArgs e)
        {
            navigationDrawer.ToggleDrawer();
        }
        private void ClickToShowPopUp(object sender, EventArgs e)
        {
            popupLayout.Show();
        }
    }
}