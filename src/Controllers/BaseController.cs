using System;
using System.Collections.Generic;
using System.Linq;
using downr.Models;
using downr.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using downr.Models.TagCloud;

namespace downr.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IYamlIndexer _indexer;

        public BaseController(IYamlIndexer indexer)
        {
            _indexer = indexer;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
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

            ViewBag.TagCloud = new TagCloudModel{
                Tags = tagCloud.OrderBy(x => x.Key)
                                .Select(x=> new Tag{ Name = x.Key, Count = x.Value })
                                .ToArray()
            };
        }
    }
}