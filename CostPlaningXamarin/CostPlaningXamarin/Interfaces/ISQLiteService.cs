using CostPlaningXamarin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CostPlaningXamarin.Interfaces
{
    public interface ISQLiteService
    {
        void CreateDBAsync(string path);
        void SaveOrderAsync(Order order);
        void SaveAllOrders(IList<Order> orders);
        void DeleteAllOrders();
        Task<List<Order>> GetOrdersAsync();
        Task<List<Order>> GetOrdersUnsyncAsync();
        Task<List<User>> GetUsers();
        Task<List<Category>> GetAllCategories();
    }
}
