using CostPlaningXamarin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CostPlaningXamarin.Interfaces
{
    public interface ICategoryService
    {
        Dictionary<int, int> PostCategories(List<Category> orders, string deviceId);
        List<Category> GetAllCategoriesByIds(List<int> ids);
        List<Category> GetCategories();
        int GetLastCategoryServerId();
        bool EditCategory(Category category, string deviceId);
        /// <summary>
        /// Returns ids of Categores with are disable
        /// </summary>
        /// <returns></returns>
        Dictionary<int, bool> GetAllCategoresVisibility(string deviceId);
        Task PostCategory(Category category,string deviceId);
    }
}
