using CostPlaningXamarin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CostPlaningXamarin.Interfaces
{
    public interface ISQLiteService
    {
        Task CreateDBAsync(string path);
        void CreateAppUser(User user);
        void SaveOrderAsync(Order order);
        void SaveAllOrders(IList<Order> orders);
        Task<List<Order>> GetOrdersAsync();
        Task<List<Order>> GetOrdersUnsyncAsync();
        bool CheckIfHaveOrderForSync();
        Task<List<Order>> GetAllOrdersForUserById(int userId);
        void ChangeOrdersForUserDevace(List<Order> orders, int newId);
        Task<List<User>> GetUsers();
        //Task<User> GetUser();
        bool CheckIfExistUser();
        Task<List<Category>> GetAllCategories();
        Task<int> CountUserInMobile();
        void PostNewUsers(IList<User> newUsers);
        void UpdateDeviceUser(int newId);
        User GetAppUser();
        Task<List<Order>> OrderForSync();
        void SyncOrders(Dictionary<int, int> ids);
        void SyncOrders(IList<Order> orders);
        IList<int> GetAllSyncOrdersIds();
        int GetLastOrderServerId();
        int GetLastUserServerId();
        
    }
}
