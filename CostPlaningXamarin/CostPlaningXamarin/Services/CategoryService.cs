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
        //private const string urlLocalHost = "http://192.168.1.88:80/";
        private const string urlLocalHost = "http://192.168.1.88:54481/";
        private HttpContent MediaTypeHeaderValue(object o)
        {
            try
            {
                var data = JsonConvert.SerializeObject(o,Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                var content = new StringContent(data);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return content;
            }
            catch (System.Exception e)
            {

                throw;
            }
        }
        private string ResponseResult(string route)
        {
            var res = _httpClient.GetAsync(urlLocalHost + route).GetAwaiter().GetResult();
            return res.Content.ReadAsStringAsync().Result;
        }
        public Dictionary<int, int> UpdateCategories(List<Category> categories)
        {
            try
            {
            var res = _httpClient.PostAsync(urlLocalHost + "Category/UpdateCategories", MediaTypeHeaderValue(categories)).GetAwaiter().GetResult();

            var content = res.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<Dictionary<int, int>>(content);
            }
            catch (System.Exception e)
            {

                throw;
            }


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
    }
}
