using downr.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace downr.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly ITagCloudBuilder _tagCloudBuilder;

        protected BaseController(ITagCloudBuilder tagCloudBuilder)
        {
            _tagCloudBuilder = tagCloudBuilder;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.TagCloud = _tagCloudBuilder.GetTagCloud();            
        }
    }
}