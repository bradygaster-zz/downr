using System;
using System.Linq;
using downr.Services;
using Microsoft.AspNetCore.Mvc;

namespace downr.Controllers
{
    public class HomeController : Controller
    {
        IYamlIndexer _indexer;

        public HomeController(IYamlIndexer indexer)
        {
            _indexer = indexer;
        }

        [Route("{id?}")]
        public IActionResult Index(string id)
        {
            // if no slug was provided show the last one
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index", "Posts", new
                {
                    slug = _indexer.Metadata.ElementAt(0).Slug
                });

            // if the slug was provided AND found, redirect to it
            if (_indexer.Metadata.Any(x => x.Slug == id))
                return RedirectToAction("Index", "Posts", new
                {
                    slug = _indexer.Metadata.ElementAt(0).Slug
                });
            else // no match was found, show the last one
                return RedirectToAction("Index", "Posts", new
                {
                    slug = _indexer.Metadata.ElementAt(0).Slug
                });
        }
    }
}
