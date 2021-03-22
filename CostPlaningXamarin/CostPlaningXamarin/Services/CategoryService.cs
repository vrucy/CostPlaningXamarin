using CostPlaningXamarin.Helper;
using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using CostPlaningXamarin.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(CategoryService))]
namespace CostPlaningXamarin.Services
{
    public class CategoryService : ICategoryService
    {
        //TODO: DI
        private static readonly HttpClient _httpClient = new HttpClient();

        private const string urlLocalHost = Constants.urlLocalHost;
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
        private async Task<string> ResponseResult(string route)
        {
            var res = await _httpClient.GetAsync(urlLocalHost + route);

            return await res.Content.ReadAsStringAsync();
        }

        public async Task<List<Category>> GetCategories(string deviceId)
        {
            return JsonConvert.DeserializeObject<List<Category>>(await ResponseResult(string.Format("Category/GetGategories/{0}", deviceId)));
        }

        public async Task<int> GetLastCategoryServerId()
        {
            return JsonConvert.DeserializeObject<int>(await ResponseResult("Category/GetLastCategoryServerId"));
        }

        public async Task<bool> EditCategory(Category category, string deviceId)
        {
            var res =await _httpClient.PutAsync(urlLocalHost + string.Format("Category/EditCategory/{0}", deviceId), MediaTypeHeaderValue(category));

            if (res.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public async Task<Dictionary<int, bool>> GetAllCategoresVisibility(string deviceId)
        {
            return JsonConvert.DeserializeObject<Dictionary<int, bool>>(await ResponseResult(string.Format("Category/SyncVisbility/{0}", deviceId)));
        }
        public async Task<Category> PostCategory(Category category, string deviceId)
        {
            var res = await _httpClient.PostAsync(urlLocalHost + string.Format("Category/PostCategory/{0}", deviceId), MediaTypeHeaderValue(category));
            var content = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Category>(content);

        }
        public async Task<List<Category>> GetUnsyncCategories(int lastCategoryId)
        {
            return JsonConvert.DeserializeObject<List<Category>>(await ResponseResult(string.Format("Category/GetUnsyncCategories/{0}", lastCategoryId)));
        }
    }
}
