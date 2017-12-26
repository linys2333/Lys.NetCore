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
               .AddJsonFile("hosting.json")
               .AddCommandLine(args)
               .Build();
            
            var host = new WebHostBuilder()
                .UseKestrel(options =>
                {
//                    options.Listen(IPAddress.Loopback, 5001, listenOptions =>
//                    {
//                        listenOptions.UseHttps("testCert.pfx", "testPassword");
//                    });
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
