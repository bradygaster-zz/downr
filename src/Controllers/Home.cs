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

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Posts", new
            {
                slug = _indexer.Metadata.ElementAt(0).Slug
            });
        }
    }
}
