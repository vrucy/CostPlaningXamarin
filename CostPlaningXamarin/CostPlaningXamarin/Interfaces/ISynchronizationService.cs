using CostPlaningXamarin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CostPlaningXamarin.Interfaces
{
    interface ISynchronizationService
    {
        //Task FirstSyncUserOwner(User appUser);
        Task SyncUsers(int Id);
        Task SyncOrders(List<Order> orders, string deviceId);
        Task SyncCategoies(int lastCategoryId);
        Task SyncVisible<T>(string deviceId);
    }
}
