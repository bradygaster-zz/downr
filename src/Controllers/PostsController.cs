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
            if (_indexer.Metadata.Any(x => x[Strings.MetadataNames.Slug] == slug))
            {
                ViewBag.HtmlContent = _markdownLoader.GetContentToRender(slug);
            }
            else
            {
                return NotFound();
            }

            return View();
        }
    }
}
