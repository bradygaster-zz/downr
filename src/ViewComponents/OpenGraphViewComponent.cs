using System.Threading.Tasks;
using downr.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace downr.ViewComponents
{
    public class OpenGraphViewComponent : ViewComponent
    {
        private readonly DownrOptions _downrOptions;

        public OpenGraphViewComponent(IOptions<DownrOptions> options)
        {
            _downrOptions = options.Value;
        }

        public async Task<IViewComponentResult> InvokeAsync(Metadata metadata = null)
        {
            OpenGraphViewComponentModel model;

            // defaults
            if (metadata == null)
            {
                model = new OpenGraphViewComponentModel
                {
                    Title = _downrOptions.Title,
                    Description = _downrOptions.Description,
                };
            }
            // post or page with metadata
            else
            {
                model = new OpenGraphViewComponentModel
                {
                    Title = metadata.Title,
                    Description = metadata.Description,
                    Image = metadata.Image
                };
            }

            return View("Default", model);
        }
    }
}
