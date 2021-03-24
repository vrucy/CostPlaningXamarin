using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CostPlaningXamarin.Helper
{
    public class HttpClientHelper: IHttpClientHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string urlLocalHost = Constants.urlLocalHost;

        public HttpContent MediaTypeHeaderValue(object o)
        {
            var data = JsonConvert.SerializeObject(o, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            var content = new StringContent(data);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return content;
        }
        public async Task<string> ResponseResult(string route)
        {
            var res = await _httpClient.GetAsync(urlLocalHost + route);

            return await res.Content.ReadAsStringAsync();
        }
        public async Task<string> PostAsync<T>(T postItem, string url) where T : class
        {
            var res = await _httpClient.PostAsync(urlLocalHost + url , MediaTypeHeaderValue(postItem));
            return await res.Content.ReadAsStringAsync();
        }

        public async Task<HttpResponseMessage> PutAsync<T>(T item, string url) where T : class
        {
            return  await _httpClient.PutAsync(urlLocalHost + url, MediaTypeHeaderValue(item));
        }

        public async Task<string> PutOrdersAsync(List<Order> orders,  string url)
        {
            var res = await _httpClient.PutAsync(urlLocalHost + url , MediaTypeHeaderValue(orders));
            return await res.Content.ReadAsStringAsync();
        }
    }
}
