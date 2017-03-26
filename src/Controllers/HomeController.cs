using downr.Services;
using Microsoft.AspNetCore.Mvc;

namespace downr.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(ITagCloudBuilder tagCloudBuilder) : base(tagCloudBuilder)
        {
        }

        // [Route("")]
        // public IActionResult Index()
        // {
        //     return RedirectToAction("Page", "Downr", new { slug = "" });
        // }

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