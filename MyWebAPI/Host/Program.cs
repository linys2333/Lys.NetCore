using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net;

namespace Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
                // 配置启动url
               .AddJsonFile("hosting.json")
               .AddCommandLine(args)
               .Build();
            
            var host = new WebHostBuilder()
                .UseKestrel(options =>
                {
                    // 配置https证书
                    options.Listen(IPAddress.Any, 44325, listenOptions =>
                    {
                        listenOptions.UseHttps("Cert/testCert.pfx", "1234");
                    });
                })
                .UseConfiguration(config)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
