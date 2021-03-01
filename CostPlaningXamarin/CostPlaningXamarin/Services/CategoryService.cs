using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using CostPlaningXamarin.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

[assembly: Xamarin.Forms.Dependency(typeof(CategoryService))]
namespace CostPlaningXamarin.Services
{
    public class CategoryService : ICategoryService
    {
        //TODO: DI
        private static readonly HttpClient _httpClient = new HttpClient();
        //private const string urlLocalHost = "http://10.0.2.2:54481/";
        private const string urlLocalHost = "http://192.168.1.88:80/";
        //private const string urlLocalHost = "http://192.168.1.88:54481/";
        private HttpContent MediaTypeHeaderValue(object o)
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
        private string ResponseResult(string route)
        {

            var res = _httpClient.GetAsync(urlLocalHost + route).GetAwaiter().GetResult();
            return res.Content.ReadAsStringAsync().Result;
        }
        public Dictionary<int, int> PostCategories(List<Category> categories,int userId)
        {
            var res = _httpClient.PostAsync(urlLocalHost + string.Format("Category/PostCategories/{0}", userId), MediaTypeHeaderValue(categories)).GetAwaiter().GetResult();

            var content = res.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<Dictionary<int, int>>(content);
        }

        public List<Category> GetAllCategoriesByIds(List<int> ids)
        {
            var res = _httpClient.PostAsync(urlLocalHost + "Category/GetAllCategoriesByIds", MediaTypeHeaderValue(ids)).GetAwaiter().GetResult();

            var content = res.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<List<Category>>(content);
        }

        public List<Category> GetCategories()
        {
            return JsonConvert.DeserializeObject<List<Category>>(ResponseResult("Category/GetGategories"));
        }

        public int GetLastCategoryServerId()
        {
            return JsonConvert.DeserializeObject<int>(ResponseResult("Category/GetLastCategoryServerId"));
        }

        public bool EditCategory(Category category,int userId)
        {
            var res = _httpClient.PutAsync(urlLocalHost + string.Format("Category/EditCategory/{0}",userId), MediaTypeHeaderValue(category)).GetAwaiter().GetResult();

            if (res.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public Dictionary<int, bool> GetAllCategoresVisibility(int appUserId)
        {
            return JsonConvert.DeserializeObject<Dictionary<int, bool>>(ResponseResult(string.Format("Category/SyncVisbility/{0}", appUserId)));
        }

        //public void SyncDisable(List<int> ids)
        //{
        //    _httpClient.PutAsync(urlLocalHost + "Category/SyncDisableOnServer", MediaTypeHeaderValue(ids)).Wait();
        //}
    }
}
