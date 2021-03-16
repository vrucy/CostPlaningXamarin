using CostPlaningXamarin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CostPlaningXamarin.Interfaces
{
    public interface ICategoryService
    {
        List<Category> GetCategories();
        int GetLastCategoryServerId();
        bool EditCategory(Category category, string deviceId);
        /// <summary>
        /// Returns ids of Categores with are disable
        /// </summary>
        /// <returns></returns>
        Dictionary<int, bool> GetAllCategoresVisibility(string deviceId);
        Task PostCategory(Category category,string deviceId);
        List<Category>GetUnsyncCategories(int lastCategoryId);
    }
}
