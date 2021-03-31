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
        private readonly NLog.ILogger _logger = NLog.LogManager.GetCurrentClassLogger();

        public SQLiteService()
        {
        }
        //TODO: await and test
        public async Task CreateDBAsync(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            //db.DropTableAsync<User>().Wait();
            // db.DropTableAsync<Category>().GetAwaiter().GetResult();
            //db.DropTableAsync<Order>().Wait();
            db.CreateTableAsync<User>().Wait();
            db.CreateTableAsync<Order>().Wait();
            db.CreateTableAsync<Category>().Wait();
            db.CreateTableAsync<Device>().Wait();
        }
        public void DeleteAll<T>() where T : class
        {
            db.DeleteAllAsync<T>();
        }
        public void DropTable<T>() where T : new()
        {
            db.DropTableAsync<T>().Wait();
        }
        public Task<List<Order>> GetOrdersAsync()
        {
            return db.GetAllWithChildrenAsync<Order>();
        }
        public Task<List<User>> GetUsers()
        {
            return db.Table<User>().ToListAsync();
        }
        public bool IsFirstSyncNeed()
        {
            return !db.GetAllWithChildrenAsync<User>().Result.Any() && !db.GetAllWithChildrenAsync<Category>().Result.Any();
        }
        public Task<List<Category>> GetAllCategories()
        {
            var allCategores = db.Table<Category>();

            return allCategores.ToListAsync();
        }
        public async Task CreateAppUser(User user)
        {
            user.DeviceUser = true;

            await db.InsertOrReplaceAsync(user);
        }
        public bool CheckIfExistUser()
        {
            return db.Table<User>().FirstOrDefaultAsync(x => x.DeviceUser == true).GetAwaiter().GetResult() == null;
        }
        public User GetAppUser()
        {
            return db.Table<User>().FirstOrDefaultAsync(x => x.DeviceUser == true).Result;
        }
        public Task<List<Order>> OrderForSync()
        {
            return db.GetAllWithChildrenAsync<Order>(x => x.ServerId == 0);
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
        public async Task SaveItems<T>(IList<T> collection)
        {
            try
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
                    await db.InsertAllAsync(collection);
                }
                else if (typeof(T) == typeof(User))
                {
                    await db.InsertAllAsync(collection);
                }
            }
            catch (Exception e)
            {
                _logger.Warn("Exeption while save into sqlite: " + e.Message + "Inner ex: " + e.InnerException);
                throw;
            }
        }
        public async Task SaveAsync<T>(T item)
        {
            await db.InsertAsync(item);
        }
        public bool IsSyncData<T>()
        {
            if (typeof(T) == typeof(Order))
            {
                return db.GetAllWithChildrenAsync<Order>(x => x.ServerId == 0).Result.Any();
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
                return db.GetAllWithChildrenAsync<Category>().Result.OrderByDescending(x => x.Id).FirstOrDefault().Id;
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
        //TODO:code repite
        public async Task SyncVisbility<T>(List<T> collection)
        {
            if (typeof(T) == typeof(Category))
            {

                foreach (var item in collection as List<Category>)
                {
                    //var category = await db.Table<Category>().Where(x => x.Id == item.Id).FirstOrDefaultAsync();
                    //category.IsVisible = item.Value;
                    await db.InsertOrReplaceAsync(item);
                }
            }
            if (typeof(T) == typeof(Order))
            {
                foreach (var item in collection as List<Order>)
                {
                    var order = await db.Table<Order>().Where(x => x.ServerId == item.Id).FirstOrDefaultAsync();

                    if (order == null)
                    {
                        item.ServerId = item.Id;

                        await db.InsertAsync(item);
                    }
                    else
                    {

                        order.IsVisible = item.IsVisible;
                        order.Cost = item.Cost;
                        order.Date = item.Date;

                        await db.InsertOrReplaceAsync(order);
                    }
                }
            }
        }

        public async Task Visibility<T>(T item, bool visibility)
        {
            if (typeof(T) == typeof(Category))
            {
                var category = item as Category;
                category.IsVisible = visibility;
                await db.InsertOrReplaceAsync(category);
            }
            if (typeof(T) == typeof(Order))
            {
                var order = item as Order;
                order.IsVisible = visibility;
                try
                {

                    await db.InsertOrReplaceAsync(order);
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }

        public Models.Device GetCurrentDeviceInfo()
        {
            return db.Table<Device>().FirstOrDefaultAsync().Result;
        }
    }
}
