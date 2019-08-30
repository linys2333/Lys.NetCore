using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json")
                .AddCommandLine(args)
                .Build();

            return WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(true)
                .UseConfiguration(config)
                .UseKestrel()
                .UseStartup<Startup>();
        }
    }
}
