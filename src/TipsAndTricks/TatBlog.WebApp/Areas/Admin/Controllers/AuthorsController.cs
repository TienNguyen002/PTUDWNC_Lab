using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IAuthorRepository _authorRepository;
        public AuthorsController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }
        public async Task<IActionResult> Index(AuthorFilterModel model)
        {
            var authorQuery = new AuthorQuery()
            {
                KeyWord = model.KeyWord,
                JoinedMonth = model.JoinedMonth,
                JoinedYear = model.JoinedYear,
            };
            //ViewBag.AuthorsList = await 
            return View();
        }
    }
}
