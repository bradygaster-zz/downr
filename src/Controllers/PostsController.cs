using System.Collections.Generic;
using System.Linq;
using downr.Models;
using downr.Services;
using Microsoft.AspNetCore.Mvc;

namespace downr.Controllers
{
    public class PostsController : BaseController
    {
        IYamlIndexer _indexer;

        public PostsController(IYamlIndexer indexer)
          : base(indexer)
        {
            _indexer = indexer;
        }

        [Route("posts/{slug?}")]
        public IActionResult Index(string slug)
        {
            Metadata metadata;

            if (string.IsNullOrEmpty(slug))
            {
                return RedirectToAction("Index", "Posts", new
                {
                    slug = _indexer.PostsMetadata.ElementAt(0).Value.Slug
                });
            }
            else if (_indexer.TryGetPost(slug, out metadata))
            {
                ViewData["Title"] = metadata.Title;

                // where are we in the list of posts?

                int index = _indexer.PostsMetadata.Select(x => x.Value).ToList().FindIndex(x => x.Slug == slug);
                if (index != 0)
                {
                    ViewBag.Next = _indexer.PostsMetadata.ElementAt(index - 1).Value.Slug;
                    ViewBag.NextTitle = _indexer.PostsMetadata.ElementAt(index - 1).Value.Title;
                }
                // first post?
                if (index != _indexer.PostsCount - 1)
                {
                    ViewBag.Previous = _indexer.PostsMetadata.ElementAt(index + 1).Value.Slug;
                    ViewBag.PreviousTitle = _indexer.PostsMetadata.ElementAt(index + 1).Value.Title;
                }

                return View("Post", new Metadata[] { metadata });
            }
            else
            {
                return RedirectToAction("Index", "Posts", new
                {
                    slug = _indexer.PostsMetadata.ElementAt(0).Value.Slug
                });
            }
        }
    }
}
