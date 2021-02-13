using CostPlaningXamarin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CostPlaningXamarin.Interfaces
{
    interface ISynchronizationService
    {
        void FirstSyncUserOwner(User appUser);
        void SyncUsers(int Id);
        void SyncOrders(List<Order> orders);
        void SyncCategoies(List<Category> categories, int userId);
        void SyncVisible<T>(int appUserId);
    }
}
