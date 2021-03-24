using CostPlaningXamarin.Helper;
using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using CostPlaningXamarin.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(UserService))]
namespace CostPlaningXamarin.Services
{
    public class UserService: IUserService
    {
        //TODO: DI
        private static readonly HttpClientHelper _httpClient = new HttpClientHelper();

        public async Task<IList<User>> GetAllUsers()
        {
            return JsonConvert.DeserializeObject<List<User>>(await _httpClient.ResponseResult("User/GetAllUsers"));
        }

        public async Task<int> GetLastUserServerId()
        {
            return JsonConvert.DeserializeObject<int>(await _httpClient.ResponseResult("User/GetLastUserServerId"));
        }

        public async Task<List<User>> GetUnsyncUsers(int lastUserId)
        {
            return JsonConvert.DeserializeObject<List<User>>(await _httpClient.ResponseResult(string.Format("User/GetUnsyncUsers/{0}", lastUserId)));
        }
        public async Task PostDevice(Models.Device device)
        {
            await _httpClient.PostAsync(device, "User/PostDevice");
        }

        public async Task<User> PostUser(User user)
        {
            return JsonConvert.DeserializeObject<User>(await _httpClient.PostAsync(user, "User/PostUser"));         
        }
    }
}
