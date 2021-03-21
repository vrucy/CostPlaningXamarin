using CostPlaningXamarin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CostPlaningXamarin.Interfaces
{
    public interface ISQLiteService
    {
        Task CreateDBAsync(string path);
        Task CreateAppUser(User user);
        void DeleteAll<T>() where T : class;
        void DropTable<T>() where T : new();
        Task<List<Order>> GetOrdersAsync();
        bool IsSyncData<T>();
        Task<List<User>> GetUsers();
        bool CheckIfExistUser();
        Task<List<Category>> GetAllCategories();
        User GetAppUser();
        Task<List<Order>> OrderForSync();
        Task SyncOrders(Dictionary<int, int> ids);
        Task SaveAsync<T>(T item);
        Task SaveItems<T>(IList<T> collection);
        IList<int> GetAllSyncIds<T>();
        int GetLastServerId<T>() ;
        bool IsFirstSyncNeed();
        Task Visibility<T>(T item,bool visibliity);
        Task SyncVisbility<T>(Dictionary<int, bool> collection);
        Models.Device GetCurrentDeviceInfo();
    }
}
