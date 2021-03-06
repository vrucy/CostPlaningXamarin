﻿using CostPlaningXamarin.Command;
using CostPlaningXamarin.Helper;
using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace CostPlaningXamarin.ViewModels
{
    public class OrdersViewModel : BaseViewModel
    {
        private List<Order> _orders;
        private List<Order> _allOrders;
        private Lazy<List<User>> _users;
        private Lazy<List<Category>> _categories;
        private ICommand _ClearAllFilters;
        private List<string> _DateFrom;
        private List<string> _DateTo;
        ISQLiteService _sqliteService = DependencyService.Get<ISQLiteService>();
        INavigationServices _navigationService = DependencyService.Get<INavigationServices>();


        public OrdersViewModel()
        {
            _allOrders = _sqliteService.GetOrdersAsync().GetAwaiter().GetResult();
           // _orders = _allOrders;
            _users = new Lazy<List<User>>(GetUsers);

            _categories = new Lazy<List<Category>>(GetCategories);
            Date = new List<string>();
            PopulateDateCollection();
        }
        private List<User> GetUsers()
        {
            return _sqliteService.GetUsers().GetAwaiter().GetResult();
        }
        private List<Category> GetCategories()
        {
            return _sqliteService.GetAllCategories().GetAwaiter().GetResult();
        }
        public List<Order> Orders
        {
            get
            {
                return _orders;
            }
            set
            {
                _orders = value;

                OnPropertyChanged(nameof(Orders));
                OnPropertyChanged("Cost");

            }
        }
        private Order _order;

        public Order Order
        {
            get
            {
                return _order;
            }
            set
            {
                _order = value;
                _navigationService.NavigateToEditOrderAsync(_order);
            }
        }
        private DateTime _selectedDate = DateTime.Now;

        public DateTime SelectedDate
        {
            get
            {
                return _selectedDate;
            }
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        public List<User> Users
        {
            get { return _users.Value; }
        }
        private User _selectedUser;

        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;

                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        public List<Category> Categories
        {
            get { return _categories.Value; }
        }
        private Category _selectedCategory;

        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));

            }
        }
        public List<string> DateFrom
        {
            get
            {
                if (_DateFrom != null)
                {
                    return _DateFrom;
                }
                else
                {
                    return PopulateDateCollection();
                }

            }
            set
            {
                _DateFrom = value;
                OnPropertyChanged(nameof(DateFrom));
            }
        }
        private string _visibility;

        public string Visibility
        {
            get 
            {
                if (_visibility == null)
                {
                    return "Enable";
                }
                return _visibility; 
            }
            set { _visibility = value; }
        }

        private string _DateFromSelected;

        public string DateFromSelected
        {
            get
            {
                return _DateFromSelected;
            }
            set
            {
                _DateFromSelected = value;
                if (_DateFromSelected == null)
                {
                    OnPropertyChanged(nameof(DateFromSelected));

                    return;
                }
                ChekerValueDateFromTo();
                OnPropertyChanged(nameof(DateFromSelected));


                OnPropertyChanged(nameof(Orders));
            }
        }
        private void ChekerValueDateFromTo()
        {
            if (_DateToSelected != null)
            {
                var dateFromIndex = PopulateDateCollection().FindIndex(y => y.Contains(_DateFromSelected));
                var dateToIndex = PopulateDateCollection().FindIndex(y => y.Contains(_DateToSelected));
                if (dateToIndex < dateFromIndex)
                {
                    DateToSelected = DateFromSelected;
                    OnPropertyChanged(nameof(DateToSelected));
                }
            }
        }
        public List<string> DateTo
        {
            get
            {
                if (_DateTo != null)
                {
                    return _DateTo;
                }
                else
                {
                    return PopulateDateCollection();
                }
            }
            set
            {
                _DateTo = value;
                OnPropertyChanged(nameof(DateTo));

            }
        }
        private string _DateToSelected;

        public string DateToSelected
        {
            get { return _DateToSelected; }
            set
            {
                _DateToSelected = value;
                if (_DateToSelected == null)
                {
                    OnPropertyChanged(nameof(DateToSelected));
                    return;
                }
                ChekerValueDateToFrom();
                OnPropertyChanged(nameof(DateToSelected));
            }
        }
        private void ChekerValueDateToFrom()
        {
            int dateFromIndex = 0;
            if (!String.IsNullOrEmpty(_DateFromSelected))
            {
                dateFromIndex = PopulateDateCollection().FindIndex(y => y.Contains(_DateFromSelected));
            }
            var dateToIndex = PopulateDateCollection().FindIndex(y => y.Contains(_DateToSelected));

            if (dateFromIndex > dateToIndex)
            {
                DateFromSelected = DateToSelected;

                OnPropertyChanged(nameof(DateFromSelected));
            }
        }

        private List<string> Date { get; set; }

        public List<string> PopulateDateCollection()
        {
            if (_orders == null)
            {
                _orders = _allOrders.Select(i => { i.Date.ToShortDateString(); return i; }).Where(o => o.Date.Month == DateTime.Now.Month && o.IsVisible == true).ToList();
                OnPropertyChanged(nameof(Orders));
            }
            foreach (var item in _allOrders)
            {
                Date.Add(string.Format("{0}/{1}", item.Date.ToString("MMM"), item.Date.ToString("yyyy")));
            }
            SortedSet<string> month = new SortedSet<string>(Date);

            return month.OrderBy(x => x.StringToDateTime().Month).ToList();
        }
        public ICommand ClearFilterCommand
        {
            get
            {
                if (_ClearAllFilters == null)
                {
                    _ClearAllFilters = new RelayCommand(ClearFilter);
                }
                return _ClearAllFilters;
            }
        }
        //clear all filter
        private void ClearFilter(object x)
        {
            _orders = _allOrders;
            _selectedUser = null;
            _selectedCategory = null;
            _DateFromSelected = null;
            _DateToSelected = null;
            OnPropertyChanged(nameof(SelectedUser));
            OnPropertyChanged(nameof(DateToSelected));
            OnPropertyChanged(nameof(DateFromSelected));
            OnPropertyChanged(nameof(SelectedCategory));
            OnPropertyChanged(nameof(Orders));

        }
        private ICommand _ApplyFilters;

        public ICommand ApplyFiltersCommand
        {
            get
            {
                if (_ApplyFilters == null)
                {
                    _ApplyFilters = new RelayCommand(ApplyFilters);
                }
                return _ApplyFilters;
            }
        }
        private List<Order> VisibleFilter(List<Order> orders)
        {
            if (_visibility.Equals( "Enable"))
            {
                return orders.Where(o=>o.IsVisible == true).ToList();
            }
            else if (_visibility.Equals("Disable"))
            {
                return orders.Where(o => o.IsVisible == false).ToList();
            }
            else
            {
                return orders;
            }
        }
        private void ApplyFilters(object x)
        {
            List<Order> _copy = new List<Order>();
            _copy = VisibleFilter(_allOrders);
            if (SelectedCategory != null)
            {
                _copy = _copy.Where(o => o.CategoryId == _selectedCategory.Id).ToList();
            }
            if (SelectedUser != null)
            {
                _copy = _copy.Where(o => o.UserId == _selectedUser.Id).ToList();
            }
            if (DateFromSelected != null && DateToSelected != null)
            {
                _copy = _copy.Where(o => o.Date.Month >= DateFromSelected.StringToDateTime().Month &&
                                            o.Date.Month <= DateToSelected.StringToDateTime().Month).ToList();
            }
            else if (DateFromSelected != null)
            {
                _copy = _copy.Where(o => o.Date.Month >= DateFromSelected.StringToDateTime().Month).ToList();
            }
            else if (DateToSelected != null)
            {
                var i = DateToSelected.StringToDateTime().Month;
                _copy = _copy.Where(o => o.Date.Month <= DateToSelected.StringToDateTime().Month).ToList();
            }
            _orders = _copy;
            OnPropertyChanged(nameof(Orders));
        }
        private ICommand _NavigateEdit;
        public ICommand NavigateToEditCommand
        {
            get
            {
                if (_NavigateEdit == null)
                {
                    _NavigateEdit = new RelayCommand(NavigateToEdit);
                }
                return _NavigateEdit;
            }
        }
        public void NavigateToEdit(object x)
        {
            _navigationService.NavigateToAddItem();
        }
    }
}
