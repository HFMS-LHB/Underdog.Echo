using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Underdog.Echo.Common.Helper;

namespace Underdog.Echo.Extensions.ServiceExtensions
{
    public static class HttpClientSetup
    {
        public static void AddHttpClientSetup(this IServiceCollection services)
        {
            services.AddSingleton(provider =>
            {
                var httpClient = new HttpClient();
                try
                {
                    var baseUriStr = AppSettings.app(["ExternalAPI", "BaseUri"]);
                    var timeOut = AppSettings.app(["ExternalAPI", "TimeOut"]);
                    if (!string.IsNullOrEmpty(baseUriStr))
                    {
                        httpClient.BaseAddress = new Uri(baseUriStr);
                    }
                    if (int.TryParse(timeOut, out int timeOutValue))
                    {
                        httpClient.Timeout = TimeSpan.FromSeconds(timeOutValue);
                    }
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
                }
                catch
                {
                    return new HttpClient();
                }
                return httpClient;
            });
        }
    }
}
