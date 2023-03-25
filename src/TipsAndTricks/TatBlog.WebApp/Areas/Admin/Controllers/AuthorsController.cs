using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;
using TatBlog.WebApp.Validations;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly IMediaManager _mediaManager;
        private readonly IValidator<AuthorEditModel> _authorValidator;
        public AuthorsController(IAuthorRepository authorRepository,
            IMapper mapper,
            IMediaManager mediaManager)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            _mediaManager = mediaManager;
            _authorValidator = new AuthorValidator(_authorRepository);
        }

        [HttpGet]
        public async Task<IActionResult> Index(AuthorFilterModel model,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 10)
        {
            var authorQuery = _mapper.Map<AuthorQuery>(model);
            ViewBag.AuthorsList = await _authorRepository.GetPagedAuthorsAsync(authorQuery, pageNumber: pageNumber, pageSize: pageSize);
            ViewBag.AuthorQuery = authorQuery;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id = 0)
        {
            var author = id > 0
                ? await _authorRepository.GetAuthorByIdAsync(id)
                : null;
            var model = author == null
                ? new AuthorEditModel()
                : _mapper.Map<AuthorEditModel>(author);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(
            int id,
            [FromQuery(Name = "p")] int pageNumber,
            [FromQuery(Name = "ps")] int pageSize)
        {
            await _authorRepository.DeleteAuthorByIdAsync(id);
            return RedirectToAction(nameof(Index), new { p = pageNumber, ps = pageSize });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AuthorEditModel model)
        {
            var validatorResult = await this._authorValidator.ValidateAsync(model);
            if (!validatorResult.IsValid)
            {
                validatorResult.AddToModelState(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var author = model.Id > 0
                ? await _authorRepository.GetAuthorByIdAsync(model.Id) : null;
            if (author == null)
            {
                author = _mapper.Map<Author>(model);
                author.Id = 0;
                author.JoinedDate = DateTime.Now;
            }
            else
            {
                _mapper.Map(model, author);
            }
            if (model.ImageFile?.Length > 0)
            {
                var newImagePath = await _mediaManager.SaveFileAsync(
                    model.ImageFile.OpenReadStream(),
                    model.ImageFile.FileName,
                    model.ImageFile.ContentType);
                if (!string.IsNullOrWhiteSpace(newImagePath))
                {
                    await _mediaManager.DeleteFileAsync(author.ImageUrl);
                    author.ImageUrl = newImagePath;
                }
            }
            await _authorRepository.AddOrUpdateAuthorAsync(author);
            return RedirectToAction(nameof(Index));
        }
    }
}
