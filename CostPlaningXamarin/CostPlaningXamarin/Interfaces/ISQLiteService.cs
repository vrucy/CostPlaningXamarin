using CostPlaningXamarin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CostPlaningXamarin.Interfaces
{
    public interface ISQLiteService
    {
        Task CreateDBAsync(string path);
        void CreateAppUser(User user);
        void DeleteAll<T>() where T : class;
        void CreateTable<T>() where T : new();
        void DropTable<T>() where T : new();
        Task<List<Order>> GetOrdersAsync();
        bool IsSyncData<T>();
        Task<List<Order>> GetAllOrdersForUserById(int userId);
        Task<List<User>> GetUsers();
        bool CheckIfExistUser();
        Task<List<Category>> GetAllCategories();
        Task PostNewUsers(IList<User> newUsers);
        void UpdateDeviceUser(int newId);
        User GetAppUser();
        Task<List<Order>> OrderForSync();
        Task<List<Category>> CategoriesForSync();
        Task SyncOrders(Dictionary<int, int> ids);
        Task SyncCategories(Dictionary<int, int> ids);
        Task SaveAsync<T>(T item);
        Task SaveItems<T>(IList<T> collection);
        IList<int> GetAllSyncIds<T>();
        int GetLastServerId<T>() ;
        //List<int> AllDisable<T>();
        //List<int> AllEnable<T>();
        Task Disable<T>(T item);
        Task SyncVisbility<T>(Dictionary<int, bool> collection, bool isWriteToDb);
    }
}
