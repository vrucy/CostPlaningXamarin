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
        public async Task SyncOrders(List<Order> ordersForSync,List<Order> newOrder,string deviceId)
        {
            if (ordersForSync.Count != 0)
            {
                SemaphoreSlim ss = new SemaphoreSlim(1);
                await ss.WaitAsync();
                var ids = await orderService.UpdateOrder(ordersForSync, deviceId);
                ss.Release();
                await SQLiteService.SyncOrders(ids);
            }
            if (newOrder.Any())
            {
                await SQLiteService.SyncVisbility(newOrder);
            }
        }        
    }
}
