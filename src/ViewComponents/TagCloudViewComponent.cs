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

        public async Task<IViewComponentResult> InvokeAsync()
        {
             // get all the categories
            var tagCloud = new Dictionary<string, int>();
            _indexer.Metadata.Select(x => x.Categories).ToList().ForEach(categories =>
            {
                categories?.ToList().ForEach(category =>
                {
                    if (!tagCloud.ContainsKey(category))
                        tagCloud.Add(category, 0);
                    tagCloud[category] += 1;
                });
            });

            var model = new TagCloudModel{
                Tags = tagCloud.OrderBy(x => x.Key)
                                .Select(x=> new Tag{ Name = x.Key, Count = x.Value })
                                .ToArray()
            };
            return View(model);
        }
    }
}