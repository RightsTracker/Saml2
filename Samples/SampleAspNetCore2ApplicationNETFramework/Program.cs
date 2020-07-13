using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace SampleAspNetCore2ApplicationNETFramework
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddNLog("NLog.config");
            }).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();


        //public static void Main(string[] args)
        //{
        //    BuildWebHost(args).Run();
        //}

        //public static IWebHost BuildWebHost(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>()
        //        .Build();
    }
}
