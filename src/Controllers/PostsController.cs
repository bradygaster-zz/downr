using System.Collections.Generic;
using System.Linq;
using downr;
using downr.Models;
using downr.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace downr.Controllers
{
    public class PostsController : BaseController
    {
        private readonly DownrOptions _options;

        public PostsController(
            IYamlIndexer indexer,
            IOptions<DownrOptions> options
            )
            : base(indexer)
        {
            _options = options.Value;
        }

        public IActionResult Index(int? page = null)
        {
            switch (_options.HomePageStyle)
            {
                case HomePageStyle.SummaryList:
                    return Index_SummaryList(page);

                case HomePageStyle.LatestPost:
                default: // make this the default - beats throwing an exception etc ;-)
                    return Index_LatestPost();
            }
        }

        [NonAction]
        private IActionResult Index_SummaryList(int? page = null)
        {
            var pageSize = _options.PageSize;
            page = page ?? 1;
            var pageIndex = page.Value - 1;
            var posts = _indexer.Metadata
                                .Skip(pageIndex * pageSize)
                                .Take(pageSize)
                                .ToArray();

            var model = new PostListModel
            {
                Posts = posts,
                NextPageLink = (page > 1) ? Url.Action("Index", new { page = page - 1 }) : null,
                PreviousPageLink = (_indexer.Metadata.Count > (pageIndex + 1) * pageSize) ? Url.Action("Index", new { page = page + 1 }) : null
            };
            return View("PostList", model);
        }

        [NonAction]
        private IActionResult Index_LatestPost()
        {
            return RedirectToAction("Post", "Posts", new
            {
                slug = _indexer.Metadata.ElementAt(0).Slug
            });
        }

        public IActionResult Post(string slug)
        {
            // make sure the post is found in the index
            var meta = _indexer.Metadata.FirstOrDefault(x => x.Slug == slug);
            if (meta == null)
            {
                // not found - send to homepage
                return RedirectToAction("Index", "Posts");
            }
            ViewData["Title"] = meta.Title;

            // where are we in the list of posts?
            // last post?
            var model = new PostModel
            {
                Post = meta
            };
            int index = _indexer.Metadata.FindIndex(x => x.Slug == slug);
            if (index != 0)
            {
                var next = _indexer.Metadata[index - 1];
                model.NextPostSlug = next.Slug;
                model.NextPostTitle = next.Title;
            }
            // first post?
            if (index != _indexer.Metadata.Count - 1)
            {
                var previous = _indexer.Metadata[index + 1];
                model.PreviousPostSlug = previous.Slug;
                model.PreviousPostTitle = previous.Title;
            }

            return View("Post", model);
        }
    }
}
