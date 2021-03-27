using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CostPlaningXamarin.Helper
{
    public class HttpClientHelper : IHttpClientHelper
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string urlLocalHost = Constants.urlLocalHost;
        private readonly NLog.ILogger _logger = NLog.LogManager.GetCurrentClassLogger();

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
        public async Task<string> ResponseResultAsync(string route)
        {
            try
            {
                var res = await _httpClient.GetAsync(urlLocalHost + route);
                _logger.Info("Send get request to: " + route);
                return await res.Content.ReadAsStringAsync();
            }
            catch (System.Exception e)
            {
                _logger.Warn("Exeption in getRequest: " + e.Message, "Inner: " + e.InnerException);
                throw;
            }
        }
        public async Task<string> PostAsync<T>(T postItem, string url) where T : class
        {
            try
            {
                var res = await _httpClient.PostAsync(urlLocalHost + url, MediaTypeHeaderValue(postItem));
                return await res.Content.ReadAsStringAsync();
            }
            catch (System.Exception e)
            {
                _logger.Error("PostAync<T>: " + e.Message + "Inner: " + e.InnerException);
                throw;
            }
        }

        public async Task<HttpResponseMessage> PutAsync<T>(T item, string url) where T : class
        {
            return await _httpClient.PutAsync(urlLocalHost + url, MediaTypeHeaderValue(item));
        }

        public async Task<string> PutOrdersAsync(List<Order> orders, string url)
        {
            try
            {
                var res = await _httpClient.PutAsync(urlLocalHost + url, MediaTypeHeaderValue(orders));
                return await res.Content.ReadAsStringAsync();
            }
            catch (System.Exception e)
            {
                _logger.Error("PutOrdersAsync: " + e.Message + "inner: " + e.InnerException);
                throw;
            }
        }
    }
}
