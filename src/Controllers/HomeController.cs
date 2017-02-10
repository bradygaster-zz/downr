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
            return RedirectToAction("Page", "Downr", new { slug = "home" });
        }

        [Route("error/{0}")]
        public IActionResult Error(int errorCode)
        {
            ViewBag.ErrorCode = errorCode;

            return View();
        }

        [Route("error/404")]
        public IActionResult PageNotFound()
        {
            return View();
        }

        [Route("error/500")]
        public IActionResult InternalError()
        {
            return View();
        }
    }
}