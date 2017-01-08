using System.Collections.Generic;
using System.Linq;
using downr.Models;
using downr.Services;
using Microsoft.AspNetCore.Mvc;

namespace downr.Controllers
{
    public class PostsController : BaseController
    {
        IPostsIndexer _indexer;

        public PostsController(IPostsIndexer indexer)
          : base(indexer)
        {
            _indexer = indexer;
        }

        [Route("posts/{slug}")]
        public IActionResult Index(string slug)
        {
            Metadata metadata;
            if (_indexer.TryGet(slug, out metadata))
            {
                ViewData["Title"] = metadata.Title;

                // where are we in the list of posts?

                int index = _indexer.Metadata.Select(x => x.Value).ToList().FindIndex(x => x.Slug == slug);
                if (index != 0)
                {
                    ViewBag.Next = _indexer.Metadata.ElementAt(index - 1).Value.Slug;
                    ViewBag.NextTitle = _indexer.Metadata.ElementAt(index - 1).Value.Title;
                }
                // first post?
                if (index != _indexer.Count - 1)
                {
                    ViewBag.Previous = _indexer.Metadata.ElementAt(index + 1).Value.Slug;
                    ViewBag.PreviousTitle = _indexer.Metadata.ElementAt(index + 1).Value.Title;
                }

                return View("Post", new Metadata[] { metadata });
            }
            else
            {
                return RedirectToAction("Index", "Posts", new
                {
                    slug = _indexer.Metadata.ElementAt(0).Value.Slug
                });
            }
        }
    }
}
