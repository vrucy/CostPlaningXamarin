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

            db.CreateTableAsync<User>().Wait();
            db.CreateTableAsync<Order>().Wait();
            db.CreateTableAsync<Category>().Wait();
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
        public Task<List<Order>> GetOrdersAsync()
        {
            return db.GetAllWithChildrenAsync<Order>();
        }
        //TODO: Move to order service this need to be form sql
        public Task<List<User>> GetUsers()
        {
            return db.Table<User>().ToListAsync();
        }
        public bool IsFirstSyncNeed()
        {
            return !db.GetAllWithChildrenAsync<User>().Result.Any() && !db.GetAllWithChildrenAsync<Category>().Result.Any();
        }
        //TODO: Here filter categores who not visible
        public Task<List<Category>> GetAllCategories()
        {
            var allCategores = db.Table<Category>();

            return allCategores.ToListAsync();
        }
        public void CreateAppUser(User user)
        {
            user.DeviceUser = true;

            //user.Id = 1;
            db.InsertOrReplaceAsync(user);
        }
        public bool CheckIfExistUser()
        {
            return db.Table<User>().FirstOrDefaultAsync(x => x.DeviceUser == true).GetAwaiter().GetResult() == null;
        }
        public User GetAppUser()
        {
            return db.Table<User>().FirstOrDefaultAsync(x => x.DeviceUser == true).Result;
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
        public async Task SyncOrders(Dictionary<int, int> ids)
        {
            foreach (var item in ids)
            {
                var currentItem = db.GetWithChildrenAsync<Order>(item.Key).Result;
                currentItem.ServerId = item.Value;

                await db.InsertOrReplaceAsync(currentItem);
            }
        }
        public async Task SyncCategories(Dictionary<int, int> ids)
        {
            foreach (var item in ids)
            {
                var currentItem = db.GetWithChildrenAsync<Category>(item.Key).Result;
                currentItem.ServerId = item.Value;

                await db.InsertOrReplaceAsync(currentItem);
            }
        }
        public async Task SaveItems<T>(IList<T> collection)
        {
            if (typeof(T) == typeof(Order))
            {
                var x = collection as List<Order>;
                foreach (var item in x)
                {
                    item.ServerId = item.Id;

                   await db.InsertAsync(item);
                }
            }
            else if (typeof(T) == typeof(Category))
            {
                var x = collection as List<Category>;
                foreach (var item in x)
                {
                    item.ServerId = item.Id;
                    await db.InsertAsync(item);
                }
            }
            else if (typeof(T) == typeof(User))
            {
                var x = collection as List<User>;
                await db.InsertAllAsync(x);

            }
        }
        public async Task SaveAsync<T>(T item)
        {
            await db.InsertAsync(item);
        }
        //public IList<int> GetAllSyncOrdersIds()
        //{
        //    return db.GetAllWithChildrenAsync<Order>(x => x.ServerId != 0).Result.Select(o => o.ServerId).ToList();
        //}
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
        //public int GetLastOrderServerId()
        //{
        //    var orders = db.GetAllWithChildrenAsync<Order>().Result;

        //    if (orders.Count == 0)
        //    {
        //        return 0;
        //    }
        //    return orders.OrderByDescending(x => x.ServerId).FirstOrDefault().ServerId;
        //}

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
        //TODO: Check if need writeToDb && code repite
        public async Task SyncVisbility<T>(Dictionary<int, bool> collection, bool isWriteToDb)
        {
            if (typeof(T) == typeof(Category))
            {
                foreach (var item in collection)
                {
                    var category = db.FindAsync<Category>(x => x.ServerId == item.Key).Result;
                    category.IsVisible = item.Value;

                    await db.InsertOrReplaceAsync(category);
                }
            }
            if (typeof(T) == typeof(Order))
            {
                foreach (var item in collection)
                {
                    var order = await db.Table<Order>().Where(x=>x.ServerId == item.Key).FirstOrDefaultAsync();
                    order.IsVisible = item.Value;

                    await db.InsertOrReplaceAsync(order);
                }
            }
        }

        public async Task Visibility<T>(T item,bool visibility)
        {
            if (typeof(T) == typeof(Category))
            {
                var category = item as Category;
                //promena visiblity
                category.IsVisible = visibility;
                await db.UpdateAsync(category);
            }
            if (typeof(T) == typeof(Order))
            {
                var order = item as Order;
                order.IsVisible = visibility;
                await db.UpdateAsync(order);
            }
        }
    }
}
