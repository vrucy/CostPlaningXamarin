using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using CostPlaningXamarin.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(SynchronizationService))]
namespace CostPlaningXamarin.Services
{
    public class SynchronizationService : ISynchronizationService
    {
        IOrderService orderService = DependencyService.Get<IOrderService>();
        ICategoryService categoryService = DependencyService.Get<ICategoryService>();
        IUserService userService = DependencyService.Get<IUserService>();
        ISQLiteService SQLiteService = DependencyService.Get<ISQLiteService>();
        //public async Task FirstSyncUserOwner(User appUser)
        //{
        //    var serverUser = await userService.PostAppUser();
        //    SQLiteService.DropTable<User>();
        //    SQLiteService.CreateTable<User>();
        //    await SyncNewUsers(serverUser.Id);
        //    var orders = await SQLiteService.GetAllOrdersForUserById(appUser.Id);
        //    if (orders.Count > 0)
        //    {
        //        //TODO: need some rollback if crash conn?
        //        await PostOrders(orders);
        //    }
        //}
        //private async Task SyncNewUsers(int userId)
        //{
        //    var allUsers = userService.GetAllUsers().Result;
        //    await SQLiteService.SaveItems(allUsers);
        //    SQLiteService.UpdateDeviceUser(userId);
        //}
        public async Task SyncUsers(int lastUserId)
        {
            var users = userService.GetUnsyncUsers(lastUserId);
            await SQLiteService.SaveItems(users);
        }
        private async Task PostOrders(List<Order> orders)
        {
            await orderService.PostOrdersSync(orders);
        }
        //TODO: refactor code repat
        public async Task SyncOrders(List<Order> orders)
        {
            if (orders.Count != 0)
            {
                var ids = orderService.UpdateOrder(orders);
                await SQLiteService.SyncOrders(ids);
            }
            var ordersSync = SQLiteService.GetAllSyncIds<Order>().ToList();

            var ordersFromServer = orderService.GetOrdersByIds(ordersSync);

            if (ordersFromServer != null )
            {
                await SQLiteService.SaveItems(ordersFromServer);
            }
        }
        public async Task SyncCategoies(List<Category> categories, int userId)
        {
            if (categories.Count != 0)
            {
                var ids = categoryService.PostCategories(categories, userId);
                await SQLiteService.SyncCategories(ids);
            }
            var categoriesSync = SQLiteService.GetAllSyncIds<Category>().ToList();
            var categoriesFromServer = categoryService.GetAllCategoriesByIds(categoriesSync);

            if (categoriesFromServer.Count != 0)
            {
                await SQLiteService.SaveItems(categoriesFromServer);
            }
        }
        public async Task SyncVisible<T>(int appUserId)
        {
            if (typeof(T) == typeof(Category))
            {
                var categoresForDisable = categoryService.GetAllCategoresVisibility(appUserId);
                if (categoresForDisable.Count > 0)
                {
                    await SyncVisiblityOnMobile<Category>(categoresForDisable);
                }
            }
            if (typeof(T) == typeof(Order))
            {
                var ordersForSyncVisibility = orderService.GetAllOrdersVisibility(appUserId);
                if (ordersForSyncVisibility.Count > 0)
                {
                    await SyncVisiblityOnMobile<Order>(ordersForSyncVisibility);
                }
            }
        }
        private async Task SyncVisiblityOnMobile<T>(Dictionary<int, bool> collection)
        {
            if (collection.Any())
            {
                if (typeof(T) == typeof(Category))
                {
                    await SQLiteService.SyncVisbility<Category>(collection, true);
                }
                else if (typeof(T) == typeof(Order))
                {
                    await SQLiteService.SyncVisbility<Order>(collection, true);
                }
            }
        }
    }
}
