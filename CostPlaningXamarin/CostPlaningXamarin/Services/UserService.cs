using CostPlaningXamarin.Helper;
using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using CostPlaningXamarin.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(UserService))]
namespace CostPlaningXamarin.Services
{
    public class UserService: IUserService
    {
        //TODO: DI
        private static readonly HttpClient _httpClient = new HttpClient();

        private const string urlLocalHost = Constants.urlLocalHost;
        ISQLiteService SQLiteService = DependencyService.Get<ISQLiteService>();

        private HttpContent MediaTypeHeaderValue(object o)
        {
            var data = JsonConvert.SerializeObject(o);
            var content = new StringContent(data);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return content;
        }
        public async Task<IList<User>> GetAllUsers()
        {
            return JsonConvert.DeserializeObject<List<User>>(await ResponseResult("User/GetAllUsers"));
        }
        private async Task<string> ResponseResult(string route)
        {
            var res = await _httpClient.GetAsync(urlLocalHost + route);
            return await res.Content.ReadAsStringAsync();
        }

        public async Task<int> GetLastUserServerId()
        {
            return JsonConvert.DeserializeObject<int>(await ResponseResult("User/GetLastUserServerId"));
        }

        public async Task<List<User>> GetUnsyncUsers(int lastUserId)
        {
            var x = await ResponseResult(string.Format("User/GetUnsyncUsers/{0}" , lastUserId));
            return JsonConvert.DeserializeObject<List<User>>(x);
        }
        public async Task PostDevice(Models.Device device)
        {
            await _httpClient.PostAsync(urlLocalHost + "User/PostDevice", MediaTypeHeaderValue(device));
        }

        public async Task<User> PostUser(User user)
        {
            var res = await _httpClient.PostAsync(urlLocalHost + "User/PostUser", MediaTypeHeaderValue(user));

                var u = JsonConvert.DeserializeObject<User>(await res.Content.ReadAsStringAsync());
                return u;           
        }
    }
}
