using CostPlaningXamarin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CostPlaningXamarin.Interfaces
{
    interface IUserService
    {
        Task<IList<User>> GetAllUsers();
        int GetLastUserServerId();
        List<User> GetUnsyncUsers(int lastUserId);
        void PostDevice(Models.Device device);
        Task<User> PostUser(User user);
    }
}
