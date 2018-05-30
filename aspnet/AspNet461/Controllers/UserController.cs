using Steeltoe.Common.Discovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AspNet461.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        //请求消息处理器
        private readonly DiscoveryHttpClientHandler _handler;
        //调用地址，这里指定应用名[user]，服务中心会自动分配地址，并实现负载均衡
        private const string userUrl = "http://user/api/user";

        private IDiscoveryClient _client;

        /// <summary>
        /// 构造器注入
        /// </summary>
        /// <param name="client"></param>
        public UserController(IDiscoveryClient client)
        {
            _client = client;
            _handler = new DiscoveryHttpClientHandler(client);
        }
        /// <summary>
        /// 异步调用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> GetUserAsync()
        {
            var client = new HttpClient(_handler);
            return await client.GetStringAsync(userUrl);
        }

        /// <summary>
        ///  同步调用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("UserSync")]
        public string GetUserSync()
        {
            return _client.DoPost(userUrl);
        }
    }
}
