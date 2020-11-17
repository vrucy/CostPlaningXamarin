using CostPlanningXamarin.Http.Net.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CostPlanningXamarin.Http.Net.Services
{
    public class HttpService : IHttpService
    {
        private readonly IServerLocator _serverLocator;
        private static object _tokenLocker = new object();
        private static string BearerToken;
        //private static ILogger _logger;
        public static readonly TimeSpan DefaultHttpClientTimeout = TimeSpan.FromSeconds(1500);

        public HttpService(IServerLocator serverLocator/*, ILogger logger*/)
        {
            _serverLocator = serverLocator;
            //_logger = logger;
        }

        //public static string GetBearerToken()
        //{
        //    lock (_tokenLocker)
        //    {
        //        if (BearerToken == null) BearerToken = CreateBearerToken();
        //        return BearerToken;
        //    }
        //}

        //public static string CreateBearerToken()
        //{
        //    // discover endpoints from metadata
        //    var client = new HttpClient();
        //    var disco = client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
        //    {
        //        Address = ConfigurationManager.AppSettings["IdentityServer"],
        //        Policy =
        //        {
        //            RequireHttps = false
        //        }
        //    });

        //    if (disco.Result.IsError)
        //    {
        //        _logger.Error($"Could not get the discovery document from Identity Server! Exception: {disco.Result.Exception}");
        //    }

        //    // request token
        //    var tokenResponse = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        //    {
        //        Address = disco.Result.TokenEndpoint,
        //        ClientId = ConfigurationManager.AppSettings["AppName"],
        //        ClientSecret = "ba767eb9-7b17-987f-145e-11719d0a55ff",
        //        Scope = "ComDataUpdaterServer",
        //    });

        //    if (tokenResponse.Result.IsError)
        //    {
        //        //_logger.Error($"Could not get a token for the client from Identity Server! Exception: {disco.Result.Exception}");
        //    }

        //    return tokenResponse.Result.AccessToken;
        //}

        private HttpClientHandler CreateDefaultHandler()
        {
            return new HttpClientHandler { UseDefaultCredentials = true };
        }

        private HttpClient CreateDefaultHttpClient(string domainName)
        {
            return CreateDefaultHttpClient(domainName, DefaultHttpClientTimeout);
        }

        private HttpClient CreateDefaultHttpClient(string domainName, TimeSpan timeout)
        {
            var fullAddress = _serverLocator
                .GetServerAddress()
                .Format(domainName);

            var httpClient = new HttpClient(CreateDefaultHandler()) { BaseAddress = new Uri(fullAddress), Timeout = timeout };

            //var token = GetBearerToken();
            //httpClient.SetBearerToken(token);

            return httpClient;
        }

        public Task<TResponse> InvokeServerPostFunctionWithResponseAsync<TResponse>(string domainName, string methodName, object parameter, int count = 0)
        {
            return Task.Run<TResponse>(() => InvokeServerPostFunctionWithResponse<TResponse>(domainName, methodName, parameter, DefaultHttpClientTimeout));
        }

        public Task<TResponse> InvokeServerPostFunctionWithResponseAsync<TResponse>(string domainName, string methodName, object parameter, TimeSpan timeout, int count = 0)
        {
            return Task.Run<TResponse>(() => InvokeServerPostFunctionWithResponse<TResponse>(domainName, methodName, parameter, timeout));
        }

        public TResponse InvokeServerPostFunctionWithResponse<TResponse>(string domainName, string methodName, object parameter, int count = 0)
        {
            try
            {
                using (var client = CreateDefaultHttpClient(domainName, DefaultHttpClientTimeout))
                using (var response = client.PostAsJsonAsync(methodName, parameter).Result)
                {
                    EnsureSuccessStatusCode(response);
                    return response.Content.ReadAsJsonAsync<TResponse>().Result;
                }
            }
            catch (HttpRequestException hre)
            {
                //BearerToken = null;
                //BearerToken = GetBearerToken();
                //count++;
                //if (count < 2)
                //{
                //    return InvokeServerPostFunctionWithResponse<TResponse>(domainName, methodName, parameter, count);
                //}
            }
            catch (Exception ex)
            {

            }
            return default(TResponse);
        }

        public TResponse InvokeServerPostFunctionWithResponse<TResponse>(string domainName, string methodName, object parameter, TimeSpan timeout, int count = 0)
        {
            try
            {
                using (var client = CreateDefaultHttpClient(domainName, timeout))
                using (var response = client.PostAsJsonAsync(methodName, parameter).Result)
                {
                    EnsureSuccessStatusCode(response);
                    return response.Content.ReadAsJsonAsync<TResponse>().Result;
                }
            }
            catch (HttpRequestException hre)
            {
                //BearerToken = null;
                //BearerToken = GetBearerToken();
                //count++;
                //if (count < 2)
                //{
                //    return InvokeServerPostFunctionWithResponse<TResponse>(domainName, methodName, parameter, timeout, count);
                //}
            }
            catch (Exception ex)
            {

            }
            return default(TResponse);
        }

        public Task<TResponse> InvokeServerGetFunctionWithResponseAsync<TResponse>(string domainName, string methodName, int count = 0)
        {
            return Task.Run<TResponse>(() => InvokeServerGetFunctionWithResponse<TResponse>(domainName, methodName));
        }

        public TResponse InvokeServerGetFunctionWithResponse<TResponse>(string domainName, string methodName, int count = 0)
        {
            return InvokeServerGetFunctionWithResponse<TResponse>(domainName, methodName, DefaultHttpClientTimeout);
        }

        public TResponse InvokeServerGetFunctionWithResponse<TResponse>(string domainName, string methodName, TimeSpan timeout, int count = 0)
        {
            try
            {
                using (var client = CreateDefaultHttpClient(domainName, timeout))
                using (var result = client.GetAsync(methodName).Result)
                {
                    EnsureSuccessStatusCode(result);
                    return result.Content.ReadAsJsonAsync<TResponse>().Result;
                }
            }
            catch (HttpRequestException hre)
            {
                //BearerToken = null;
                //BearerToken = GetBearerToken();
                //count++;
                //if (count < 2)
                //{
                //    return InvokeServerGetFunctionWithResponse<TResponse>(domainName, methodName, timeout, count);
                //}
            }
            catch (Exception ex)
            {

            }
            return default(TResponse);
        }

        public Task InvokeServerPostFunctionAsync(string domainName, string methodName, object parameter, int count = 0)
        {
            return Task.Run(() => InvokeServerPostFunctionAsync(domainName, methodName, parameter));
        }

        public void InvokeServerPostFunction(string domainName, string methodName, object parameter, int count = 0)
        {
            try
            {
                using (var client = CreateDefaultHttpClient(domainName))
                using (var response = client.PostAsJsonAsync(methodName, parameter).Result)
                {
                    EnsureSuccessStatusCode(response);
                }
            }
            catch (HttpRequestException hre)
            {
                //BearerToken = null;
                //BearerToken = GetBearerToken();
                //count++;
                //if (count < 2)
                //{
                //    InvokeServerPostFunction(domainName, methodName, parameter, count);
                //}
            }
            catch (Exception ex)
            {

            }
        }

        public Task InvokeServerGetFunctionAsync(string domainName, string methodName, int count = 0)
        {
            return Task.Run(() => InvokeServerGetFunction(domainName, methodName));
        }

        public void InvokeServerGetFunction(string domainName, string methodName, int count = 0)
        {
            InvokeServerGetFunction(domainName, methodName, DefaultHttpClientTimeout);
        }

        public void InvokeServerGetFunction(string domainName, string methodName, TimeSpan timeout, int count = 0)
        {
            try
            {
                using (var client = CreateDefaultHttpClient(domainName, timeout))
                using (var result = client.GetAsync(methodName).Result)
                {
                    EnsureSuccessStatusCode(result);
                }
            }
            catch (HttpRequestException hre)
            {
                //BearerToken = null;
                //BearerToken = GetBearerToken();
                //count++;
                //if (count < 2)
                //{
                //    InvokeServerGetFunction(domainName, methodName, timeout, count);
                //}
            }
            catch (Exception ex)
            {

            }
        }

        private void EnsureSuccessStatusCode(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                using (HttpContent content = response.Content)
                {
                    Task<string> result = content.ReadAsStringAsync();
                    //var message = JsonConvert.DeserializeObject<ErrorMessage>(result.Result);
                    throw new Exception(result.Result);
                }
            }
        }

        private class ErrorMessage
        {
            public string Message { get; set; }
        }
    }
}
