using CostPlaningXamarin.Helper;
using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using CostPlaningXamarin.Services;
using Newtonsoft.Json;
using System;
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
            return JsonConvert.DeserializeObject<List<User>>(ResponseResult("User/GetAllUsers"));
        }
        private string ResponseResult(string route)
        {
            var res = _httpClient.GetAsync(urlLocalHost + route).GetAwaiter().GetResult();
            return res.Content.ReadAsStringAsync().Result;
        }

        public int GetLastUserServerId()
        {
            return JsonConvert.DeserializeObject<int>(ResponseResult("User/GetLastUserServerId"));
        }

        public List<User> GetUnsyncUsers(int lastUserId)
        {
            var x = ResponseResult(string.Format("User/GetUnsyncUsers/{0}" , lastUserId));
            return JsonConvert.DeserializeObject<List<User>>(x);
        }
        public void PostDevice(Models.Device device)
        {
            _httpClient.PostAsync(urlLocalHost + "User/PostDevice", MediaTypeHeaderValue(device));
        }

        public async Task<User> PostUser(User user)
        {
                var res = await _httpClient.PostAsync(urlLocalHost + "User/PostUser", MediaTypeHeaderValue(user))/*.GetAwaiter().GetResult()*/;
                var u = JsonConvert.DeserializeObject<User>(await res.Content.ReadAsStringAsync()/*.Result*/);
                return u;           
        }
    }
}
