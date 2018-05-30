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
using Microsoft.Extensions.Configuration;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AspNet461
{
    public class ApplicationConfig
    {
        public static IConfigurationRoot Configuration { get; set; }

        public static void RegisterConfig(string environment)
        {

            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(GetContentRoot())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddCloudFoundry()
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public static string GetContentRoot()
        {
            var basePath = (string)AppDomain.CurrentDomain.GetData("APP_CONTEXT_BASE_DIRECTORY") ??
               AppDomain.CurrentDomain.BaseDirectory;
            return Path.GetFullPath(basePath);
        }
    }
}