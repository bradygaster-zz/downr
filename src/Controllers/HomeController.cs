using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace downr.Controllers
{
    public class HomeController : Controller
    {
        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("posts/{slug}")]
        public IActionResult posts(string slug)
        {
            ViewData.Add("Slug", slug);
            return View();
        }
    }
}
