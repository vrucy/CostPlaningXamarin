using CostPlaningXamarin.ViewModels;
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
            BindingContext = new UserViewModel();
            
        }
        
    }
}