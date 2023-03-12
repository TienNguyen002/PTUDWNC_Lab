using Microsoft.AspNetCore.Mvc;
using TatBlog.Core;
using TatBlog.Core.Contracts;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
    public class BestAuthorsWidget : ViewComponent
    {
        private readonly IAuthorRepository _authorRepository;
        public BestAuthorsWidget(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            IPagingParams pagingParams = new PagingParams()
            {
                PageNumber = 1,
                PageSize = 4,
            };
            var bestAuthor = _authorRepository.GetNPopularAuthors(4, pagingParams);
            return View();
        }
    }
}
