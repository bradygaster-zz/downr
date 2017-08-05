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
using Microsoft.Extensions.Options;

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
            services.AddOptions();

            services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/svg+xml", "application/font-woff2" });
            });

            // Add framework services.
            services.AddMvc();

            // add site services
            services.AddSingleton<IMarkdownContentLoader, DefaultMarkdownContentLoader>();
            services.AddSingleton<IYamlIndexer, DefaultYamlIndexer>();

            services.Configure<DownrOptions>(Configuration.GetSection("downr"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IYamlIndexer yamlIndexer,
            IOptions<DownrOptions> downrOptions)
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

            var stem = downrOptions.Value.Stem.StripLeading("/");
            if (!string.IsNullOrEmpty(stem) && !stem.EndsWith("/"))
            {
                stem += "/";
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "blog-root",
                    template: stem,
                    defaults: new { controller = "Posts", Action = "Index" }
                );
                routes.MapRoute(
                    name: "blog-post",
                    template: stem + "posts/{slug}",
                    defaults: new { controller = "Posts", Action = "Post" }
                );
                routes.MapRoute(
                    name: "blog-categories",
                    template: stem + "category/{name}",
                    defaults: new { controller = "Category", Action = "Index" }
                );
                routes.MapRoute(
                    name: "blog-feed-rss",
                    template: stem + "feed/rss",
                    defaults: new { controller = "Feed", Action = "Rss" }
                );
            });

            var logger = loggerFactory.CreateLogger<Startup>();
            logger.LogInformation("Options.Title: '{0}'", downrOptions.Value.Title);
            logger.LogInformation("Options.RooUrl: '{0}'", downrOptions.Value.RootUrl);
            logger.LogInformation("Options.Stem: '{0}' ({1})", downrOptions.Value.Stem, stem);

            // get the path to the content directory so the yaml headers can be indexed as metadata
            if (string.IsNullOrWhiteSpace(env.WebRootPath))
            {
                env.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                logger.LogInformation("Set WebRootPath to '{0}'", env.WebRootPath);
            }
            var contentPath = Path.Combine(env.WebRootPath, "posts");
            yamlIndexer.IndexContentFiles(contentPath);
        }
    }
}
