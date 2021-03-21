using CostPlaningXamarin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CostPlaningXamarin.Interfaces
{
    interface IUserService
    {
        Task<IList<User>> GetAllUsers();
        Task<int> GetLastUserServerId();
        Task<List<User>> GetUnsyncUsers(int lastUserId);
        Task PostDevice(Models.Device device);
        Task<User> PostUser(User user);
    }
}
