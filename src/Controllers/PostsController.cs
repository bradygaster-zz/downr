using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using downr.Services;
using Microsoft.AspNetCore.Mvc;

namespace downr.Controllers
{
    public class PostsController : Controller
    {
        IMarkdownContentLoader _markdownLoader;

        public PostsController(IMarkdownContentLoader markdownLoader)
        {
            _markdownLoader = markdownLoader;
        }

        [Route("posts/{slug}")]
        public IActionResult Index(string slug)
        {
            ViewData.Add("Slug", slug);
            return View();
        }
    }
}
