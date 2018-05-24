using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pivotal.Discovery.Client;
using Steeltoe.Discovery.Eureka;

namespace OrderApi.Controllers
{
    [Route("api/Order")]
    public class OrderController : Controller
    {
        //请求消息处理器
        private readonly DiscoveryHttpClientHandler _handler;
        //调用地址，这里指定应用名[user]，服务中心会自动分配地址，并实现负载均衡
        private const string userUrl = "http://user/api/user";


        public OrderController(IDiscoveryClient client, ILoggerFactory loggerFactory)
        {
            _handler = new DiscoveryHttpClientHandler(client);
        }
        /// <summary>
        /// 在Order服务里调User服务中，获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("User")]
        public async Task<string> GetUserAsync()
        {
            var client = new HttpClient(_handler);
            return await client.GetStringAsync(userUrl);
        }

        /// <summary>
        /// 获取服务中心所有服务和它的Url地址
        /// </summary>
        /// <returns></returns>
        [HttpGet("Service")]
        public IEnumerable<string> Get()
        {
            DiscoveryClient discoveryClient = new DiscoveryClient(new EurekaClientConfig
            {
                EurekaServerServiceUrls = "http://localhost:8761/eureka/",
                ProxyHost = "http://localhost:8761/eureka/",
                ProxyPort = 8761,

            });
            foreach (var item in discoveryClient.Applications.GetRegisteredApplications())
            {
                yield return $"{item.Name}={  string.Join(",", (from p in item.Instances select p.HomePageUrl).ToArray()) }";
            }
        }

    }
}
