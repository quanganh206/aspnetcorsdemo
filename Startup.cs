using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;

namespace vitapidotnet
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*services.AddCors(options => {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:8100"));
            });*/
            // Add framework services.
            services.AddMvc();
            services.AddCors(options => {
                options.AddPolicy("AllowAllOrigins", builder => builder.AllowAnyOrigin());
                options.AddPolicy("Access-Control-Allow-Headers", builder => builder
                    .WithHeaders("X-Requested-With", "X-CSRF-Token", "X-XSRF-TOKEN", "Content-Type", "Authorization"));
                options.AddPolicy("Access-Control-Allow-Credentials", builder => { builder.AllowCredentials(); });
                options.AddPolicy("Access-Control-Allow-Methods", builder => { builder.WithMethods("GET", "POST", "PUT", "DELETE"); });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //app.UseCors(_ => _.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseCors(_ => _.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
