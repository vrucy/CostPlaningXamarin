using Android.Widget;
using CostPlaningXamarin.Command;
using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace CostPlaningXamarin.ViewModels
{
    public class AddNewOrderViewModel: BaseViewModel
    {
        public ICommand NavigateCommand { get; private set; }
        public ICommand NavigateBackCommand { get; private set; }
        private Order _order;
        private List<Category> _categories;
        private RelayCommand _SubmitCommand;
        ISQLiteService SQLService = DependencyService.Get<ISQLiteService>();

        public AddNewOrderViewModel()
        {
            _order = new Order();
            _categories = SQLService.GetAllCategories().Result;
        }

        private DateTime? _previusDate;

        public DateTime? PreviusDate
        {
            get { return _previusDate; }
            set
            {
                _previusDate = value;
                _order.Date = (DateTime)_previusDate;
                OnPropertyChanged(nameof(PreviusDate));
            }
        }
        public Order Order
        {
            get
            {
                return _order;
            }
            set
            {
                OnPropertyChanged(nameof(Order));
                if (_order.Cost > 0.00)
                {
                    SubmitCommand.RaiseCanExecuteChanged();
                }
                _order = value;
               
            }
        }
        public List<Category> Categories
        {
            get { return _categories; }
            set { _categories = value; }
        }
        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get 
            {
                return _selectedCategory; 
            }
            set 
            {
                _selectedCategory = value;
                _order.CategoryId = _selectedCategory.Id;
                _order.Category = _selectedCategory;
                if (Validate())
                {
                    SubmitCommand.RaiseCanExecuteChanged();
                }
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }
        private double _cost;

        public double Cost
        {
            get { return _cost; }
            set
            {
                _cost = value;
                _order.Cost = value;
                if (Validate())
                {
                    SubmitCommand.RaiseCanExecuteChanged();
                }
                OnPropertyChanged("Cost");
            }
        }
        private bool Validate()
        {
            bool isCostField = ValidateCostField();
            bool isCategoryField = ValidateCategoryField();

            return isCostField && isCategoryField;
        }
        private bool ValidateCostField()
        {
           return CheckValue(_cost);
       }
        private bool ValidateCategoryField()
        {
            return CheckValue(_selectedCategory);
        }
        private bool CheckValue<T>(T value)
        {
            if (value == null)
            {
                return false;
            }

            if (typeof(T) == typeof(double))
            {
                double d = Convert.ToDouble(value);
                return d > 0;
            }
            if (typeof(T) == typeof(int))
            {
                int i = Convert.ToInt32(value);
                return i > 0;
            }
            return true;
        }
        
        public RelayCommand SubmitCommand
        {
            get
            {
                if (_SubmitCommand == null)
                {
                    
                    _SubmitCommand = new RelayCommand(
                        Submit, CanSend);
                }

                return _SubmitCommand;
            }
        }
        private bool CanSend(object x)
        {
            return Validate();
        }
        private void Submit(object x)
        {
            try
            {
                 SQLService.SaveOrderAsync(CreateOrder());
                ResetField();

                Toast.MakeText(Android.App.Application.Context,"Success",ToastLength.Long).Show(); 
            }
            catch (Exception e)
            {
                Toast.MakeText(Android.App.Application.Context, "Error", ToastLength.Long).Show();
                throw;
            }
        }
        //TODO: need reset selected category problem with nullable object in picer
        private void ResetField()
        {
            //_selectedCategory = null;
            _order = new Order();
            _cost = 0;
            //OnPropertyChanged("SelectedCategory");
            OnPropertyChanged("Order");
            OnPropertyChanged("Cost");
        }
        private Order CreateOrder()
        {
            if (_order.Date == default(DateTime))
            {
                _order.Date = DateTime.Now;
            }
            _order.UserId = SQLService.GetAppUser().Id;
            _order.User = SQLService.GetAppUser();
            return _order;
        }
    }
}
;