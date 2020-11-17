using Android.Widget;
using CostPlaningXamarin.Command;
using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
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
        private Category _selectedCategory;
        private ICommand _SubmitCommand;
        IOrderService orderService = DependencyService.Get<IOrderService>();
        ISQLiteService SQLService = DependencyService.Get<ISQLiteService>();
        IWiFiManager wiFiManager = DependencyService.Get<IWiFiManager>();

        public AddNewOrderViewModel()
        {
            _order = new Order();
           
            _categories = SQLService.GetAllCategories().Result;
        }
        private DateTime _previusDate;

        public DateTime PreviusDate
        {
            get { return _previusDate; }
            set {
                _previusDate = value;
                _order.Date = _previusDate;
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
                _order = value;
                OnPropertyChanged();
                //PropertyValidation();
            }
        }

        public List<Category> Categories
        {
            get { return _categories; }
            set { _categories = value; }
        }
        [Required(ErrorMessage = "Category should not be empty")]
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set 
            {
                _selectedCategory = value;
                _order.CategoryId = _selectedCategory.Id;
                _order.Category = _selectedCategory;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }
        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public ICommand SubmitCommand
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
            wiFiManager.IsHomeWifiConnected();
            //TODO: how know who is user? when use fingerprint
            try
            {
                if (_order.Date == default(DateTime))
                {
                    _order.Date = DateTime.Now;
                }
                    
                //SQLService.SaveOrderAsync(_order);
                //reset all fields to null
                _selectedCategory = null;
                
                Toast.MakeText(Android.App.Application.Context,"Success",ToastLength.Long).Show(); 
            }
            catch (Exception e)
            {
                Toast.MakeText(Android.App.Application.Context, "Error", ToastLength.Long).Show();
                throw;
            }
        }

    }
}
