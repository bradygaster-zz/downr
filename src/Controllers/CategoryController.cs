using System.Collections.Generic;
using System.Linq;
using downr.Models;
using downr.Services;
using Microsoft.AspNetCore.Mvc;

namespace downr.Controllers
{
    public class CategoryController : BaseController
    {
        public CategoryController(IYamlIndexer indexer) : base(indexer) { }

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
                var postsInCategory = _indexer.Metadata.Where(x => x.Categories.Contains(name));
                foreach (var post in postsInCategory)
                {
                    postsForView.Add(post);
                    titlesInCategory.Add(post.Slug, post.Title);
                }

                ViewBag.TitlesInCategory = titlesInCategory;
                return View("Post", postsForView.ToArray());
            }

            return View();
        }
    }
}