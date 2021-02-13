using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using CostPlaningXamarin.Services;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(SynchronizationService))]
namespace CostPlaningXamarin.Services
{
    public class SynchronizationService: ISynchronizationService
    {
        IOrderService orderService = DependencyService.Get<IOrderService>();
        ICategoryService categoryService = DependencyService.Get<ICategoryService>();
        IUserService userService = DependencyService.Get<IUserService>();
        ISQLiteService SQLiteService = DependencyService.Get<ISQLiteService>();
        public async void FirstSyncUserOwner(User appUser)
        {
            var serverUser = userService.PostAppUser().GetAwaiter().GetResult();
            SQLiteService.DropTable<User>();
            SQLiteService.CreateTable<User>();
            SyncNewUsers(serverUser.Id);
            var orders = await SQLiteService.GetAllOrdersForUserById(appUser.Id);
            if (orders.Count > 0)
            {
                //TODO: need some rollback if crash conn?
                PostOrders(orders);
            }
        }
        private void SyncNewUsers(int userId)
        {
            var allUsers = userService.GetAllUsers().Result;
            SQLiteService.PostNewUsers(allUsers);
            SQLiteService.UpdateDeviceUser(userId);
        }
        public void SyncUsers(int lastUserId)
        {
            var users = userService.GetUnsyncUsers(lastUserId);
            SQLiteService.PostNewUsers(users.Result);
        }
        private void PostOrders(List<Order> orders)
        {
            orderService.PostOrdersSync(orders);
        }
        //TODO: refactor
        public void SyncOrders(List<Order> orders)
        {
            if (orders.Count != 0)
            {
                var ids = orderService.UpdateOrder(orders);
                SQLiteService.SyncOrders(ids);
            }
            var ordersSync = SQLiteService.GetAllSyncIds<Order>().ToList();

            var ordersFromServer = orderService.GetOrdersByIds(ordersSync);

            if (ordersFromServer.Count != 0)
            {
                SQLiteService.SaveItems(ordersFromServer);
            }
        }
        public void SyncCategoies(List<Category> categories,int userId)
        {
            if (categories.Count != 0)
            {
                var ids = categoryService.PostCategories(categories, userId);
                SQLiteService.SyncCategories(ids);
            }
            var categoriesSync = SQLiteService.GetAllSyncIds<Category>().ToList();
            var categoriesFromServer = categoryService.GetAllCategoriesByIds(categoriesSync);

            if (categoriesFromServer.Count != 0)
            {
                SQLiteService.SaveItems(categoriesFromServer);
            }
        }
        public void SyncVisible<T>(int appUserId)
        {
            if (typeof(T) == typeof(Category))
            {
                var categoresForDisable = categoryService.GetAllCategoresVisibility(appUserId);
                if (categoresForDisable.Count > 0)
                {
                    SyncVisiblityOnMobile(categoresForDisable);
                }
            }
            if (typeof(T) == typeof(Order))
            {

                
            }
        }
        private void SyncVisiblityOnMobile(Dictionary<int, bool> collection)
        {
            if (collection.Any())
            {
                SQLiteService.SyncVisbility<Category>(collection, true);
            }
        }
    }
}
