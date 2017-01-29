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
            return Redirect("home");
        }
    }
}