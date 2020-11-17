using System;
using System.Threading.Tasks;

namespace CostPlanningXamarin.Http.Net.Interfaces
{
    interface IHttpService
    {
        Task InvokeServerGetFunctionAsync(string domainName, string methodName, int count = 0);
        Task InvokeServerPostFunctionAsync(string domainName, string methodName, object parameter, int count = 0);
        Task<TResponse> InvokeServerGetFunctionWithResponseAsync<TResponse>(string domainName, string methodName, int count = 0);
        Task<TResponse> InvokeServerPostFunctionWithResponseAsync<TResponse>(string domainName, string methodName, object parameter, TimeSpan timeout, int count = 0);

        void InvokeServerGetFunction(string domainName, string methodName, int count = 0);
        void InvokeServerGetFunction(string domainName, string methodName, TimeSpan timeout, int count = 0);
        void InvokeServerPostFunction(string domainName, string methodName, object parameter, int count = 0);
        TResponse InvokeServerGetFunctionWithResponse<TResponse>(string domainName, string methodName, int count = 0);
        TResponse InvokeServerGetFunctionWithResponse<TResponse>(string domainName, string methodName, TimeSpan timeout, int count = 0);
        TResponse InvokeServerPostFunctionWithResponse<TResponse>(string domainName, string methodName, object parameter, int count = 0);
        TResponse InvokeServerPostFunctionWithResponse<TResponse>(string domainName, string methodName, object parameter, TimeSpan timeout, int count = 0);
    }
}
