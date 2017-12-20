using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Common;
using Common.Configuration;
using Common.FFmpeg;
using Common.Interfaces;
using Domain.CallRecord;
using EF;
using Host.Controllers.CallRecord;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Host
{
    public static class MyInitializer
    {
        public static IServiceProvider Initialize(IServiceCollection services, IConfiguration config)
        {
            services.AddDbContextPool<MyDbContext>(opt => opt.UseSqlServer(config.GetConnectionString("Default")));

            services.Configure<AuthConfig>(config.GetSection("IdentityServer"));
            services.Configure<FileServiceConfig>(config.GetSection("FileService"));
            
            ConfigureFFmpeg(services);

            ConfigureAutoMapper();
            
            return ConfigureServiceProvider(services);
        }

        private static void ConfigureFFmpeg(IServiceCollection services)
        {
            IFFmpeg ffmpeg;
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    ffmpeg = new LinuxFFmpeg();
                    break;
                case PlatformID.Win32NT:
                    ffmpeg = new WindowsFFmpeg();
                    break;
                default:
                    return;
            }
            services.AddSingleton(ffmpeg);
        }

        private static void ConfigureAutoMapper()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<CallRecordDto, CallRecord>();
            });
        }

        private static IServiceProvider ConfigureServiceProvider(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);

            var assemblies = GetAssemblies();
            if (assemblies != null)
            {
                builder.RegisterAssemblyTypes(assemblies)
                    .AsImplementedInterfaces()
                    .AsSelf();
            }

            return new AutofacServiceProvider(builder.Build());
        }

        private static Assembly[] GetAssemblies()
        {
            return "Domain,EF".Split(',')
                .Select(s => Assembly.Load(s))
                .ToArray();
        }
    }
}
