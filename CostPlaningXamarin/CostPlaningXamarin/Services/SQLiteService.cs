using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using CostPlaningXamarin.Services;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System.Collections.Generic;
using SQLiteNetExtensionsAsync.Extensions;
using System.Threading.Tasks;
using System.Linq;

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
            //db.DropTableAsync<Category>().GetAwaiter().GetResult();
            //db.DropTableAsync<Order>().Wait();

            db.CreateTableAsync<Order>().Wait();
            db.CreateTableAsync<Category>().Wait();
            db.CreateTableAsync<User>().Wait();
            Seed(db);
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
        public async void Seed(SQLiteAsyncConnection db)
        {
            if (db.Table<Category>().ToListAsync().Result.Count == 0)
            {

                db.InsertAsync(new Category() { Name = "Hrana" });
                db.InsertAsync(new Category() { Name = "Razno" });
                db.InsertAsync(new Category() { Name = "Putovanja" });
            }
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
        //Read All Items
        public Task<List<Order>> GetOrdersAsync()
        {
            return db.GetAllWithChildrenAsync<Order>();
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
            return db.Table<Category>().ToListAsync();
        }
        public void CreateAppUser(User user)
        {
            var x = db.Table<User>().CountAsync().Result;
            user.DeviceUser = true;
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
                item.ServerId = item.Id;
                item.Id = 0;
            }

            db.InsertAllAsync(users).Wait();

        }
        public void UpdateDeviceUser(int newId)
        {
            var appUser = db.GetAllWithChildrenAsync<User>().Result.FirstOrDefault(x => x.ServerId == newId);

            appUser.DeviceUser = true;
            db.UpdateWithChildrenAsync(appUser).Wait();
        }
        public Task<List<Order>> OrderForSync()
        {
            return db.GetAllWithChildrenAsync<Order>(x => x.ServerId == 0);
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
        public void SaveOrders(IList<Order> orders)
        {
            foreach (var item in orders)
            {
                item.ServerId = item.Id;
                db.InsertAsync(item).GetAwaiter().GetResult();
            }
        }
        public IList<int> GetAllSyncOrdersIds()
        {
            return db.GetAllWithChildrenAsync<Order>(x => x.ServerId != 0).Result.Select(o => o.ServerId).ToList();
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

        public bool CheckIfHaveOrderForSync()
        {
            return db.GetAllWithChildrenAsync<Order>(x => x.ServerId == 0).Result.Any();
        }

        public int GetLastUserServerId()
        {
            return db.GetAllWithChildrenAsync<User>().Result.OrderByDescending(x => x.ServerId).FirstOrDefault().ServerId;
        }


    }
}
