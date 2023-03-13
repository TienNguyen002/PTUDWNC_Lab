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
        private readonly IAuthorRepository _authorRepository;
        public BlogController(IBlogRepository blogRepository, IAuthorRepository authorRepository)
        {
            _blogRepository = blogRepository;
            _authorRepository = authorRepository;
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
            IPagingParams pagingParams = CreatePagingParamsForPost(pageNumber, pageSize);
            //Truy vấn các bài viết theo điều kiện đã tạo
            var postsList = await _blogRepository.GetPagesPostQueryAsync(postQuery, pagingParams);
            //Lưu lại điều kiện truy vấn để hiển thị trong View
            ViewBag.PostQuery = postQuery;
            return View(postsList);
        }
        public async Task<IActionResult> Category(
            string slug,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 2)
        {
            var category = await _blogRepository.GetCategoryBySlugAsync(slug);
            var postQuery = new PostQuery()
            {
                PublishedOnly = true,
                CategorySlug = slug,
                CategoryName = category.Name,
            };
            IPagingParams pagingParams = CreatePagingParamsForPost(pageNumber, pageSize);
            var postsList = await _blogRepository.GetPagesPostQueryAsync(postQuery, pagingParams);
            ViewBag.PostQuery = postQuery;
            ViewBag.Title = $"Các bài viết có chủ đề '{postQuery.CategoryName}'";
            return View("Index", postsList);
        }
        public async Task<IActionResult> Author(
            string slug,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 5)
        {
            var author = await _authorRepository.GetAuthorBySlugAsync(slug);
            var postQuery = new PostQuery()
            {
                PublishedOnly = true,
                AuthorSlug = slug,
                AuthorName = author?.FullName,
            };
            IPagingParams pagingParams = CreatePagingParamsForPost(pageNumber, pageSize);
            var postsList = await _blogRepository.GetPagesPostQueryAsync(postQuery, pagingParams);
            ViewBag.PostQuery = postQuery;
            ViewBag.Title = $"Các bài viết có tác giả '{postQuery.AuthorName}'";
            return View("Index", postsList);
        }
        public async Task<IActionResult> Tag(
            string slug,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 5)
        {
            var tag = await _blogRepository.GetTagBySlugAsync(slug);
            var postQuery = new PostQuery()
            {
                PublishedOnly = true,
                TagSlug = slug,
                TagName = tag.Name,
            };
            IPagingParams pagingParams = CreatePagingParamsForPost(pageNumber, pageSize);
            var postsList = await _blogRepository.GetPagesPostQueryAsync(postQuery, pagingParams);
            ViewBag.PostQuery = postQuery;
            ViewBag.Title = $"Các bài viết có tag '{postQuery.TagName}'";
            return View("Index", postsList);
        }
        public async Task<IActionResult> Post(
            int year,
            int month,
            int day,
            string slug)
        {
            var posts = await _blogRepository.GetPostAsync(year, month, slug);
            if(posts == null)
            {
                ViewBag.Title = $"Không có bài viết nào có '{slug}'";
            }
            if(!posts.Published)
            {
                ViewBag.Title = $"Bài viết có '{slug}' chưa được công bố";
            }
            await _blogRepository.IncreaseViewCountAsync(posts.Id);
            return View("PostInfo", posts);
        }

        public async Task<IActionResult> Archive(
            [FromQuery(Name = "month")] int month,
            [FromQuery(Name = "year")] int year,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 10)
        {
            var postQuery = new PostQuery()
            {
                PostMonth = month,
                PostYear = year
            };
            IPagingParams pagingParams = CreatePagingParamsForPost(pageNumber, pageSize);
            var posts = await _blogRepository.GetPagesPostQueryAsync(postQuery, pagingParams);
            ViewBag.PostQuery = postQuery;
            ViewBag.Title = $"Các bài viết của tháng {postQuery.PostMonth} và của năm {postQuery.PostYear}";
            return View("Index", posts);
        }

        public IActionResult About() => View();
        public IActionResult Contact() => View();
        public IActionResult Rss() => Content("Nội dung sẽ được cập nhật");

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
