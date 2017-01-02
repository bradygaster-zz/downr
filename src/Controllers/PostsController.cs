using System;
using System.Linq;
using downr.Services;
using Microsoft.AspNetCore.Mvc;

namespace downr.Controllers
{
    public class PostsController : Controller
    {
        IMarkdownContentLoader _markdownLoader;
        IYamlIndexer _indexer;

        public PostsController(IMarkdownContentLoader markdownLoader,
            IYamlIndexer indexer)
        {
            _markdownLoader = markdownLoader;
            _indexer = indexer;
        }

        [Route("posts/{slug}")]
        [Route("{slug}")]
        public IActionResult Index(string slug)
        {
            // make sure the post is found in the index
            if (_indexer.Metadata.Any(x => x.Slug == slug))
            {
                ViewBag.HtmlContent = _markdownLoader.GetContentToRender(slug);
                var meta = _indexer.Metadata.First(x => x.Slug == slug);
            }
            else
            {
                return NotFound();
            }

            return View();
        }
    }
}
