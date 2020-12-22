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
            //db.DropTableAsync<Order>().GetAwaiter().GetResult();
            //db.DropTable<User>();
             //db.DropTableAsync<Category>().GetAwaiter().GetResult();
             //db.DeleteAllAsync<Order>();
             //db.DeleteAllAsync<Category>();
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
        //TODO: WHO IS USER!!!
        public void SaveOrderAsync(Order order)
        {
            try
            {
            db.InsertAsync(order);

            }
            catch (System.Exception e)
            {

                throw;
            }
        }
        public async void Seed(SQLiteAsyncConnection db)
        {
            //if (db.Table<User>().ToListAsync().Result.Count == 0)
            //{
            //    await db.InsertAsync(new User() { FirstName = "Vlada", LastName = "Vrucinic" });
            //    await db.InsertAsync(new User() { FirstName = "Jovana", LastName = "Vrucinic" });
            //}
            if (db.Table<Category>().ToListAsync().Result.Count == 0)
            {
                try
                {
               db.InsertAsync(new Category() { Name = "Hrana" });
               db.InsertAsync(new Category() { Name = "Razno" });
               db.InsertAsync(new Category() { Name = "Putovanja" });

                }
                catch (System.Exception r)
                {

                    throw;
                }
            }
        }
        public void SaveAllOrders(IList<Order> orders)
        {
            db.InsertAllAsync(orders);
        }
        //Delete
        public void DeleteItemAsync(Order order)
        {
            db.DeleteAsync(order);
        }
        public Task<List<Order>> GetAllOrdersForUserById(int id)
        {

            return db.GetAllWithChildrenAsync<Order>(x => x.UserId == id && x.IsWriteToDb == false);
        }
        //Read All Items
        public Task<List<Order>> GetOrdersAsync()
        {
            return db.GetAllWithChildrenAsync<Order>();
        }
        public Task<List<Order>> GetOrdersUnsyncAsync()
        {
            return db.GetAllWithChildrenAsync<Order>(x => x.IsWriteToDb == false);
        }

        //Read Item
        public void GetItemAsync(int orderId)
        {
            db.Table<Order>().Where(i => i.Id == orderId).FirstOrDefaultAsync();
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
            //db.DeleteAllAsync<User>();
            var x = db.Table<User>().CountAsync().Result;
            user.DeviceUser = true;
            db.InsertAsync(user);
        }
        public bool CheckIfExistUser()
        {
            //db.CreateTableAsync<User>().Wait();
            return db.Table<User>().FirstOrDefaultAsync(x => x.DeviceUser == true).GetAwaiter().GetResult() == null;
        }
        public Task<int> CountUserInMobile()
        {
            return db.Table<User>().CountAsync();
        }
        public User GetAppUser()
        {
            var y = db.Table<User>();
            return db.Table<User>().FirstOrDefaultAsync(x => x.DeviceUser == true).Result;
        }
        public void PostNewUsers(IList<User> users)
        {
            db.DropTableAsync<User>().Wait();
            db.CreateTableAsync<User>().Wait();
            foreach (var item in users)
            {
                item.WriteInDb = true;
            }
            db.InsertAllAsync(users).Wait();
        }
        public void UpdateDeviceUser(int newId)
        {
            try
            {
                var appUser = db.GetAllWithChildrenAsync<User>().Result.FirstOrDefault(x => x.Id == newId);

                appUser.DeviceUser = true;
                appUser.WriteInDb = true;
                db.UpdateWithChildrenAsync(appUser).Wait();
            }
            catch (System.Exception e)
            {

                throw;
            }
        }

        public void ChangeOrdersForUserDevace(List<Order> orders, int newId)
        {
            var d = orders.Except(db.Table<Order>().ToListAsync().Result);
            var res = db.Table<Order>().Where(x => orders.Any(o => o.UserId == x.UserId))/*.ToListAsync().Result*/;

            foreach (var item in d)
            {
                item.UserId = newId;
            }
            try
            {
                var t = db.UpdateAllAsync(d);

            }
            catch (System.Exception e)
            {

                throw;
            }
        }

        public Task<List<Order>> OrderForSync()
        {
            return db.GetAllWithChildrenAsync<Order>(x => x.IsWriteToDb == false);
        }

        public void SyncOrders(Dictionary<int, int> ids)
        {
            //List<Order> oldOrders = new List<Order>();
            foreach (var item in ids)
            {
                //oldOrders.AddRange(db.GetAllWithChildrenAsync<Order>(x=>x.Id == item).Result);
                try
                {
                    var currentItem = db.GetWithChildrenAsync<Order>(item.Key).Result;
                    currentItem.IdServer= item.Value;
                    currentItem.IsWriteToDb = true;
                    db.UpdateWithChildrenAsync(currentItem).GetAwaiter().GetResult();
                    //db.InsertWithChildrenAsync(currentItem).GetAwaiter();
                }
                catch (System.Exception e)
                {

                    throw;
                }
            }
        }
        public void SyncOrders(IList<Order> orders)
        {
            foreach (var item in orders)
            {
                try
                {
                    item.IdServer = item.Id;
                    item.IsWriteToDb = true;
                    db.InsertAsync(item).GetAwaiter().GetResult();
                    var x = db.GetAllWithChildrenAsync<Order>().Result;
                }
                catch (System.Exception e)
                {

                    throw;
                }
            }
        }
        public IList<int> GetAllSyncOrdersIds()
        {
            return db.GetAllWithChildrenAsync<Order>(x => x.IsWriteToDb == true).Result.Select(o=>o.IdServer).ToList();
        }

        public int GetLastOrderServerId()
        {
            try
            {
                var orders = db.GetAllWithChildrenAsync<Order>().Result;
                if (orders.Count == 0) 
                {
                    return 0;
                }
                    var t = orders.OrderByDescending(x => x.IdServer).FirstOrDefault().IdServer;
            }
            catch (System.Exception e)
            {

                throw;
            }
            return db.GetAllWithChildrenAsync<Order>().Result.OrderByDescending(x => x.IdServer).FirstOrDefault().IdServer;
        }

        public bool CheckIfHaveOrderForSync()
        {
            return db.GetAllWithChildrenAsync<Order>(x=>x.IsWriteToDb == false).Result.Any();
        }

        public int GetLastUserServerId()
        {
            var users = db.GetAllWithChildrenAsync<User>().Result;
            return users.OrderByDescending(x => x.Id).FirstOrDefault().Id;
        }
    }
}
