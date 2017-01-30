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

        public IViewComponentResult Invoke(Metadata metadata = null)
        {
            MetadataViewComponentModel model;

            // defaults
            if (metadata == null)
            {
                model = new MetadataViewComponentModel
                {
                    Author = _downrOptions.Author,
                    Description = _downrOptions.Description,
                    Keywords = _downrOptions.Keywords
                };
            }
            // post or page with metadata
            else
            {
                model = new MetadataViewComponentModel
                {
                    Author = metadata.Author,
                    Description = metadata.Description,
                    Keywords = _downrOptions.Keywords
                };
            }

            return View("Default", model);
        }
    }
}
