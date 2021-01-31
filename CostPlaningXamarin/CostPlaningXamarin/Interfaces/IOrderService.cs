using CostPlaningXamarin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CostPlaningXamarin.Interfaces
{
    interface IOrderService
    {
        //Task<HttpResponseMessage> PostOrders();
        //Task<IList<Order>> GetAllOrders();
        //Task<bool> CheckCategories();
        Task PostOrdersSync(List<Order> orders);
        /// <summary>
        /// Orders post on server
        /// </summary>
        /// <param name="orders"></param>
        /// <returns>Returns Dictionary(int oldKey, int newKey)</returns>
        Dictionary<int, int> UpdateOrder(List<Order> orders);
        List<Order> GetOrdersByIds(List<int> ids);
        int GetLastOrderServerId();
        int GetOrdersCountFromServer();
        bool IsServerAvailable();
    }
}
