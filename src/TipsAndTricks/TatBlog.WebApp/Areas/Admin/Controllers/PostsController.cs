﻿using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing.Printing;
using TatBlog.Core;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class PostsController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        public PostsController(
            IBlogRepository blogRepository, 
            IAuthorRepository authorRepository,
            IMapper mapper)
        {
            _blogRepository = blogRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
        }
        private async Task PopulatePostFulterModelAsync(PostFilterModel model)
        {
            var authors = await _authorRepository.GetAuthorAsync();
            var categories = await _blogRepository.GetCategoriesAsync();

            model.AuthorList = authors.Select(a => new SelectListItem()
            {
                Text = a.FullName,
                Value = a.Id.ToString()
            });
            model.CategoryList = categories.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
        }
        public async Task<IActionResult> Index(PostFilterModel model)
        {
            var postQuery = _mapper.Map<PostQuery>(model);
            IPagingParams pagingParams = CreatePagingParamsForPost(1, 10);
            ViewBag.PostsList = await _blogRepository.GetPagesPostQueryAsync(postQuery, pagingParams);
            await PopulatePostFulterModelAsync(model);
            return View(model);
        }

        private IPagingParams CreatePagingParamsForPost(
            int pageNumber = 1,
            int pageSize = 5,
            string sortColumn = "PostedDate",
            string sortOrder = "DESC")
        {
            return new PagingParams()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortColumn = sortColumn,
                SortOrder = sortOrder,
            };
        }
    }
}
