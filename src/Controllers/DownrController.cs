using System.Collections.Generic;
using System.Linq;
using downr.Services;
using downr.Models;
using downr.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace downr.Controllers
{
    public class DownrController : BaseController
    {
        private readonly IYamlIndexer _indexer;
        private readonly DownrOptions _options;
        private readonly IFeedBuilder _feedBuilder;
        private readonly IStringLocalizer<Texts> _sharedLocalizer;

        public DownrController(IYamlIndexer indexer, IFeedBuilder feedBuilder, IOptions<DownrOptions> optionsAccessor, IStringLocalizer<Texts> sharedLocalizer)
          : base(indexer)
        {
            _indexer = indexer;
            _sharedLocalizer = sharedLocalizer;
            _options = optionsAccessor.Value;
            _feedBuilder = feedBuilder;
        }

        public IActionResult Rss()
        {
            IEnumerable<KeyValuePair<string, Metadata>> posts = _indexer.PostsMetadata.Take(_options.PostCountFeed);

            string feed = _feedBuilder.BuildXmlFeed(posts.Select(x => x.Value));
            return Content(feed, "text/xml");
        }

        public IActionResult Posts(string slug)
        {
            return RedirectToAction("Post", new
            {
                slug = _indexer.PostsMetadata.ElementAt(0).Value.Slug // todo: will fail if empty
            });
        }

        public IActionResult Page(string slug)
        {
            if (_indexer.TryGetPage(slug, out Metadata metadata))
            {
                ViewBag.Title = metadata.Title;
                return View("Page", new[] { metadata });
            }

            return RedirectToAction("Posts");
        }

        public IActionResult Post(string slug)
        {
            if (_indexer.TryGetPost(slug, out Metadata metadata))
            {
                ViewBag.Title = metadata.Title;
                return View("Post", new[] { metadata });
            }

            return RedirectToAction("Posts"); // todo: could be a endless redirection if empty
        }

        [Route("category/{name}")]
        public IActionResult Category(string name)
        {
            // get all the posts in this category
            if (!string.IsNullOrEmpty(name))
            {
                ViewData["Title"] = string.Format(_sharedLocalizer["TitleCategoryPageWithName"], name);
                ViewBag.Category = name;

                var titlesInCategory = new Dictionary<string, string>();
                var postsForView = new List<Metadata>();
                var entriesOfCategory = _indexer.PostsMetadata.Where(x => x.Value.Categories.Contains(name));
                foreach (var entry in entriesOfCategory)
                {
                    var key = entry.Key;
                    var metadata = entry.Value;

                    postsForView.Add(metadata);
                    titlesInCategory.Add(metadata.Slug, metadata.Title);
                }

                ViewBag.TitlesInCategory = titlesInCategory;
                return View("Post", postsForView.ToArray());
            }

            return View();
        }
    }
}
