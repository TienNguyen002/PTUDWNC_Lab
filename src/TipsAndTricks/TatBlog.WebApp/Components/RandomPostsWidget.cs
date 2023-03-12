using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
    public class RandomPostsWidget : ViewComponent
    {
        IBlogRepository _blogRepository;
        public RandomPostsWidget(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            var random = await _blogRepository
              .GetNRandomPostsAsync(5);

            return View(random);
        }
    }
}
