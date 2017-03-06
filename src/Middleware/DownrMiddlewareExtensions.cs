using System.Collections.Generic;
using downr.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using System.IO;
using downr.Services;
using Microsoft.Extensions.DependencyInjection;

namespace downr.Middleware
{
    public static class DownrMiddlewareExtensions
    {
        public static void AddDownr(this IServiceCollection services)
        {
            services.AddSingleton<IContentLoader, DefaultMarkdownContentLoader>();
            services.AddSingleton<IPageIndexer, InMemoryMarkdownPageIndexer>();
            services.AddSingleton<IPostIndexer, InMemoryMarkdownPostIndexer>();
            services.AddSingleton<ITagCloudBuilder, InMemoryTagCloudBuilder>();
            services.AddTransient<IFeedBuilder, FeedBuilder>();
        }

        public static void RegisterMetadataRoutes(this IRouteBuilder routes, IContentIndexer contentIndexer, string controller, string action)
        {
            foreach (KeyValuePair<string, Metadata> page in contentIndexer.Metadata)
            {
                routes.MapRoute(page.Key, page.Key, new { controller, action, slug = page.Key });
            }
        }

        public static void UseDownr(this IApplicationBuilder app, IHostingEnvironment env, IRouteBuilder routes,
            DownrOptions downrOptions,
            IPageIndexer pageIndexer,
            IPostIndexer postIndexer)
        {
            // Build Indexer
            pageIndexer.Index(Path.Combine(env.WebRootPath, "pages"));
            postIndexer.Index(Path.Combine(env.WebRootPath, "posts"));

            // Downr 
            routes.MapRoute("downrFeed", downrOptions.FeedSlug, new { controller = "Downr", action = "Rss" });
            routes.MapRoute("downrCategories", "category/{name}", new { controller = "Downr", action = "Category" });

            // Register controllers of metadata
            routes.RegisterMetadataRoutes(postIndexer, "Downr", "Post");
            routes.RegisterMetadataRoutes(pageIndexer, "Downr", "Page");
        }
    }
}
