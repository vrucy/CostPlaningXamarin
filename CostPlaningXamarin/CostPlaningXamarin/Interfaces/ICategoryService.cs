using CostPlaningXamarin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CostPlaningXamarin.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategories(string deviceId);
        Task<int> GetLastCategoryServerId();
        Task<bool> EditCategory(Category category, string deviceId);
        /// <summary>
        /// Returns ids of Categores with are disable
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<int, bool>> GetAllCategoresVisibility(string deviceId);
        Task PostCategory(Category category,string deviceId);
        Task<List<Category>> GetUnsyncCategories(int lastCategoryId);
    }
}
