using CostPlaningXamarin.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CostPlaningXamarin.Interfaces
{
    interface IOrderService
    {
        Task<HttpResponseMessage> PostOrders();
        Task<IList<Order>> GetAllOrders();
    }
}
