using CostPlaningXamarin.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CostPlaningXamarin.Interfaces
{
    interface IUserService
    {
        Task<User> PostAppUser();
        Task PostCategory(Category category);
        Task<IList<User>> GetAllUsers();
        int GetLastUserServerId();
        List<User> GetUnsyncUsers(int lastUserId);
    }
}
