using System;
using System.Linq;
using downr.Models;
using downr.Services;
using Microsoft.AspNetCore.Mvc;

namespace downr.Controllers
{
    public class PagesController : Controller
    {
        IPagesIndexer _indexer;

        public PagesController(IPagesIndexer indexer)
        {
             _indexer = indexer;
         }

        [Route("{id?}")]
        public IActionResult Index(string id = "home")
        {
               Metadata metadata;
               if (_indexer.TryGet(id, out metadata))
               {
                   ViewBag.Title = metadata.Title;
                   return View("Page", new Metadata[] { metadata });
              }

            return RedirectToAction("Index", "Posts");
        }
    }
}
