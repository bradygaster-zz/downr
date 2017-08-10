using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using downr.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using downr.Models.TagCloud;

namespace downr.ViewComponents
{
    public class TagCloudViewComponent : ViewComponent
    {
        protected IYamlIndexer _indexer;

        public TagCloudViewComponent(IYamlIndexer indexer)
        {
            _indexer = indexer;
        }

        public ViewViewComponentResult Invoke()
        {
            var tags = _indexer.Metadata
                .SelectMany(p => p.Categories) // flatten post categories
                .GroupBy(c => c)
                .Select(g => new Tag { Name = g.Key, Count = g.Count() })
                .OrderBy(t=>t.Name)
                .ToArray();

            var model = new TagCloudModel
            {
                Tags = tags
            };
            return View(model);
        }
    }
}