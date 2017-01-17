using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using downr.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Net.Http.Headers;

namespace downr
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
            services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/svg+xml", "application/font-woff2" });
            });
            // Add framework services.
            services.AddMvc();

            // add site services
            services.AddSingleton<IMarkdownContentLoader, DefaultMarkdownContentLoader>();
            services.AddSingleton<IYamlIndexer, DefaultYamlIndexer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IYamlIndexer yamlIndexer)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            app.UseResponseCompression();        
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse =
                    _ => _.Context.Response.Headers[HeaderNames.CacheControl] = 
            "public,max-age=604800"  
            });

            app.UseMvc(routes =>
            {   
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

            });

            // get the path to the content directory so the yaml headers can be indexed as metadata
            var contentPath = Path.Combine(env.WebRootPath, "posts");
            yamlIndexer.IndexContentFiles(contentPath);
        }
    }
}
