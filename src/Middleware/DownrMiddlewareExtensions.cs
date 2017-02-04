using System.Collections.Generic;
using downr.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using System.IO;

namespace downr.Middleware
{
    public static class DownrMiddlewareExtensions
    {
        public static void UseDownr(this IApplicationBuilder app, IHostingEnvironment env, IRouteBuilder routes, DownrOptions downrOptions, IYamlIndexer yamlIndexer)
        {
            // Build Indexer
            yamlIndexer
                .IndexPageFiles(Path.Combine(env.WebRootPath, "pages"))
                .IndexPostFiles(Path.Combine(env.WebRootPath, "posts"))
                .Build();


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

            foreach (KeyValuePair<string, Metadata> post in yamlIndexer.PostsMetadata)
            {
                routes.MapRoute(
                    name: post.Key,
                    template: post.Key,
                    defaults: new { controller = "Downr", action = "Post", slug = post.Key });
            }

            foreach (KeyValuePair<string, Metadata> page in yamlIndexer.PagesMetadata)
            {
                routes.MapRoute(
                    name: page.Key,
                    template: page.Key,
                    defaults: new { controller = "Downr", action = "Page", slug = page.Key });
            }
        }
    }
}
