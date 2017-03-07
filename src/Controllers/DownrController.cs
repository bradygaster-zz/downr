using System.Collections.Generic;
using System.Linq;
using downr.Services;
using downr.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace downr.Controllers
{
    public class DownrController : BaseController
    {
        private readonly DownrOptions _options;
        private readonly IPostIndexer _postIndexer;
        private readonly IPageIndexer _pageIndexer;
        private readonly IFeedBuilder _feedBuilder;

        public DownrController(IPostIndexer postIndexer, IPageIndexer pageIndexer, ITagCloudBuilder tagCloudBuilder, IFeedBuilder feedBuilder, IOptions<DownrOptions> optionsAccessor)
          : base(tagCloudBuilder)
        {
            _options = optionsAccessor.Value;
            _postIndexer = postIndexer;
            _pageIndexer = pageIndexer;
            _feedBuilder = feedBuilder;
        }

        public IActionResult Rss()
        {
            IEnumerable<KeyValuePair<string, Metadata>> posts = _postIndexer.Metadata.Take(_options.PostCountFeed);

            string feed = _feedBuilder.BuildXmlFeed(posts.Select(x => x.Value));
            return Content(feed, "text/xml");
        }

        public IActionResult Posts()
        {
            return RedirectToAction("Post", new
            {
                slug = _postIndexer.Metadata.ElementAt(0).Value.Slug // todo: will fail if empty
            });
        }

        public IActionResult Page(string slug)
        {
            if (_pageIndexer.TryGet(slug, out Metadata metadata))
            {
                ViewBag.Title = metadata.Title;
                return View("Page", metadata);
            }

            return RedirectToAction("Posts");
        }

        public IActionResult Post(string slug)
        {
            if (_postIndexer.TryGet(slug, out Metadata metadata))
            {
                ViewBag.Title = metadata.Title;
                return View("Post", metadata);
            }

            return RedirectToAction("Posts"); // todo: could be a endless redirection if empty
        }

        [Route("category/{name}")]
        public IActionResult Category(string name)
        {
            var postsForView = new List<Metadata>();

            // get all the posts in this category
            if (!string.IsNullOrEmpty(name))
            {
                ViewData["Title"] = name;
                ViewBag.Category = name;

                var titlesInCategory = new Dictionary<string, string>();

                var entriesOfCategory = _postIndexer.Metadata.Where(x => x.Value.Categories.Contains(name));
                foreach (var entry in entriesOfCategory)
                {
                    var key = entry.Key;
                    var metadata = entry.Value;

                    postsForView.Add(metadata);
                    titlesInCategory.Add(metadata.Slug, metadata.Title);
                }

                ViewBag.TitlesInCategory = titlesInCategory;
            }

            return View("Categories", postsForView.ToArray());
        }
    }
}
