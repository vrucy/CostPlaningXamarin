using CostPlaningXamarin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CostPlaningXamarin.Interfaces
{
    interface IOrderService
    {
        /// <summary>
        /// Orders post on server
        /// </summary>
        /// <param name="orders"></param>
        /// <returns>Returns Dictionary(int oldKey, int newKey)</returns>
        Dictionary<int, int> UpdateOrder(List<Order> orders);
        List<Order> GetOrdersByIds(List<int> ids);
        List<Order> GetAllOrders();
        int GetLastOrderServerId();
        Dictionary<int, bool> GetAllOrdersVisibility(int appUserId);
        bool EditOrder(Order order, int userId);
        Task PostOrder(Order order);
    }
}
