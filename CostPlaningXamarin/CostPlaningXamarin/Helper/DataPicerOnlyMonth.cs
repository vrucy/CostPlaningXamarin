using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using Syncfusion.Data.Extensions;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace CostPlaningXamarin.Helper
{
    public class DataPicerOnlyMonth 
    {

        public List<string> Date { get; set; }
        ISQLiteService sqliteService = DependencyService.Get<ISQLiteService>();

        private List<Order> _orders;

        public DataPicerOnlyMonth()
        {
            Date = new List<string>();
            _orders = sqliteService.GetOrdersAsync().Result;
        }
        public List<string> PopulateDateCollection()
        {
            foreach (var item in _orders)
            {
                Date.Add(string.Format("{0}/{1}", item.Date.ToString("MMM"), item.Date.ToString("yy")));
            }
            HashSet<string> month = new HashSet<string>(Date);
            return month.ToList();
        }

    }
}
