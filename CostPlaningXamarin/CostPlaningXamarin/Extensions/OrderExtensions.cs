using CostPlaningXamarin.Models;
using System.Collections.Generic;
using System.Linq;

namespace CostPlaningXamarin.Extensions
{
    static class OrderExtensions
    {

        public static List<Order> VisibleOrders(this List<Order> orders )
        {
            return orders.Where(o => o.IsVisible == true).ToList();
        }
    }
}
