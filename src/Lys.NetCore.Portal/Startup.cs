using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using IdentityServer4.AccessTokenValidation;
using Lys.NetCore.Domain.CallRecords.Models;
using Lys.NetCore.EF;
using Lys.NetCore.Infrastructure.Extensions;
using Lys.NetCore.Infrastructure.FFmpeg;
using Lys.NetCore.Infrastructure.Services;
using Lys.NetCore.Infrastructure.Settings;
using Lys.NetCore.Infrastructure.Web;
using Lys.NetCore.Portal.DTOs;
using Lys.NetCore.Portal.HttpHandlers;
using Lys.NetCore.Portal.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Examples;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace Lys.NetCore.Portal
{
    public class Startup
    {
        private readonly ILoggerFactory m_LoggerFactory;
        private readonly IConfiguration m_Configuration;

        private AuthSetting m_AuthSetting;
        private FileServiceSetting m_FileServiceSetting;

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            m_LoggerFactory = loggerFactory;
            m_Configuration = configuration;

            ConfigureOthers();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
                {
                    // 使用https
                    options.Filters.Add(new RequireHttpsAttribute());
                    options.Filters.Add(new WebApiExceptionFilter());
                }).AddJsonOptions(options =>
                {
                    options.UseCamelCasing(true);
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified;
                    options.SerializerSettings.DateFormatString = MyConstants.DateTimeFormatter.HyphenLongDateTime;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = actionContext =>
                    {
                        var errorMessage = string.Join(",", actionContext.ModelState.Values
                            .SelectMany(ms => ms.Errors
                                .Where(e => !string.IsNullOrEmpty(e.ErrorMessage) || e.Exception != null)
                                .Select(e => string.IsNullOrEmpty(e.ErrorMessage) ? e.Exception.Message : e.ErrorMessage)));

                        var error = new ErrorDescriber { Code = HttpStatusCode.BadRequest.ToString(), Description = errorMessage };
                        return new JsonResult(WebApiResponse.Fail(error));
                    };
                });

            m_LoggerFactory.AddConsole();
            services.AddSingleton(m_LoggerFactory.CreateLogger("Lys.NetCore.Root"));

            services.AddResponseCompression();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDbContextPool<MyDbContext>(opt => opt.UseSqlServer(m_Configuration.GetConnectionString("Default")));

            services.AddMemoryCache();
            
            var apiScope = m_Configuration["IdentityServer:ApiScope"];
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "MyWebAPI"
                });

                options.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "application",
                    TokenUrl = m_Configuration["IdentityServer:TokenUrl"],
                    Scopes = new Dictionary<string, string>
                    {
                        // 供Swagger验证界面选择
                        { apiScope, "可访问的API" }
                    }
                });

                options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {"oauth2", new[] {apiScope}}
                });

                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Lys.NetCore.Portal.xml"));

                options.OperationFilter<ExamplesOperationFilter>();
                
                options.IgnoreObsoleteActions();
                options.IgnoreObsoleteProperties();
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(apiScope, policyAdmin => { policyAdmin.RequireClaim("scope", apiScope); });
            });

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                })
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = m_Configuration["IdentityServer:Authority"];
                    options.ApiName = m_Configuration["IdentityServer:ApiName"];
                    options.ApiSecret = m_Configuration["IdentityServer:ApiSecret"];
                    options.RequireHttpsMetadata = false;
                });

            ConfigureInfrastructureProject(services);
            ConfigureServiceProject(services);
            ConfigureAutoMapper(services);

            return ConfigureServiceProvider(services);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<APISpendMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // http重定向到https
            var options = new RewriteOptions().AddRedirectToHttps(301);
            app.UseRewriter(options);

            app.UseStaticFiles();
            app.UseResponseCompression();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DefaultModelsExpandDepth(-1);
                c.SwaggerEndpoint("./v1/swagger.json", "MyWebAPI");
                c.InjectStylesheet("./custom.css");
            });

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        
        private void ConfigureInfrastructureProject(IServiceCollection services)
        {
            services.AddSingleton(m_AuthSetting);
            services.AddSingleton(m_FileServiceSetting);

            services.AddFFmpeg();
        }

        private void ConfigureServiceProject(IServiceCollection services)
        {
            services.AddHttpClient("FileClient", c => { c.BaseAddress = new Uri(m_FileServiceSetting.BaseUrl); })
                .ConfigurePrimaryHttpMessageHandler(() => new HttpsClientHandler(m_FileServiceSetting.BaseUrl))
                .AddHttpMessageHandler(() => new LogAPIDelegatingHandler("FileClientAPI"))
                .AddHttpMessageHandler(() => new AuthorizationDelegatingHandler(m_FileServiceSetting.Key, m_FileServiceSetting.Secret));
        }

        private IServiceProvider ConfigureServiceProvider(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);

            RegisterInterfaceServices(builder);

            IoCExtensions.Container = builder.Build();
            return new AutofacServiceProvider(IoCExtensions.Container);
        }

        private void RegisterInterfaceServices(ContainerBuilder builder)
        {
            var assemblies = "Domain".Split(',')
                .Select(s => Assembly.Load($"Lys.NetCore.{s}"))
                .ToArray();
            
            builder.RegisterAssemblyServices<ServiceBase>(assemblies);
        }

        private void ConfigureOthers()
        {
            m_AuthSetting = m_Configuration.GetSection("IdentityServer").Get<AuthSetting>();
            m_FileServiceSetting = m_Configuration.GetSection("FileService").Get<FileServiceSetting>();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Unspecified,
                DateFormatString = MyConstants.DateTimeFormatter.HyphenLongDateTime
            };
        }

        private void ConfigureAutoMapper(IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CallRecordRequest, CallRecord>();
            });

            services.AddSingleton(config.CreateMapper());
        }
    }
}
