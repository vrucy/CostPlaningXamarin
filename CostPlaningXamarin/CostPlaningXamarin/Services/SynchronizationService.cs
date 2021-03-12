﻿using CostPlaningXamarin.Interfaces;
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
        public async Task SyncUsers(int lastUserId)
        {
            var users = userService.GetUnsyncUsers(lastUserId);
            await SQLiteService.SaveItems(users);
        }
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

            var ordersFromServer = orderService.GetOrdersByIds(ordersSync);

            if (ordersFromServer != null )
            {
                await SQLiteService.SaveItems(ordersFromServer);
            }
        }
        public async Task SyncCategoies(List<Category> categories,string deviceId)
        {
            if (categories.Count != 0)
            {
                //TODO: isto uraditi i ovde kao sa roderom
                var ids = categoryService.PostCategories(categories, deviceId);
                await SQLiteService.SyncCategories(ids);
            }
            var categoriesSync = SQLiteService.GetAllSyncIds<Category>().ToList();
            var categoriesFromServer = categoryService.GetAllCategoriesByIds(categoriesSync);

            if (categoriesFromServer.Count != 0)
            {
                await SQLiteService.SaveItems(categoriesFromServer);
            }
        }
        public async Task SyncVisible<T>(string deviceId)
        {
            if (typeof(T) == typeof(Category))
            {
                var categoresForDisable = categoryService.GetAllCategoresVisibility(deviceId);
                if (categoresForDisable.Count > 0)
                {
                    await SyncVisiblityOnMobile<Category>(categoresForDisable);
                }
            }
            if (typeof(T) == typeof(Order))
            {
                var ordersForSyncVisibility = orderService.GetAllOrdersVisibility(SQLiteService.GetCurrentDeviceInfo().DeviceId);
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
