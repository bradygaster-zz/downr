using System.Collections.Generic;
using downr.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using System.IO;
using downr.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Http;

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

        public static void UseDownr(this IApplicationBuilder app, IHostingEnvironment env, IRouteBuilder routes,
            DownrOptions downrOptions,
            IPageIndexer pageIndexer,
            IPostIndexer postIndexer)
        {
            // Build Indexer
            pageIndexer.Index(Path.Combine(env.WebRootPath, "pages"));
            postIndexer.Index(Path.Combine(env.WebRootPath, "posts"), "/posts");

            // Downr 
            routes.MapRoute("downrFeed", downrOptions.FeedSlug, new { controller = "Downr", action = "Rss" });

            var rewriteOptions = new RewriteOptions()
                .Add(ctx =>
                {
                    var request = ctx.HttpContext.Request;
                    var requestPath = request.Path;
                    if (pageIndexer.TryGet(requestPath, out Metadata metadata))
                    {
                        // TODO - look this up in routing table!
                        request.Path = "/Downr/Page";
                        request.QueryString = QueryString.Create("slug", requestPath);
                        ctx.Result = RuleResult.SkipRemainingRules;
                        return;
                    }
                    if (postIndexer.TryGet(requestPath, out metadata))
                    {
                        // TODO - look this up in routing table!
                        request.Path = "/Downr/Post";
                        request.QueryString = QueryString.Create("slug", requestPath);
                        ctx.Result = RuleResult.SkipRemainingRules;
                        return;
                    }
                });
            app.UseRewriter(rewriteOptions);
        }
    }
}
