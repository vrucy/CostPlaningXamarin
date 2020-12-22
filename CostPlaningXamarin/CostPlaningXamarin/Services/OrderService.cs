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
        private static readonly HttpClient _httpClient = new HttpClient();
        //private const string urlLocalHost = "http://10.0.2.2:54481/";
        //private const string urlLocalHost = "http://192.168.1.88:80/";
        private const string urlLocalHost = "http://192.168.1.88:54481/";
        ISQLiteService SQLiteService = DependencyService.Get<ISQLiteService>();

        private HttpContent MediaTypeHeaderValue(object o)
        {
            var data = JsonConvert.SerializeObject(o);
            var content = new StringContent(data);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return content;
        }
        public async Task<HttpResponseMessage> PostOrders()
        {
            //TODO: check if return ok;
            try
            {
                //return await _httpClient.PostAsync(urlLocalHost + "Order/PostOrders", MediaTypeHeaderValue(SQLiteService.GetOrdersUnsyncAsync()));
                var x = SQLiteService.GetOrdersUnsyncAsync().Result;
                if (x.Count > 0)
                {
                    return await _httpClient.PostAsync(urlLocalHost + "Order/PostOrders", MediaTypeHeaderValue(x));

                }
                return null;
            }
            catch (System.Exception e)
            {

                throw;
            }
        }
        public Task PostOrdersSync(List<Order> orders)
        {
            return _httpClient.PostAsync(urlLocalHost + "Order/PostOrders", MediaTypeHeaderValue(orders));
        }

        //public async Task<User> PostUser()
        //{
        //    return await SQLiteService.GetUser();
        //}
        public async Task<IList<Order>> GetAllOrders()
        {
            //var res = await _httpClient.GetAsync(urlLocalHost + "Order/GetAllOrders");
            //string responseBody = res.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<Order>>(ResponseResult("Order/GetAllOrders"));
        }


        public Task<bool> CheckCategories()
        {
            throw new System.NotImplementedException();
        }

        private string ResponseResult(string route)
        {
            var res = _httpClient.GetAsync(urlLocalHost + route).GetAwaiter().GetResult();
            return res.Content.ReadAsStringAsync().Result;
        }

        public Dictionary<int, int> UpdateOrder(List<Order> orders)
        {
            try
            {
                var res = _httpClient.PostAsync(urlLocalHost + "Order/UpdateOrders", MediaTypeHeaderValue(orders)).GetAwaiter().GetResult();

                var content = res.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return JsonConvert.DeserializeObject<Dictionary<int, int>>(content);
            }
            catch (Exception r)
            {

                throw;
            }
        }
        //TODO: need to be GetAsync!!
        public List<Order> GetOrdersByIds(List<int> ids)
        {
            try
            {
            var res = _httpClient.PostAsync(urlLocalHost + "Order/GetAllOrdersByIds" , MediaTypeHeaderValue(ids)).GetAwaiter().GetResult();

            var content = res.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var res1 = JsonConvert.DeserializeObject<List<Order>>(content);
                return JsonConvert.DeserializeObject<List<Order>>(content);

            }
            catch (Exception e)
            {

                throw;
            }
        }

        public int GetLastOrderServerId()
        {
            return JsonConvert.DeserializeObject<int>(ResponseResult("Order/GetLastOrderServerId"));
        }
    }
}
