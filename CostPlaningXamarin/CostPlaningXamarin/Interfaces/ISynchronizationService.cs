using CostPlaningXamarin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CostPlaningXamarin.Interfaces
{
    interface ISynchronizationService
    {
        Task SyncOrders(List<Order> ordersForSync,List<Order> newOrder,string deviceId);
    }
}
