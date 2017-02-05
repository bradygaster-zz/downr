using Microsoft.AspNetCore.Mvc;

namespace downr.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IYamlIndexer indexer) : base(indexer)
        {
        }

        public IActionResult Index()
        {
            return RedirectToAction("Page", "Downr", new {slug = "home"});
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}