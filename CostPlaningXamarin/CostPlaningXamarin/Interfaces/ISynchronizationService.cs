using CostPlaningXamarin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CostPlaningXamarin.Interfaces
{
    interface ISynchronizationService
    {
        Task FirstSyncUserOwner(User appUser);
        Task SyncUsers(int Id);
        Task SyncOrders(List<Order> orders);
        Task SyncCategoies(List<Category> categories, int userId);
        Task SyncVisible<T>(int appUserId);
    }
}
