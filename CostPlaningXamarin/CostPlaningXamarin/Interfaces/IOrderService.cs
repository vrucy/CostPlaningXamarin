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
        Task<Dictionary<int, int>> UpdateOrder(List<Order> orders, string deviceId);
        List<Order> GetOrdersByIds(List<int> ids);
        List<Order> GetAllOrders(string deviceId);
        int GetLastOrderServerId();
        Dictionary<int, bool> GetAllOrdersVisibility(string deviceId);
        bool EditOrder(Order order, string deviceId);
        Task PostOrder(Order order, string deviceId);
    }
}
