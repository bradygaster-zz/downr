using downr.Services;
using Microsoft.AspNetCore.Mvc;

namespace downr.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(ITagCloudBuilder tagCloudBuilder) : base(tagCloudBuilder)
        {
        }

        public IActionResult Index()
        {
            return RedirectToAction("Page", "Downr", new {slug = "home"});
        }
    }
}