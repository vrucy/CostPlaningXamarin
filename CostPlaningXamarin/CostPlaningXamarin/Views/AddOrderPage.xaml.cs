using CostPlaningXamarin.ViewModels;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CostPlaningXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddItemPage : ContentPage
    {

        public AddItemPage()
        {
            InitializeComponent();
        }

        private void OpenPicer(object sender, System.EventArgs e)
        {
            this.DatePicer.IsOpen = true;
            this.DatePicer.MaximumDate = DateTime.Now;
            
        }
    }
}