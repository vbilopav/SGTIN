using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SgtinAppCore.Interfaces;
using SgtinAppCore.Model;
using SgtinAppCore.Services;
using SgtinInfrastructure;
using Swashbuckle.AspNetCore.Swagger;

namespace SgtinApi
{
    public class AppConfig
    {
        public string SwaggerTitle { get; set; } = "SGTIN API";
        public string SwaggerVersion { get; set; } = "v1";
        public string SwaggerEndpoint { get; set; } = $"/swagger/v1/swagger.json";
        public string DiagnosticPath { get; set; } = "/diagnostic";
    }

    public class Startup
    {
        private IServiceCollection services;
        private IConfiguration config;
        private AppConfig appConfig;

        public Startup(IConfiguration config)
        {
            this.config = config;
            appConfig = new AppConfig();
            config.Bind("app", appConfig);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(appConfig.SwaggerVersion,
                    new Info
                    {
                        Title = appConfig.SwaggerTitle,
                        Version = appConfig.SwaggerVersion
                    }
                );
            });
     
            services.AddSingleton(typeof(IRepository<SgtinData>), typeof(SgtinDataRepository));
            services.AddTransient(typeof(IHexToBinConvertService), typeof(HexToBinConvertService));
            services.AddTransient(typeof(IBinToIntConvertService), typeof(BinToIntConvertService));
            services.AddTransient(typeof(ISgtinFactoryService), typeof(SgtinFactoryService));
            services.AddTransient(typeof(ISearchService), typeof(SearchService));
            this.services = services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(
                    appConfig.SwaggerEndpoint,
                    $"{appConfig.SwaggerTitle} {appConfig.SwaggerVersion}"
                );
            });
            app.UseDiagnosticPage(env, services, config, appConfig.DiagnosticPath,
                Tuple.Create("AppConfig", appConfig as object));
            app.UseMvc();
            app.UseDeveloperExceptionPage();            
        }
    }

    public static class DiagnosticPageExtensions
    {
        public static IApplicationBuilder UseDiagnosticPage(
            this IApplicationBuilder app,
            IHostingEnvironment envirment,
            IServiceCollection services,
            IConfiguration config,
            string path = "/diagnostic",
            Tuple<string, object> extraInfo = null)
        {
            app.Map(path, builder => builder.Run(async context =>
            {
                context.Response.ContentType = "application/json";

                var result = new Dictionary<string, object>
                {
                    { extraInfo?.Item1 ?? "extraInfo is not provided", extraInfo?.Item2 ?? "-" },
                    { "HostingEnvironment", envirment },
                    { "Actions", app.ApplicationServices
                        .GetRequiredService<IActionDescriptorCollectionProvider>()
                        .ActionDescriptors
                        .Items
                        .Select(item => new { name = item.DisplayName, item.AttributeRouteInfo.Template }) },
                    { "Culture", new { CultureInfo.CurrentCulture, CultureInfo.CurrentUICulture  } },
                    { "Services", services.
                        Where(item => !item.ServiceType.FullName.StartsWith("Microsoft") && !item.ServiceType.FullName.StartsWith("System")).
                        Select(item => new
                        {
                            lifetime = item.Lifetime.ToString(),
                            type = item.ServiceType.FullName,
                            implementation = item.ImplementationType?.FullName
                        })
                    },
                    { "Configuration", config.AsEnumerable() }
                };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(result, Formatting.Indented));
            }));
            return app;
        }
    }
}
