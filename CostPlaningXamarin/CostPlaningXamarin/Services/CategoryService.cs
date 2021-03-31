using CostPlaningXamarin.Helper;
using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using CostPlaningXamarin.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(CategoryService))]
namespace CostPlaningXamarin.Services
{
    public class CategoryService : ICategoryService
    {
        //TODO: DI
        private static readonly HttpClientHelper _httpClient = new HttpClientHelper();

        public async Task<List<Category>> GetCategories(string deviceId)
        {
            return JsonConvert.DeserializeObject<List<Category>>(await _httpClient.ResponseResultAsync(string.Format("Category/GetGategories/{0}", deviceId)));
        }

        public async Task<bool> EditCategory(Category category, string deviceId)
        {
            var res = await _httpClient.PutAsync(category, string.Format("Category/EditCategory/{0}",deviceId));

            if (res.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public async Task<Category> PostCategory(Category category, string deviceId)
        {
            return JsonConvert.DeserializeObject<Category>(await _httpClient.PostAsync(category, string.Format("Category/PostCategory/{0}",deviceId)));
        }
        public async Task<List<Category>> GetUnsyncCategories(string deviceId)
        {
            return JsonConvert.DeserializeObject<List<Category>>(await _httpClient.ResponseResultAsync(string.Format("Category/GetUnsyncCategories/{0}", deviceId)));
        }
    }
}
