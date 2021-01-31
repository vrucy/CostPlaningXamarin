using CostPlaningXamarin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CostPlaningXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditPage: ContentPage 
    {
        //TODO: Make generic recive T and then crate filds for them.
        public EditPage()
        {
            InitializeComponent();
        }
        public EditPage(Category category)
        {
            InitializeComponent();
            viewModel.Category = category;
        }
        public EditPage(Order order)
        {
            InitializeComponent();
            viewModel.Order = order;
        } 
        
    }
}