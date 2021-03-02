﻿using CostPlaningXamarin.Helper;
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
        public async Task<User> PostAppUser()
        {
            var appUser = SQLiteService.GetAppUser();
            var res = _httpClient.PostAsync(urlLocalHost + "User/PostAppUser", MediaTypeHeaderValue(appUser)).GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<User>(res.Content.ReadAsStringAsync().Result);
        }
        public Task PostCategory(Category category)
        {
            return _httpClient.PostAsync(urlLocalHost + "User/PostCategory", MediaTypeHeaderValue(category));
        }
        public async Task<IList<User>> GetAllUsers()
        {
            var res = await _httpClient.GetAsync(urlLocalHost + "Order/GetAllUsers");
            string responseBody = res.Content.ReadAsStringAsync().Result;

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

        public Task PostUsers(List<User> users)
        {
            throw new NotImplementedException();
        }
    }
}
