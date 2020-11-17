using CostPlaningXamarin.Helper;
using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using CostPlaningXamarin.ViewModels;
using Syncfusion.SfDataGrid.XForms;
using System;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CostPlaningXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TableOrdersPage : ContentPage
    {
        public TableOrdersPage()
        {
            InitializeComponent();
            
        }
       
        private async void DataGrid_ItemSelected(object sender, GridTappedEventsArgs e)
        {
            var order = (Order)e.RowData;
            await DisplayAlert("Description", order.Description , "Close");
        }
        void hamburgerButton_Clicked(object sender, EventArgs e)
        {
            navigationDrawer.ToggleDrawer();
        }
    }
}