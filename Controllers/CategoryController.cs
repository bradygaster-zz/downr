using System;
using System.Collections.Generic;
using System.Linq;
using downr.Models;
using downr.Services;
using Microsoft.AspNetCore.Mvc;

namespace downr.Controllers
{
    public class CategoryController : Controller
    {
        IMarkdownContentLoader _markdownLoader;
        IYamlIndexer _indexer;

        public CategoryController(IMarkdownContentLoader markdownLoader,
            IYamlIndexer indexer)
        {
            _markdownLoader = markdownLoader;
            _indexer = indexer;
        }

        [Route("category/{name}")]
        public IActionResult Index(string name)
        {
            // get all the categories
            var tagCloud = new Dictionary<string, int>();
            _indexer.Metadata.Select(x => x.Categories).ToList().ForEach(categories =>
            {
                categories.ToList().ForEach(category =>
                {
                    if (!tagCloud.ContainsKey(category))
                        tagCloud.Add(category, 0);
                    tagCloud[category] += 1;
                });
            });

            ViewBag.TagCloud = tagCloud.OrderBy(x => x.Key); ;

            // get all the posts in this category
            if (!string.IsNullOrEmpty(name))
            {
                ViewData["Title"] = "Posts in " + name;
                ViewBag.Category = name;

                var titlesInCategory = new Dictionary<string,string>();
                var postsForView = new List<Metadata>();
                var postsInCategory = _indexer.Metadata.Where(x => x.Categories.Contains(name));
                foreach (var post in postsInCategory)
                {
                    post.Content = _markdownLoader.GetContentToRender(post.Slug);
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