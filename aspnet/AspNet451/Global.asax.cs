using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Extensions.Logging;
using Pivotal.Discovery.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AspNet451
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// 客户端
        /// </summary>
        private IDiscoveryClient _client;

        /// <summary>
        /// 程序开始
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //全局配置
            var config = GlobalConfiguration.Configuration;

            // Build application configuration
            ApplicationConfig.RegisterConfig("development");

            // /autofac IoC容器
            var builder = new ContainerBuilder();

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Register IDiscoveryClient, etc.
            builder.RegisterDiscoveryClient(ApplicationConfig.Configuration, null);

            //设置全局依赖注入解析器
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // Start the Discovery client background thread
            _client = container.Resolve<IDiscoveryClient>();
        }

        protected void Application_End()
        {
            //释放客户端
            _client.ShutdownAsync();
        }
    }
}
