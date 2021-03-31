using CostPlaningXamarin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CostPlaningXamarin.Interfaces
{
    interface IOrderService
    {
        Task<Dictionary<int, int>> UpdateOrder(List<Order> orders, string deviceId);
        Task<List<Order>> GetAllOrders(string deviceId);
        Task<bool> EditOrder(Order order, string deviceId);
        Task<Order> PostOrder(Order order, string deviceId);
        Task<List<Order>> GetUnsyncOrders(string deviceId);
    }
}
