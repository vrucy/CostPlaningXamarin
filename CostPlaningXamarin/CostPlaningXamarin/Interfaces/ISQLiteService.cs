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
        void SaveAsync<T>(T item);
        void DeleteAll<T>() where T : class;
        void CreateTable<T>() where T : new();
        void DropTable<T>() where T : new();
        Task<List<Order>> GetOrdersAsync();
        bool IsSyncData<T>();
        Task<List<Order>> GetAllOrdersForUserById(int userId);
        Task<List<User>> GetUsers();
        bool CheckIfExistUser();
        Task<List<Category>> GetAllCategories();
        void PostNewUsers(IList<User> newUsers);
        void UpdateDeviceUser(int newId);
        User GetAppUser();
        Task<List<Order>> OrderForSync();
        Task<List<Category>> CategoriesForSync();
        void SyncOrders(Dictionary<int, int> ids);
        void SyncCategories(Dictionary<int, int> ids);
        void SaveItems<T>(IList<T> collection);
        IList<int> GetAllSyncIds<T>();
        int GetLastServerId<T>() ;
        
    }
}
