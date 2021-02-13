using CostPlaningXamarin.Models;
using System.Collections.Generic;

namespace CostPlaningXamarin.Interfaces
{
    public interface ICategoryService
    {
        Dictionary<int, int> PostCategories(List<Category> orders, int userId);
        List<Category> GetAllCategoriesByIds(List<int> ids);
        List<Category> GetCategories();
        int GetLastCategoryServerId();
        bool EditCategory(Category category, int userId);
        /// <summary>
        /// Returns ids of Categores with are disable
        /// </summary>
        /// <returns></returns>
        Dictionary<int, bool> GetAllCategoresVisibility(int appUserId);
        /// <summary>
        /// Send ids to server, who need to disable
        /// </summary>
        /// <param name="ids"></param>
        void SyncDisable(List<int> ids);
    }
}
