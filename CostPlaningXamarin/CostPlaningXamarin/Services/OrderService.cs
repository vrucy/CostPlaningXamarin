using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using CostPlaningXamarin.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
//using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(OrderService))]
namespace CostPlaningXamarin.Services
{
    public class OrderService : IOrderService
    {
        //TODO: DI
        private static readonly HttpClient _httpClient = new HttpClient();
        //private const string urlLocalHost = "http://10.0.2.2:54481/";
        private const string urlLocalHost = "http://192.168.1.88:80/";
        //private const string urlLocalHost = "http://192.168.1.88:54481/";
        ISQLiteService SQLiteService = DependencyService.Get<ISQLiteService>();

        private HttpContent MediaTypeHeaderValue(object o)
        {
            var data = JsonConvert.SerializeObject(o);
            var content = new StringContent(data);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return content;
        }

        public Task PostOrdersSync(List<Order> orders)
        {
            return _httpClient.PostAsync(urlLocalHost + "Order/PostOrders", MediaTypeHeaderValue(orders));
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

        public int GetLastOrderServerId()
        {
            return JsonConvert.DeserializeObject<int>(ResponseResult("Order/GetLastOrderServerId"));
        }
        public int GetOrdersCountFromServer()
        {
            return JsonConvert.DeserializeObject<int>(ResponseResult("Order/GetOrdersCountFromServer"));
        }
        public bool IsServerAvailable()
        {
            var res = _httpClient.GetAsync(urlLocalHost + "Order/IsServerAvailable");
            if (res.Result.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public List<int> AllDisableOrders()
        {
            return JsonConvert.DeserializeObject<List<int>>(ResponseResult("Order/SyncDisable"));

        }
    }
}
