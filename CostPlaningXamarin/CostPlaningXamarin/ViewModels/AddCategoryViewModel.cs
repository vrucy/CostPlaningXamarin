using Android.Widget;
using CostPlaningXamarin.Command;
using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CostPlaningXamarin.ViewModels
{
    public class AddCategoryViewModel: BaseViewModel
    {
        private Category _category;
        private RelayCommand _SubmitCommand;
        INavigationServices _navigationService = DependencyService.Get<INavigationServices>();
        ISQLiteService SQLService = DependencyService.Get<ISQLiteService>();

        public AddCategoryViewModel()
        {
            _category = new Category();
        }

        public Category Category
        {
            get { return _category; }
            set 
            {
                _category = value;
                OnPropertyChanged("Category");
            }
        }
        public RelayCommand SubmitCommand
        {
            get
            {
                if (_SubmitCommand == null)
                {

                    _SubmitCommand = new RelayCommand(Submit);
                }
                return _SubmitCommand;
            }
        }
        private void Submit(object x)
        {
            try
            {
                _category.IsVisible = !_category.IsVisible;
                SQLService.SaveAsync(_category);
                Toast.MakeText(Android.App.Application.Context, "Success", ToastLength.Long).Show();

                _navigationService.NavigateToMainPage();
            }
            catch (Exception)
            {
                Toast.MakeText(Android.App.Application.Context, "Error", ToastLength.Long).Show();
                throw;
            }
        }
    }
}
