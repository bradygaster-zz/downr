using downr.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace downr
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("options.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            // add site services
            services.AddSingleton<IMarkdownContentLoader, DefaultMarkdownContentLoader>();
            services.AddSingleton<IYamlIndexer, DefaultYamlIndexer>();

            // downr
            // Configure using a sub-section of the appsettings.json file.
            services.Configure<DownrOptions>(Configuration.GetSection("downr"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IYamlIndexer yamlIndexer, IOptions<DownrOptions> options)
        {
            DownrOptions downrOptions = options.Value;

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

            app.UseStaticFiles();


            // get the path to the content directory so the yaml headers can be indexed as metadata
            yamlIndexer.IndexPageFiles($@"{ env.WebRootPath }\" + "pages")
                        .IndexPostFiles($@"{ env.WebRootPath }\" + "posts")
                        .Build();

            app.UseMvc(routes =>
            {
                InitDownrContent(routes, downrOptions, yamlIndexer);

                routes.MapRoute(
                   name: "default",
                   template: "{controller=Home}/{action=Index}/{id?}");

            });
        }

        public void InitDownrContent(IRouteBuilder routes, DownrOptions downrOptions, IYamlIndexer yamlIndexer)
        {
            // Downr Feed
            routes.MapRoute(
                "downrFeed",
                downrOptions.FeedSlug,
                new
                {
                    controller = "Downr",
                    action = "Rss"
                }
            );
            // Downr Categories
            routes.MapRoute(
                "downrCategories",
                "category/{name}",
                new
                {
                    controller = "Downr",
                    action = "Category"
                }
            );

            foreach (var post in yamlIndexer.PostsMetadata)
            {
                routes.MapRoute(
                    name: post.Key,
                    template: post.Key,
                    defaults: new
                      {
                          controller = "Downr",
                          action = "Post",
                          slug = post.Key
                      }
                  );
            }

            foreach (var page in yamlIndexer.PagesMetadata)
            {
                routes.MapRoute(
                    name: page.Key,
                    template: page.Key,
                    defaults: new
                    {
                          controller = "Downr",
                          action = "Page",
                          slug = page.Key
                      }
                  );
            }


        }
    }
}
