using CostPlaningXamarin.Helper;
using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using CostPlaningXamarin.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(OrderService))]
namespace CostPlaningXamarin.Services
{
    public class OrderService : IOrderService
    {
        //TODO: DI
        private static readonly HttpClient _httpClient = new HttpClient();

        private const string urlLocalHost = Constants.urlLocalHost;

        private HttpContent MediaTypeHeaderValue(object o)
        {
            var data = JsonConvert.SerializeObject(o);
            var content = new StringContent(data);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return content;
        }
        private async Task<string> ResponseResult(string route)
        {
            var res = await _httpClient.GetAsync(urlLocalHost + route);
            return await res.Content.ReadAsStringAsync();
        }

        public async Task<Dictionary<int, int>> UpdateOrder(List<Order> orders, string deviceId)
        {

            var res = await _httpClient.PutAsync(urlLocalHost + string.Format("Order/UpdateOrders/{0}",deviceId), MediaTypeHeaderValue(orders));

            var content = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Dictionary<int, int>>(content);

        }
        //TODO: need to be GetAsync!!
        public async Task<List<Order>> GetOrdersByIds(List<int> ids)
        {
            var res =await _httpClient.PostAsync(urlLocalHost + "Order/GetAllOrdersByIds", MediaTypeHeaderValue(ids));

            var content =await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Order>>(content);
        }
        public async Task<List<Order>> GetAllOrders(string deviceId)
        {
            return JsonConvert.DeserializeObject<List<Order>>(await ResponseResult(string.Format("Order/GetAllOrders/{0}", deviceId)));
        }
        public async Task<int> GetLastOrderServerId()
        {
            return JsonConvert.DeserializeObject<int>(await ResponseResult("Order/GetLastOrderServerId"));
        }
        public async Task<Dictionary<int, bool>> GetAllOrdersVisibility(string deviceId)
        {
            return JsonConvert.DeserializeObject<Dictionary<int, bool>>(await ResponseResult(string.Format("Order/SyncVisibility/{0}", deviceId)));
        }
        public async Task<bool> EditOrder(Order order, string deviceId)
        {
            //TODO: every id must encrypt
            var res = await _httpClient.PutAsync(urlLocalHost + string.Format("Order/EditOrder/{0}", deviceId), MediaTypeHeaderValue(order));
            
            if (res.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task PostOrder(Order order,string deviceId)
        {
            await _httpClient.PostAsync(urlLocalHost + string.Format("Order/PostOrder/{0}", deviceId), MediaTypeHeaderValue(order));
        }
    }
}
