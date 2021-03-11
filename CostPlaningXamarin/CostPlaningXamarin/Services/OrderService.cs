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
        private string ResponseResult(string route)
        {
            var res = _httpClient.GetAsync(urlLocalHost + route).GetAwaiter().GetResult();
            return res.Content.ReadAsStringAsync().Result;
        }

        public Dictionary<int, int> UpdateOrder(List<Order> orders)
        {

            var res = _httpClient.PostAsync(urlLocalHost + "Order/UpdateOrders", MediaTypeHeaderValue(orders)).GetAwaiter().GetResult();

            var content = res.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<Dictionary<int, int>>(content);

        }
        //TODO: need to be GetAsync!!
        public List<Order> GetOrdersByIds(List<int> ids)
        {

            var res = _httpClient.PostAsync(urlLocalHost + "Order/GetAllOrdersByIds", MediaTypeHeaderValue(ids)).GetAwaiter().GetResult();

            var content = res.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<List<Order>>(content);
        }
        public List<Order> GetAllOrders()
        {
            return JsonConvert.DeserializeObject<List<Order>>(ResponseResult("Order/GetAllOrders"));
        }
        public int GetLastOrderServerId()
        {
            return JsonConvert.DeserializeObject<int>(ResponseResult("Order/GetLastOrderServerId"));
        }
        public Dictionary<int, bool> GetAllOrdersVisibility(int appUserId)
        {
            return JsonConvert.DeserializeObject<Dictionary<int, bool>>(ResponseResult(string.Format("Order/SyncVisibility/{0}", appUserId)));
        }
        public bool EditOrder(Order order, int userId)
        { 
            var res = _httpClient.PutAsync(urlLocalHost + string.Format("Order/EditOrder/{0}", userId), MediaTypeHeaderValue(order)).GetAwaiter().GetResult();
            
            if (res.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task PostOrder(Order order)
        {
            await _httpClient.PostAsync(urlLocalHost + "Order/PostOrder", MediaTypeHeaderValue(order));
        }
    }
}
