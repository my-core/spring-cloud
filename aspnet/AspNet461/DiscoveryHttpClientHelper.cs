/*********************************************************************
*Copyright (c) 2018 深圳房讯通信息技术有限公司 All Rights Reserved.
*CLR版本： 4.0.30319.42000
*公司名称：深圳房讯通信息技术有限公司
*命名空间：MyAspNetCore.Core.Http
*文件名：  DiscoveryHttpClientHelper
*版本号：  V1.0.0.0
*创建人：  Mibin
*创建时间：2018-6-1 14:16:13
*描述：
*
*--------------多次修改可添加多块注释---------------
*修改时间：2018-6-1 14:16:13
*修改人： Mibin
*描述：first create
*
**********************************************************************/
using Newtonsoft.Json;
using Pivotal.Discovery.Client;
using Steeltoe.Common.Discovery;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AspNet461
{
    public static class DiscoveryHttpClientHelper
    {
        #region --- async 基于HttpClient ---
        //请求消息处理器
        private static DiscoveryHttpClientHandler _handler;

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> DoGetAsync(this IDiscoveryClient client, string url)
        {
            _handler = new DiscoveryHttpClientHandler(client);
            HttpClient httpClient = new HttpClient(_handler);
            return await httpClient.GetStringAsync(url);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> DoPostAsync(this IDiscoveryClient client, string url, object content)
        {
            _handler = new DiscoveryHttpClientHandler(client);
            HttpClient httpClient = new HttpClient(_handler);
            byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(content));
            using (StreamContent sc = new StreamContent(new MemoryStream(data)))
            {
                sc.Headers.ContentLength = data.Length;
                sc.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var result = await httpClient.PostAsync(url, sc);
                return await result.Content.ReadAsStringAsync();
            }
        }
        #endregion

        #region --- sync 基于HttpClient ---

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string DoGetSync(this IDiscoveryClient client, string url)
        {
            _handler = new DiscoveryHttpClientHandler(client);
            HttpClient httpClient = new HttpClient(_handler);
            return httpClient.GetStringAsync(url).Result;
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string DoPostSync(this IDiscoveryClient client, string url,object content)
        {
            _handler = new DiscoveryHttpClientHandler(client);
            HttpClient httpClient = new HttpClient(_handler);
            byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(content));
            using (StreamContent sc = new StreamContent(new MemoryStream(data)))
            {
                sc.Headers.ContentLength = data.Length;
                sc.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                HttpResponseMessage response=  httpClient.PostAsync(url, sc).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        #endregion

        #region --- sync 基于HttpWebRequest ---

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string DoGet(this IDiscoveryClient client, string url)
        {
            return DoRequest(client, url, HttpMethod.Get);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string DoPost(this IDiscoveryClient client, string url)
        {
            return DoRequest(client, url, HttpMethod.Post);
        }
        #endregion

        #region --- private method ---
        /// <summary>
        /// 请求url
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <param name="httpMethod"></param>
        /// <returns></returns>
        private static string DoRequest(IDiscoveryClient client, string url, HttpMethod httpMethod)
        {
            string result = "";
            try
            {
                Uri uri = client.LookupService(new Uri(url));
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = httpMethod.Method;
                
                try
                {
                    using (Stream stream = ((HttpWebResponse)request.GetResponse()).GetResponseStream())
                    {
                        result = new StreamReader(stream, Encoding.UTF8).ReadToEnd();
                    }
                }
                catch (WebException exception1)
                {
                    using (Stream stream2 = ((HttpWebResponse)exception1.Response).GetResponseStream())
                    {
                        result = new StreamReader(stream2).ReadToEnd();
                    }
                }
            }
            catch (Exception exception)
            {
                result = exception.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取服务地址
        /// </summary>
        /// <param name="client"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        private static Uri LookupService(this IDiscoveryClient client, Uri current)
        {
            if (!current.IsDefaultPort)
            {
                return current;
            }

            var instances = client.GetInstances(current.Host);
            if (instances.Count > 0)
            {
                int indx = new Random().Next(instances.Count);
                return new Uri(instances[indx].Uri, current.PathAndQuery);
            }

            return current;

        }
        #endregion
    }
}
