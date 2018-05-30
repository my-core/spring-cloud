/*********************************************************************
*Copyright (c) 2018 深圳房讯通信息技术有限公司 All Rights Reserved.
*CLR版本： 4.0.30319.42000
*公司名称：深圳房讯通信息技术有限公司
*命名空间：AspNet451.App_Start
*文件名：  DiscoveryContainerBuilderExtensions
*版本号：  V1.0.0.0
*创建人：  Mibin
*创建时间：2018-5-30 10:00:47
*描述：
*
*-----------------------------
*修改时间：2018-5-30 10:00:47
*修改人： Mibin
*描述：first create
*
**********************************************************************/
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pivotal.Discovery.Client;
using Steeltoe.CloudFoundry.Connector;
using Steeltoe.CloudFoundry.Connector.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNet451
{
    public static class DiscoveryContainerBuilderExtensions
    {
        public static void RegisterLoggingFactory(this ContainerBuilder container, ILoggerFactory factory)
        {
            container.RegisterInstance<ILoggerFactory>(factory).SingleInstance();
        }

        public static void RegisterDiscoveryClient(this ContainerBuilder container, IConfigurationRoot config, ILoggerFactory loggerFactory)
        {
            EurekaServiceInfo info = config.GetSingletonServiceInfo<EurekaServiceInfo>();
            DiscoveryOptions configOptions = new DiscoveryOptions(config)
            {
                ClientType = DiscoveryClientType.EUREKA
            };

            DiscoveryClientFactory factory = new DiscoveryClientFactory(info, configOptions);
            container.Register<IDiscoveryClient>(c => (IDiscoveryClient)factory.CreateClient(null, loggerFactory)).SingleInstance();
        }
    }
}