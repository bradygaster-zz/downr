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
            //Go to a slug if provided otherwise go to latest.
            var slug = _indexer.Metadata.FirstOrDefault(x => x.Slug == id)?.Slug;
            return RedirectToAction("Index", "Posts", new
                {
                    slug = slug ?? _indexer.Metadata.ElementAt(0).Slug
                });
        }
    }
}
