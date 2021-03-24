using CostPlaningXamarin.Helper;
using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using CostPlaningXamarin.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(OrderService))]
namespace CostPlaningXamarin.Services
{
    public class OrderService : IOrderService
    {
        //TODO: DI
        private static readonly HttpClientHelper _httpClient = new HttpClientHelper();

        public async Task<Dictionary<int, int>> UpdateOrder(List<Order> orders, string deviceId)
        {
            return JsonConvert.DeserializeObject<Dictionary<int, int>>(await _httpClient.PutOrdersAsync(orders, string.Format("Order/UpdateOrders/{0}", deviceId)));
        }
        //TODO: need to be GetAsync!!
        public async Task<List<Order>> GetOrdersByIds(List<int> ids)
        {
            return JsonConvert.DeserializeObject<List<Order>>(await _httpClient.PostAsync(ids, "Order/GetAllOrdersByIds"));
        }
        public async Task<List<Order>> GetAllOrders(string deviceId)
        {
            return JsonConvert.DeserializeObject<List<Order>>(await _httpClient.ResponseResult(string.Format("Order/GetAllOrders/{0}", deviceId)));
        }
        public async Task<int> GetLastOrderServerId()
        {
            return JsonConvert.DeserializeObject<int>(await _httpClient.ResponseResult("Order/GetLastOrderServerId"));
        }
        public async Task<Dictionary<int, bool>> GetAllOrdersVisibility(string deviceId)
        {
            return JsonConvert.DeserializeObject<Dictionary<int, bool>>(await _httpClient.ResponseResult(string.Format("Order/SyncVisibility/{0}", deviceId)));
        }
        public async Task<bool> EditOrder(Order order, string deviceId)
        {
            //TODO: every id must encrypt
            var res = await _httpClient.PutAsync(order, string.Format("Order/EditOrder/{0}", deviceId));
            
            if (res.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<Order> PostOrder(Order order,string deviceId)
        {
            return JsonConvert.DeserializeObject<Order>(await _httpClient.PostAsync(order, string.Format("Order/PostOrder/{0}", deviceId)));
        }
    }
}
