using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebAdvert.SearchApi.Extensions;
using WebAdvert.SearchApi.Services;
using WebAdvert.SearchApi.HealthChecks;

namespace WebAdvert.SearchApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddElasticSearch(Configuration);
            services.AddTransient<ISearchService, SearchService>();
            services.AddControllers();
            services.AddHealthChecks().AddCheck<SearchHealthCheck>("SearchService", timeout: TimeSpan.FromMinutes(1));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            loggerFactory.AddAWSProvider(
                Configuration.GetAWSLoggingConfigSection(),
                (loglevel, message, exception) => $"[{DateTime.Now} {loglevel} {message} {exception?.Message} {exception?.StackTrace}]");

            app.UseHealthChecks("/search/health");

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
