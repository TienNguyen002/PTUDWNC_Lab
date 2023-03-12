using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
    public class TagCloudWidget : ViewComponent
    {
        private readonly IBlogRepository _blogRepository;
        public TagCloudWidget(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tags = await _blogRepository.GetAllTagsAsync();
            return View(tags);
        }
    }
}
