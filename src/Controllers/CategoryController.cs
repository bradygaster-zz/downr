using System.Collections.Generic;
using System.Linq;
using downr.Models;
using downr.Services;
using Microsoft.AspNetCore.Mvc;

namespace downr.Controllers
{
    public class CategoryController : BaseController
    {
        IPostsIndexer _indexer;

        public CategoryController(IPostsIndexer indexer) : base(indexer)
        {
            _indexer = indexer;
        }

        [Route("category/{name}")]
        public IActionResult Index(string name)
        {
            // get all the posts in this category
            if (!string.IsNullOrEmpty(name))
            {
                ViewData["Title"] = "Posts in " + name;
                ViewBag.Category = name;

                var titlesInCategory = new Dictionary<string, string>();
                var postsForView = new List<Metadata>();
                var entriesOfCategory = _indexer.Metadata.Where(x => x.Value.Categories.Contains(name));
                foreach (var entry in entriesOfCategory)
                {
                    var key = entry.Key;
                    var metadata = entry.Value;

                    postsForView.Add(metadata);
                    titlesInCategory.Add(metadata.Slug, metadata.Title);
                }

                ViewBag.TitlesInCategory = titlesInCategory;
                return View("Post", postsForView.ToArray());
            }

            return View();
        }
    }
}