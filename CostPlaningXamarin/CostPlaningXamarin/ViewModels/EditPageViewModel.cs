using Android.Widget;
using CostPlaningXamarin.Command;
using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
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
        private bool isChaked;

        public bool IsChaked
        {
            get { return isChaked; }
            set 
            {
                _order.IsVisible = !value;
                OnPropertyChanged(nameof(Order));
            }
        }
        private bool isChakedCat;

        public bool IsChakedCat
        {
            get { return isChakedCat; }
            set
            {
                _category.IsVisible = !value;
                OnPropertyChanged(nameof(Category));
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
        private async void Edit(object x)
        {
            var device = SQLService.GetCurrentDeviceInfo().DeviceId;

            if (_category != null)
            {
                if (await categoryService.EditCategory(_category, device))
                {
                    await SQLService.Visibility(_category, _category.IsVisible);
                    Toast.MakeText(Android.App.Application.Context, "Success", ToastLength.Long).Show();
                }
                else
                {
                    _category.IsVisible = !_category.IsVisible;
                    Toast.MakeText(Android.App.Application.Context, "Error", ToastLength.Long).Show();
                }
                _category = null;
            }
            if (_order != null)
            {
                if (await orderService.EditOrder(_order, device))
                {
                    await SQLService.Visibility(_order, _order.IsVisible);
                    Toast.MakeText(Android.App.Application.Context, "Success", ToastLength.Long).Show();

                }
                else
                {
                    _order.IsVisible = !_order.IsVisible;
                    Toast.MakeText(Android.App.Application.Context, "Error", ToastLength.Long).Show();
                }
                _order = null;
            }
        }
    }
}
