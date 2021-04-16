using CostPlaningXamarin.Extensions;
using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;
using System;
using System.Globalization;

namespace CostPlaningXamarin.ViewModels
{
    public class ChartOrdersViewModel
    {
        ISQLiteService _sqliteService = DependencyService.Get<ISQLiteService>();
        //private List<Order> _allOrders;

        public ChartOrdersViewModel()
        {
            _orders = _sqliteService.GetOrdersAsync().Result.VisibleOrders();
            CreateGraphicData();
        }
        private List<dynamic> _graphicData = new List<dynamic>();
        public List<dynamic> GraphicData
        {
            get { return _graphicData; }
            set { _graphicData = value; }
        }


        private List<Order> _orders;
        public List<Order> Orders
        {
            get { return _orders; }
            set { _orders = value; }
        }
        private void CreateGraphicData()
        {
            foreach (var item in _orders.GroupBy(x => new { x.Date.Month ,x.Date.Year}))
            {
                var year = item.Key.Year.ToString().Substring( 2);
                var month = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(item.Key.Month);
                var date = month + "/" + year;
                
                GraphicData.Add(new { Date = date, Cost = item.Sum(x => x.Cost) });
            }
        }
    }
    }
