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
        void DeleteAll<T>() where T : class;
        void CreateTable<T>() where T : new();
        void DropTable<T>() where T : new();
        Task<List<Order>> GetOrdersAsync();
        bool CheckIfHaveOrderForSync();
        Task<List<Order>> GetAllOrdersForUserById(int userId);
        Task<List<User>> GetUsers();
        //Task<User> GetUser();
        bool CheckIfExistUser();
        Task<List<Category>> GetAllCategories();
        void PostNewUsers(IList<User> newUsers);
        void UpdateDeviceUser(int newId);
        User GetAppUser();
        Task<List<Order>> OrderForSync();
        void SyncOrders(Dictionary<int, int> ids);
        void SaveOrders(IList<Order> orders);
        IList<int> GetAllSyncOrdersIds();
        int GetLastOrderServerId();
        int GetLastUserServerId();
        
    }
}
