/*********************************************************************
*Copyright (c) 2018 深圳房讯通信息技术有限公司 All Rights Reserved.
*CLR版本： 4.0.30319.42000
*公司名称：深圳房讯通信息技术有限公司
*命名空间：AspNet461
*文件名：  DiscoveryHelper
*版本号：  V1.0.0.0
*创建人：  Mibin
*创建时间：2018-6-5 10:18:23
*描述：
*
*-----------------------------
*修改时间：2018-6-5 10:18:23
*修改人： Mibin
*描述：first create
*
**********************************************************************/
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pivotal.Discovery.Client;
using Steeltoe.CloudFoundry.Connector;
using Steeltoe.CloudFoundry.Connector.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNet451_WebForm
{
    public class DiscoveryHelper
    {
        /// <summary>
        /// 客户端实例
        /// </summary>
        public static IDiscoveryClient discoveryClient { get; set; }
       
        /// <summary>
        /// 注册客户端（在程序入口调用，如Global类的App_Start方法）
        /// </summary>
        /// <param name="config"></param>
        public static void RegisterDiscoveryClient(IConfigurationRoot config)
        {
            EurekaServiceInfo info = config.GetSingletonServiceInfo<EurekaServiceInfo>();
            DiscoveryOptions configOptions = new DiscoveryOptions(config)
            {
                ClientType = DiscoveryClientType.EUREKA
            };

            DiscoveryClientFactory factory = new DiscoveryClientFactory(info, configOptions);
            discoveryClient = (IDiscoveryClient)factory.CreateClient(null, null);
        }
    }
}