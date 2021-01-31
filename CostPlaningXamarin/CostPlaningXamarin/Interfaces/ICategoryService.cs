using CostPlaningXamarin.Models;
using System.Collections.Generic;

namespace CostPlaningXamarin.Interfaces
{
    public interface ICategoryService
    {
        Dictionary<int, int> UpdateCategories(List<Category> orders);
        List<Category> GetAllCategoriesByIds(List<int> ids);
        List<Category> GetCategories();
        int GetLastCategoryServerId();

    }
}
