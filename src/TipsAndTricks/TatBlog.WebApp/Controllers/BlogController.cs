using Microsoft.AspNetCore.Mvc;
using TatBlog.Core;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        //Action này xử lý HTTP request đến trang chủ của
        //ứng dụng web hoặc tìm kiếm bài viết theo từ khóa
        [HttpGet]
        public async Task<IActionResult> Index(
            [FromQuery(Name = "k")] string keyword = null,
            [FromQuery(Name ="p")] int pageNumber = 1,
            [FromQuery(Name ="ps")] int pageSize =10)
        {
            //Tạo đối tượng chứa các điều kiện truy vấn
            var postQuery = new PostQuery()
            {
                //Chỉ lấy những bài viết có trạng thái Published
                PublishedOnly = true,
                KeyWord = keyword,
            };
            IPagingParams pagingParams = new PagingParams()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortColumn = "PostedDate",
                SortOrder = "DESC"
            };
            //Truy vấn các bài viết theo điều kiện đã tạo
            var postsList = await _blogRepository.GetPagesPostQueryAsync(postQuery, pagingParams);
            //Lưu lại điều kiện truy vấn để hiển thị trong View
            ViewBag.PostQuery = postQuery;
            return View(postsList);
        }
        public IActionResult About() => View();
        public IActionResult Contact() => View();
        public IActionResult Rss() => Content("Nội dung sẽ được cập nhật");
    }
}
