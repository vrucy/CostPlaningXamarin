using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using CostPlaningXamarin.Services;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System.Collections.Generic;
using SQLiteNetExtensionsAsync.Extensions;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(SQLiteService))]
namespace CostPlaningXamarin.Services
{
    public class SQLiteService : ISQLiteService
    {
        SQLiteAsyncConnection db;
        public SQLiteService()
        {
        }
        public async void CreateDBAsync(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            //db.DropTable<Order>();
            //db.DropTable<User>();
            //db.DropTable<Category>();
           // db.DeleteAll<Order>();
            await db.CreateTableAsync<Order>();
            await db.CreateTableAsync<Category>();
            await db.CreateTableAsync<User>();
            Seed(db);
        }
        //Insert and Update new record
        //TODO: WHO IS USER!!!
        public void SaveOrderAsync(Order order)
        {
             order.UserId = 2;
             db.InsertAsync(order);
        }
        public async void Seed(SQLiteAsyncConnection db)
        {
            if (db.Table<User>().ToListAsync().Result.Count == 0)
            {
                await db.InsertAsync(new User() { FirstName = "Vlada", LastName = "Vrucinic" });
                await db.InsertAsync(new User() { FirstName = "Jovana", LastName = "Vrucinic" });
            }
            if (db.Table<Category>().ToListAsync().Result.Count == 0)
            {
                await db.InsertAsync(new Category() { Name = "Hrana" });
                await db.InsertAsync(new Category() { Name = "Razno" });
                await db.InsertAsync(new Category() { Name = "Putovanja" });
            }

        }
        public void SaveAllOrders(IList<Order> orders)
        {
            db.InsertAllAsync(orders);
        }
        public void DeleteAllOrders()
        {
            db.DeleteAllAsync<Order>();
        }
        //Delete
        public void DeleteItemAsync(Order order)
        {
            db.DeleteAsync(order);
        }

        //Read All Items
        public Task<List<Order>> GetOrdersAsync()
        {
            return db.GetAllWithChildrenAsync<Order>();
        }
        public Task<List<Order>> GetOrdersUnsyncAsync()
        {
            return db.GetAllWithChildrenAsync<Order>(x=>x.IsWriteToDb==false);
        }

        //Read Item
        public void GetItemAsync(int orderId)
        {
             db.Table<Order>().Where(i => i.Id == orderId).FirstOrDefaultAsync();
        }
        public async Task<User> GetUserAsync(int userId)
        {
            return await db.Table<User>().Where(i => i.Id == userId).FirstOrDefaultAsync();
        }


        public async Task<List<User>> GetUsers()
        {
            return await db.Table<User>().ToListAsync();
        }


        public Task<List<Category>> GetAllCategories()
        {
            return db.Table<Category>().ToListAsync();
        }
    }
}
