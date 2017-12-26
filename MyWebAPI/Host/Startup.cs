using IdentityServer4.AccessTokenValidation;
using LysCore.Service;
using LysCore.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;

namespace Host
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddInMemoryCollection()
               .AddJsonFile("appsettings.json", true, true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            ConfigureLogger(services);

            services.AddMvc(options =>
            {
                //options.Filters.Add(new RequireHttpsAttribute());
                options.Filters.Add(new ExceptionFilter());
                options.Filters.Add(new ActionFilter());
            }).AddJsonOptions(options => 
                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver());

            services.AddOptions();

            ConfigureIdentity(services);
            ConfigureSwagger(services);

            LysService.ServiceProvider = MyInitializer.Initialize(services, Configuration);
            return LysService.ServiceProvider;
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //UseHttps(app);
            UseSwagger(app);
            app.UseStaticFiles();
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        private void ConfigureLogger(IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
        }

        private void ConfigureIdentity(IServiceCollection services)
        {
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration["IdentityServer:Authority"];
                    options.ApiName = Configuration["IdentityServer:ApiName"];
                    options.ApiSecret = Configuration["IdentityServer:ApiSecret"];
                    options.RequireHttpsMetadata = false;
                });
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
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
                    TokenUrl = Configuration["IdentityServer:TokenUrl"],
                    Scopes = new Dictionary<string, string>
                    {
                        { "api1", "api1" },
                    }
                });
 
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "Host.xml");
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
            });
        }

        private void UseHttps(IApplicationBuilder app)
        {
            var options = new RewriteOptions().AddRedirectToHttps();
            app.UseRewriter(options);
        }

        private void UseSwagger(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.ConfigureOAuth2(Configuration["IdentityServer:ClinetId"], 
                    Configuration["IdentityServer:ClinetSecret"],
                    "", 
                    Configuration["IdentityServer:ApiName"]);
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyWebAPI");
                c.InjectStylesheet("/swagger/custom.css");
            });
        }
    }
}
