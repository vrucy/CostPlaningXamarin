using CostPlaningXamarin.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CostPlaningXamarin.Interfaces
{
    interface IHttpClientHelper
    {
        HttpContent MediaTypeHeaderValue(object o);
        Task<string> ResponseResult(string route);
        Task<string> PostAsync<T>(T postItem, string url) where T : class;
        Task<HttpResponseMessage> PutAsync<T>(T item,  string url) where T : class;
        Task<string> PutOrdersAsync(List<Order> orders, string url) ;
    }
}
