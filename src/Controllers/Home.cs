using System;
using System.Linq;
using downr.Services;
using Microsoft.AspNetCore.Mvc;

namespace downr.Controllers
{
    public class HomeController : Controller
    {
        IMarkdownContentLoader _markdownLoader;
        IYamlIndexer _indexer;

        public HomeController(IMarkdownContentLoader markdownLoader,
            IYamlIndexer indexer)
        {
            _markdownLoader = markdownLoader;
            _indexer = indexer;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Posts", new
            {
                slug = _indexer.Metadata.ElementAt(0).Slug
            });
        }
    }
}
