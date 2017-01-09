using downr.Models;
using Microsoft.AspNetCore.Mvc;

namespace downr.Controllers
{
    public class PagesController : BaseController
    {
        IYamlIndexer _indexer;

        public PagesController(IYamlIndexer indexer) : base(indexer)
        {
            _indexer = indexer;
        }

        // I will make this great again => middleware
        [Route("{id?}/{sub0?}/{sub1?}/{sub2?}/{sub3?}/{sub4?}/{sub5?}/{sub6?}/{sub7?}/{sub8?}/{sub9?}")]
        public IActionResult Index(string id = "home",

        string sub0 = null,
        string sub1 = null,
        string sub2 = null,
        string sub3 = null,
        string sub4 = null,
        string sub5 = null,
        string sub6 = null,
        string sub7 = null,
        string sub8 = null,
        string sub9 = null)
        {
            // I will make this great again => middleware
            var slug = $"{id}/{sub0}/{sub1}/{sub2}/{sub3}/{sub4}/{sub5}/{sub6}/{sub7}/{sub8}/{sub9}".TrimEnd('/');

            Metadata metadata;
            if (_indexer.TryGetPage(slug, out metadata))
            {
                ViewBag.Title = metadata.Title;
                return View("Page", new Metadata[] { metadata });
            }

            // maybe it was post?
            if (_indexer.TryGetPost(slug, out metadata))
            {
                return RedirectToAction("Index", "Posts", new
                {
                    slug = slug
                });
            }

            return RedirectToAction("Index", "Posts");
        }
    }
}
