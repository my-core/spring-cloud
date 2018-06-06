/*********************************************************************
*Copyright (c) 2018 深圳房讯通信息技术有限公司 All Rights Reserved.
*CLR版本： 4.0.30319.42000
*公司名称：深圳房讯通信息技术有限公司
*命名空间：AspNet461.App_Start
*文件名：  ApplicationConfig
*版本号：  V1.0.0.0
*创建人：  Mibin
*创建时间：2018-5-29 16:24:17
*描述：
*
*-----------------------------
*修改时间：2018-5-29 16:24:17
*修改人： Mibin
*描述：first create
*
**********************************************************************/
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Steeltoe.Extensions.Configuration;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;


namespace AspNet451_WebForm
{
    public class ApplicationConfig
    {
        public static IConfigurationRoot Configuration { get; set; }

        public static void RegisterConfig(string environment)
        {
            var env = new HostingEnvironment(environment);
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddCloudFoundry()
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }


        public class HostingEnvironment : IHostingEnvironment
        {
            public HostingEnvironment(string env)
            {
                EnvironmentName = env;

                ApplicationName = "";
                ContentRootPath = GetContentRoot();
            }

            public string GetContentRoot()
            {
                var basePath = (string)AppDomain.CurrentDomain.GetData("APP_CONTEXT_BASE_DIRECTORY") ??
                   AppDomain.CurrentDomain.BaseDirectory;
                return Path.GetFullPath(basePath);
            }

            public string ApplicationName { get; set; }

            public IFileProvider ContentRootFileProvider { get; set; }

            public string ContentRootPath { get; set; }

            public string EnvironmentName { get; set; }
            public object PlatformServices { get; private set; }
            public IFileProvider WebRootFileProvider { get; set; }

            public string WebRootPath { get; set; }

            IFileProvider IHostingEnvironment.WebRootFileProvider { get; set; }
        }
    }
}