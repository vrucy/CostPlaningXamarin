using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using CostPlaningXamarin.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        //TODO: refactor code repat
        public async Task SyncOrders(List<Order> orders, string deviceId)
        {
            if (orders.Count != 0)
            {
                SemaphoreSlim ss = new SemaphoreSlim(1);
                await ss.WaitAsync();
                //TODO: proveriti ovde da ne da ids prazne se sync
                var ids = await orderService.UpdateOrder(orders, deviceId);
                ss.Release();
                await SQLiteService.SyncOrders(ids);
            }
            var ordersSync = SQLiteService.GetAllSyncIds<Order>().ToList();

            var ordersFromServer = await orderService.GetOrdersByIds(ordersSync);

            if (ordersFromServer.Count > 0)
            {
                await SQLiteService.SaveItems(ordersFromServer);
            }
        }
        public async Task SyncUsers(int lastUserId)
        {
            var users = await userService.GetUnsyncUsers(lastUserId);
            await SQLiteService.SaveItems(users);
        }
        public async Task SyncCategoies(int lastCategoryId)
        {
            var categories = await categoryService.GetUnsyncCategories(lastCategoryId);
            await SQLiteService.SaveItems(categories);
        }
        public async Task SyncVisible<T>(string deviceId)
        {
            if (typeof(T) == typeof(Category))
            {
                var categoresForSync = await categoryService.GetAllCategoresVisibility(deviceId);

                if (categoresForSync.Count > 0)
                {
                    await SyncVisiblityOnMobile<Category>(categoresForSync);
                }

            }
            if (typeof(T) == typeof(Order))
            {
                var ordersForSyncVisibility = await orderService.GetAllOrdersVisibility(SQLiteService.GetCurrentDeviceInfo().DeviceId);

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
                    await SQLiteService.SyncVisbility<Category>(collection);
                }
                else if (typeof(T) == typeof(Order))
                {
                    await SQLiteService.SyncVisbility<Order>(collection);
                }
            }
        }
    }
}
