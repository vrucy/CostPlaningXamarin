using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using CostPlaningXamarin.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;
//using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(OrderService))]
namespace CostPlaningXamarin.Services
{
    public class OrderService : IOrderService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        //private const string url = "http://10.0.2.2:54481/";
        private const string url = "http://192.168.1.88:80/";
        private const string urlLocalHost = "http://192.168.1.88:45455/";
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

            var x = SQLiteService.GetOrdersUnsyncAsync();
            var y = MediaTypeHeaderValue(SQLiteService.GetOrdersUnsyncAsync());
            return await _httpClient.PostAsync(url + "Order/PostOrders", MediaTypeHeaderValue(SQLiteService.GetOrdersUnsyncAsync()));
        }

        public async Task<IList<Order>> GetAllOrders()
        {
            var res = await _httpClient.GetAsync(url + "Order/GetAllOrders");
            string responseBody = res.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<Order>>(responseBody);
        }
    }
}
