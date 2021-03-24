using CostPlaningXamarin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CostPlaningXamarin.Interfaces
{
    interface IOrderService
    {
        Task<Dictionary<int, int>> UpdateOrder(List<Order> orders, string deviceId);
        Task<List<Order>> GetOrdersByIds(List<int> ids);
        Task<List<Order>> GetAllOrders(string deviceId);
        Task<int> GetLastOrderServerId();
        Task<Dictionary<int, bool>> GetAllOrdersVisibility(string deviceId);
        Task<bool> EditOrder(Order order, string deviceId);
        Task<Order> PostOrder(Order order, string deviceId);
    }
}
