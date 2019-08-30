using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NLog.Web;
using System;
using System.Net;

namespace Lys.NetCore.Portal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Info("程序初始化");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "程序出异常");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var envConfig = new ConfigurationBuilder()
                .AddJsonFile("environment.json", optional: true)
                .Build();
            var environmentName = envConfig.GetValue<string>("EnvironmentName");
            var httpsPort = envConfig.GetValue<int>("https.port");

            return WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(true)
                .UseSetting(WebHostDefaults.DetailedErrorsKey, "true")
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.Sources.Clear();
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true);
                })
                .UseKestrel(options =>
                {
                    // 配置https证书
                    options.Listen(IPAddress.Any, httpsPort, listenOptions =>
                    {
                        listenOptions.UseHttps("Cert/testCert.pfx", "1234");
                    });
                })
                .UseStartup<Startup>()
                .UseNLog();
        }
    }
}
