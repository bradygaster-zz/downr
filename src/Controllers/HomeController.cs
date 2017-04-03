using System.Linq;
using downr.Services;
using Microsoft.AspNetCore.Mvc;

namespace downr.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IPostIndexer _postIndexer;
        
        public HomeController(ITagCloudBuilder tagCloudBuilder,
            IPostIndexer postIndexer) : base(tagCloudBuilder)
        {
            _postIndexer = postIndexer;
        }

        [Route("")]
        public IActionResult Index()
        {
            var slug = _postIndexer.Metadata.ElementAt(0).Value.Slug;
            return Redirect(slug);
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