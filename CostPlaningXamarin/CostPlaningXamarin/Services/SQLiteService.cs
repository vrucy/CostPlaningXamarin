using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using CostPlaningXamarin.Services;
using SQLite;
using System.Collections.Generic;
using SQLiteNetExtensionsAsync.Extensions;
using System.Threading.Tasks;
using System.Linq;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(SQLiteService))]
namespace CostPlaningXamarin.Services
{
    public class SQLiteService : ISQLiteService
    {
        SQLiteAsyncConnection db;
        public SQLiteService()
        {
        }
        public async Task CreateDBAsync(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            //db.DropTableAsync<User>().Wait();
           // db.DropTableAsync<Category>().GetAwaiter().GetResult();
            //db.DropTableAsync<Order>().Wait();

            db.CreateTableAsync<Order>().Wait();
            db.CreateTableAsync<Category>().Wait();
            db.CreateTableAsync<User>().Wait();
        }
        private User AppUser()
        {
            return db.Table<User>().Where(x => x.DeviceUser == true).FirstAsync().Result;
        }
        //Insert and Update new record
        public void SaveOrderAsync(Order order)
        {
            db.InsertAsync(order);
        }

        public void DeleteAll<T>() where T : class
        {
            db.DeleteAllAsync<T>();
        }
        public void DropTable<T>() where T : new()
        {
            db.DropTableAsync<T>().Wait();
        }
        public void CreateTable<T>() where T : new()
        {
            db.CreateTableAsync<T>().Wait();
        }
        public Task<List<Order>> GetAllOrdersForUserById(int id)
        {
            return db.GetAllWithChildrenAsync<Order>(x => x.UserId == id && x.ServerId == 0);
        }
        public async Task<List<Order>> GetOrdersAsync()
        {
            var x= await db.GetAllWithChildrenAsync<Order>();
            return await db.GetAllWithChildrenAsync<Order>();
        }
        public Task<List<Order>> GetOrdersUnsyncAsync()
        {
            return db.GetAllWithChildrenAsync<Order>(x => x.ServerId == 0);
        }
        //TODO: Move to order service this need to be form sql
        public Task<List<User>> GetUsers()
        {
            return db.Table<User>().ToListAsync();
        }
        public Task<List<Category>> GetAllCategories()
        {
            var x = db.GetAllWithChildrenAsync<Category>();
            return x;
            //return db.Table<Category>().Where(x => x.IsDisable == false).ToListAsync().;
        }
        public void CreateAppUser(User user)
        {
            user.DeviceUser = true;

            user.Id = 1;
            db.InsertAsync(user);
        }
        public bool CheckIfExistUser()
        {
            return db.Table<User>().FirstOrDefaultAsync(x => x.DeviceUser == true).GetAwaiter().GetResult() == null;
        }
        public Task<int> CountUserInMobile()
        {
            return db.Table<User>().CountAsync();
        }
        public User GetAppUser()
        {
            return db.Table<User>().FirstOrDefaultAsync(x => x.DeviceUser == true).Result;
        }
        public void PostNewUsers(IList<User> users)
        {
            foreach (var item in users)
            {
                try
                {
                    db.InsertAsync(item).Wait();
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }
        public void UpdateDeviceUser(int newId)
        {
            var appUser = db.GetAllWithChildrenAsync<User>().Result.FirstOrDefault(x => x.Id == newId);

            appUser.DeviceUser = true;
            db.UpdateWithChildrenAsync(appUser).Wait();
        }
        public Task<List<Order>> OrderForSync()
        {
            return db.GetAllWithChildrenAsync<Order>(x => x.ServerId == 0);
        }
        public Task<List<Category>> CategoriesForSync()
        {
            return db.GetAllWithChildrenAsync<Category>(x => x.ServerId == 0);
        }
        public void SyncOrders(Dictionary<int, int> ids)
        {
            foreach (var item in ids)
            {
                var currentItem = db.GetWithChildrenAsync<Order>(item.Key).Result;
                currentItem.ServerId = item.Value;
                db.UpdateWithChildrenAsync(currentItem).GetAwaiter().GetResult();

            }
        }
        public void SyncCategories(Dictionary<int, int> ids)
        {
            foreach (var item in ids)
            {
                var currentItem = db.GetWithChildrenAsync<Category>(item.Key).Result;
                currentItem.ServerId = item.Value;
                db.UpdateWithChildrenAsync(currentItem).GetAwaiter().GetResult();

            }
        }
        public void SaveItems<T>(IList<T> collection)
        {
            if (typeof(T) == typeof(Order))
            {
                var x = collection as List<Order>;
                foreach (var item in x)
                {
                    item.ServerId = item.Id; 
                    db.InsertAsync(item).Wait();
                    var f = db.GetAllWithChildrenAsync<Order>().Result;
                }
                var df = db.GetAllWithChildrenAsync<Order>().Result;
            }
            else if (typeof(T) == typeof(Category))
            {
                var x = collection as List<Category>;
                foreach (var item in x)
                {
                    item.ServerId = item.Id;
                    db.InsertAsync(item).GetAwaiter().GetResult();
                }
            }
            else if (typeof(T) == typeof(User))
            {
                var x = collection as List<Category>;
                db.InsertAllAsync(x);
            }
        }
        public void SaveAsync<T>(T item)
        {
            db.InsertAsync(item);
        }
        public IList<int> GetAllSyncOrdersIds()
        {
            return db.GetAllWithChildrenAsync<Order>(x => x.ServerId != 0).Result.Select(o => o.ServerId).ToList();
        }
        public IList<int> GetAllSyncIds<T>()
        {
            if (typeof(T) == typeof(Order))
            {
                return db.GetAllWithChildrenAsync<Order>(x => x.ServerId != 0).Result.Select(o => o.ServerId).ToList();
            }
            else if (typeof(T) == typeof(Category))
            {
                return db.GetAllWithChildrenAsync<Category>(x => x.ServerId != 0).Result.Select(o => o.ServerId).ToList();
            }
            else if (typeof(T) == typeof(User))
            {
                return db.GetAllWithChildrenAsync<User>(x => x.Id != 0).Result.Select(o => o.Id).ToList();
            }

            return null;
        }
        public int GetLastOrderServerId()
        {
            var orders = db.GetAllWithChildrenAsync<Order>().Result;

            if (orders.Count == 0)
            {
                return 0;
            }
            return orders.OrderByDescending(x => x.ServerId).FirstOrDefault().ServerId;
        }

        public bool IsSyncData<T>()
        {
            if (typeof(T) == typeof(Order))
            {
                return db.GetAllWithChildrenAsync<Order>(x => x.ServerId == 0).Result.Any();
            }
            else if (typeof(T) == typeof(Category))
            {
                return db.GetAllWithChildrenAsync<Category>(x => x.ServerId == 0).Result.Any();
            }
            return false;
        }

        //TODO: getting model through reflection its be only 2 line of code if posible
        public int GetLastServerId<T>()
        {
            if (typeof(T) == typeof(User))
            {
                return db.GetAllWithChildrenAsync<User>().Result.OrderByDescending(x => x.Id).FirstOrDefault().Id;
            }
            else if (typeof(T) == typeof(Order))
            {
                if (IsOrderEmpty())
                {
                    return -1;
                }
                return db.GetAllWithChildrenAsync<Order>().Result.OrderByDescending(x => x.ServerId).FirstOrDefault().ServerId;
            }
            else if (typeof(T) == typeof(Category) && !IsCategoryEmpty())
            {
                if (IsCategoryEmpty())
                {
                    return 0;
                }
                return db.GetAllWithChildrenAsync<Category>().Result.OrderByDescending(x => x.ServerId).FirstOrDefault().ServerId;
            }
            return -1;
        }
        private bool IsOrderEmpty()
        {
            return !db.GetAllWithChildrenAsync<Order>().Result.Any();
        }
        private bool IsCategoryEmpty()
        {
            return !db.GetAllWithChildrenAsync<Category>().Result.Any();
        }

        public List<int> AllDisable<T>()
        {
            if (typeof(T) == typeof(Category))
            {
                return db.GetAllWithChildrenAsync<Category>(c => c.IsDisable == true).Result.Select(x => x.ServerId).ToList();
            }
            return null;
        }
        public List<int> AllEnable<T>()
        {
            if (typeof(T) == typeof(Category))
            {
                return db.GetAllWithChildrenAsync<Category>(c => c.IsDisable == false).Result.Select(x => x.ServerId).ToList();
            }
            return null;
        }
        public void SyncVisbility<T>(Dictionary<int, bool> collection, bool isWriteToDb)
        {
            if (typeof(T) == typeof(Category))
            {
                foreach (var item in collection)
                {
                    var category = db.FindAsync<Category>(x => x.ServerId == item.Key).Result;
                    category.IsDisable = item.Value;
                    category.IsWriteToDB = isWriteToDb;

                    db.UpdateWithChildrenAsync(category).Wait();
                }
            }
        }
    }
}
