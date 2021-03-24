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
        Task<Dictionary<int, bool>> GetAllCategoresVisibility(string deviceId);
        Task<Category> PostCategory(Category category,string deviceId);
        Task<List<Category>> GetUnsyncCategories(int lastCategoryId);
    }
}
