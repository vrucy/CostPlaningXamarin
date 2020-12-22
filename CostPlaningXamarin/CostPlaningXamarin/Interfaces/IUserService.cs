using CostPlaningXamarin.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CostPlaningXamarin.Interfaces
{
    interface IUserService
    {
        Task<User> PostAppUser();
        Task<bool> CheckNewUsers();
        Task<IList<User>> GetAllUsers();
        Task<IList<User>> GetAllUsersWithoutAppUser(int appUserId);
        int GetLastUserServerId();
    }
}
