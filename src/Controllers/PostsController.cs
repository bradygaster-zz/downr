using System.Collections.Generic;
using System.Linq;
using downr.Models;
using downr.Services;
using Microsoft.AspNetCore.Mvc;

namespace downr.Controllers
{
    public class PostsController : BaseController
    {
        public PostsController(IYamlIndexer indexer) : base(indexer) { }

        [Route("posts/{slug}")]
        public IActionResult Index(string slug)
        {
            // make sure the post is found in the index
            if (_indexer.Metadata.Any(x => x.Slug == slug))
            {
                var meta = _indexer.Metadata.First(x => x.Slug == slug);
                ViewData["Title"] = meta.Title;

                // where are we in the list of posts?
                // last post?
                int index = _indexer.Metadata.FindIndex(x => x.Slug == slug);
                if (index != 0)
                {
                    ViewBag.Next = _indexer.Metadata.ElementAt(index - 1).Slug;
                    ViewBag.NextTitle = _indexer.Metadata.ElementAt(index - 1).Title;
                }
                // first post?
                if (index != _indexer.Metadata.Count - 1)
                {
                    ViewBag.Previous = _indexer.Metadata.ElementAt(index + 1).Slug;
                    ViewBag.PreviousTitle = _indexer.Metadata.ElementAt(index + 1).Title;
                }

                return View("Post", new Metadata[] { meta });
            }
            else
            {
                return RedirectToAction("Index", "Posts", new
                {
                    slug = _indexer.Metadata.ElementAt(0).Slug
                });
            }
        }
    }
}
