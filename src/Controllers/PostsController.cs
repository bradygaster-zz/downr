using System;
using System.Collections.Generic;
using System.Linq;
using downr;
using downr.Models;
using downr.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace downr.Controllers
{
    public class PostsController : Controller
    {
        private readonly DownrOptions _options;
        private readonly PostService _postService;

        public PostsController(
            PostService postService,
            IOptions<DownrOptions> options
            )
        {
            _postService = postService;
            _options = options.Value;
        }

        public IActionResult Index(int page = 1)
        {
            switch (_options.HomePageStyle)
            {
                case HomePageStyle.SummaryList:
                    return Index_SummaryList(page);

                case HomePageStyle.LatestPost:
                default: // make this the default - beats throwing an exception etc ;-)
                    return Index_LatestPost();
            }
        }

        [NonAction]
        private IActionResult Index_SummaryList(int page, string category = null)
        {
            PostListModel model = GetPostList(page);
            return View("PostList", model);
        }

        [NonAction]
        private IActionResult Index_LatestPost()
        {
            return RedirectToAction("Post", "Posts", new
            {
                slug = _postService.GetLatestPost().Slug
            });
        }

        public IActionResult Post(string slug)
        {
            // make sure the post is found in the index
            var meta = _postService.GetPostBySlug(slug);
            if (meta == null)
            {
                // not found - send to homepage
                return RedirectToAction("Index", "Posts");
            }
            ViewData["Title"] = meta.Title;

            // where are we in the list of posts?
            // last post?
            var model = new PostModel
            {
                Post = meta
            };

            var prevNext = _postService.GetPreviousAndNextPosts(slug);
            if (prevNext.next != null)
            {
                model.NextPostSlug = prevNext.next.Slug;
                model.NextPostTitle = prevNext.next.Title;
            }
            if (prevNext.previous != null)
            {
                model.PreviousPostSlug = prevNext.previous.Slug;
                model.PreviousPostTitle = prevNext.previous.Title;
            }

            return View("Post", model);
        }

        public IActionResult Category(string name, int page = 1)
        {
            var model = new CategoryPostListModel
            {
                CategoryName = name,
                PostList = GetPostList(page, category: name),
            };
            return View("CategoryPostList", model);
        }

        private PostListModel GetPostList(int page, string category = null)
        {
            var pageSize = _options.PageSize;
            var pageIndex = page - 1;
            var posts = _postService.GetPosts(pageIndex * pageSize, pageSize, category);
            var postCount = _postService.GetNumberOfPosts(category);

            var pagingFunction = (category == null) ? (Func<int, string, string>)GetPagedIndexLink : GetPagedCategoryLink;
            var model = new PostListModel
            {
                Posts = posts,
                NextPageLink = (page > 1) ? pagingFunction(page - 1, category) : null,
                PreviousPageLink = (postCount > (pageIndex + 1) * pageSize) ? pagingFunction(page + 1, category) : null
            };
            return model;
        }

        private string GetPagedIndexLink(int page, string category)
        {
            if (page > 1)
                return Url.Action("Index", new { page });
            if (page == 1)
                return Url.Action("Index"); // page defaults to 1 so keep URL clean :-)

            throw new ArgumentException("page must be greater than or equal to one");
        }
        private string GetPagedCategoryLink(int page, string category)
        {
            if (page > 1)
                return Url.Action("Category", new { page, nameof = category });
            if (page == 1)
                return Url.Action("Category", new { name = category }); // page defaults to 1 so keep URL clean :-)

            throw new ArgumentException("page must be greater than or equal to one");
        }
    }
}
