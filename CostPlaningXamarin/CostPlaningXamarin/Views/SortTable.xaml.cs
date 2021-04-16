using CostPlaningXamarin.Models;
using Syncfusion.SfDataGrid.XForms;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CostPlaningXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SortTable : ContentPage
    {
        public SortTable()
        {
            InitializeComponent();
        }
        private async void DataGrid_ItemSelected(object sender, GridTappedEventsArgs e)
        {
            var order = (Order)e.RowData;
            if (order != null)
            {
                await DisplayAlert("Description", order.Description, "Close");
            }
        }
        void hamburgerButton_Clicked(object sender, EventArgs e)
        {
            navigationDrawer.ToggleDrawer();
        }
    }
}