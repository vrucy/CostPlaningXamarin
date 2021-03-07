using Android.Widget;
using CostPlaningXamarin.Command;
using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace CostPlaningXamarin.ViewModels
{
    public class EditPageViewModel : BaseViewModel
    {
        private bool _isOrder;
        private bool _isCategory;
        private Category _category;
        private List<Category> _categories;
        private Order _order;
        private RelayCommand _EditCommand;
        ISQLiteService SQLService = DependencyService.Get<ISQLiteService>();
        ICategoryService categoryService = DependencyService.Get<ICategoryService>();
        IOrderService orderService = DependencyService.Get<IOrderService>();

        //TODO: Generic make editpage.xaml.cs recive T
        public EditPageViewModel()
        {
            _categories = SQLService.GetAllCategories().Result;
        }

        public List<Category> Categories
        {
            get { return _categories; }
            set 
            {
                _categories = value;
                OnPropertyChanged("Categories");
            }
        }

        public Category Category
        {
            get 
            {
                return _category; 
            }
            set 
            {
                _category = value;
                IsCategory = true;
                OnPropertyChanged("Category");
                OnPropertyChanged("IsCategory");
            }
        }
        public Order Order
        {
            get { return _order; }
            set 
            {
                _order = value;
                IsOrder = true;
                OnPropertyChanged("Order");
                OnPropertyChanged("ChangedDate");
                OnPropertyChanged("IsOrder");
            }
        }
        public bool IsOrder
        {
            get 
            { 
                return _isOrder && IsLoaded(); 
            }
            set 
            {
                _isOrder = value;
                OnPropertyChanged("Order");
                OnPropertyChanged("IsOrder");
            }
        }

        private bool IsLoaded()
        {
            if (Order != null)
            {
                return true;
            }
            return false;
        }
        public bool IsCategory
        {
            get { return _isCategory; }
            set 
            {
                _isCategory = value;
                OnPropertyChanged(nameof(IsCategory));

            }
        }

        public RelayCommand EditCommand
        {
            get
            {
                if (_EditCommand == null)
                {
                    _EditCommand = new RelayCommand(Edit);
                }

                return _EditCommand;
            }
        }
        private void Edit(object x)
        {
            var user = SQLService.GetAppUser();

            if (_category != null)
            {
                _category.IsVisible = !_category.IsVisible;
                if (categoryService.EditCategory(_category, user.Id))
                {
                    SQLService.Visibility(_category, _category.IsVisible);
                    Toast.MakeText(Android.App.Application.Context, "Success", ToastLength.Long).Show();
                }
                else
                {
                    _category.IsVisible = !_category.IsVisible;
                    Toast.MakeText(Android.App.Application.Context, "Error", ToastLength.Long).Show();
                }
            }
            if (_order != null)
            {
                _order.IsVisible = !_order.IsVisible;
                if (orderService.EditOrder(_order, user.Id))
                {
                    SQLService.Visibility(_order, _order.IsVisible);
                    Toast.MakeText(Android.App.Application.Context, "Success", ToastLength.Long).Show();

                }
                else
                {
                    _order.IsVisible = !_order.IsVisible;
                    Toast.MakeText(Android.App.Application.Context, "Error", ToastLength.Long).Show();
                }

            }
        }
    }
}
