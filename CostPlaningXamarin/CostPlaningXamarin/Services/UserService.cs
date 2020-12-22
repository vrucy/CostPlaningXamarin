using CostPlaningXamarin.Interfaces;
using CostPlaningXamarin.Models;
using CostPlaningXamarin.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
[assembly: Xamarin.Forms.Dependency(typeof(UserService))]
namespace CostPlaningXamarin.Services
{
    public class UserService: IUserService
    {
        //TODO: DI
        private static readonly HttpClient _httpClient = new HttpClient();

        private const string urlLocalHost = "http://192.168.1.88:54481/";
        ISQLiteService SQLiteService = DependencyService.Get<ISQLiteService>();

        public void SyncUsers()
        {
            ResponseResult("Order/GetAllUsers");
        }
        private HttpContent MediaTypeHeaderValue(object o)
        {
            var data = JsonConvert.SerializeObject(o);
            var content = new StringContent(data);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return content;
        }
        public async Task<User> PostAppUser()
        {
            var appUser = SQLiteService.GetAppUser();
            var res = _httpClient.PostAsync(urlLocalHost + "User/PostAppUser", MediaTypeHeaderValue(appUser)).GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<User>(res.Content.ReadAsStringAsync().Result);
        }
        public async Task<IList<User>> GetAllUsers()
        {
            //var res = await _httpClient.GetAsync(urlLocalHost + "Order/GetAllUsers");
            //string responseBody = res.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<User>>(ResponseResult("User/GetAllUsers"));
        }

        public async Task<bool> CheckNewUsers()
        {

            //var res = _httpClient.GetAsync(urlLocalHost + "Order/GetNumberOfUsers").GetAwaiter().GetResult();
            //var responseBody = res.Content.ReadAsStringAsync().Result;

            //TODO: Not good codition
            return SQLiteService.CountUserInMobile().GetAwaiter().GetResult() != Int32.Parse(ResponseResult("User/GetNumberOfUsers"));
        }
        public async Task<IList<User>> GetAllUsersWithoutAppUser(int appUserId)
        {
            var res = _httpClient.PostAsync(urlLocalHost + "User/GetAllUsersWithoutAppUser", MediaTypeHeaderValue(appUserId))/*.GetAwaiter().GetResult()*/;
            return JsonConvert.DeserializeObject<List<User>>(await res.Result.Content.ReadAsStringAsync());
        }
        private string ResponseResult(string route)
        {
            var res = _httpClient.GetAsync(urlLocalHost + route).GetAwaiter().GetResult();
            return res.Content.ReadAsStringAsync().Result;
        }

        public int GetLastUserServerId()
        {
            return JsonConvert.DeserializeObject<int>(ResponseResult("Order/GetLastUserServerId"));
        }
    }
}
