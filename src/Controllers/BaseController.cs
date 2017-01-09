using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace downr.Controllers
{
    public abstract class BaseController : Controller
    {
        IYamlIndexer _indexer;

        public BaseController(IYamlIndexer indexer)
        {
            _indexer = indexer;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.TagCloud = _indexer.TagCloud;            
        }
    }
}