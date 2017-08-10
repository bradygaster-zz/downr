using System.Collections.Generic;
using System.Linq;
using downr.Models;
using downr.Services;
using Microsoft.AspNetCore.Mvc;

namespace downr.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IYamlIndexer _indexer;

        public CategoryController(IYamlIndexer indexer)
        {
            _indexer = indexer;
        }

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
                var model = new PostListModel
                {
                    Posts = postsForView.ToArray()
                    // TODO - add paging
                };
                return View("PostList", model);
            }

            return RedirectToRoute("blog-index");
        }
    }
}