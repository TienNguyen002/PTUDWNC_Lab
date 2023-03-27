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
    public class CategoriesController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CategoryEditModel> _categoryValidator;
        public CategoriesController(
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
            _categoryValidator = new CategoryValidator(_blogRepository);
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            CategoryFilterModel model,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 10)
        {
            var categoryQuery = _mapper.Map<CategoryQuery>(model);
            ViewBag.Items = await _blogRepository.GetPagedCategoriesAsync(categoryQuery: categoryQuery, pageNumber: pageNumber, pageSize: pageSize);
            ViewBag.CategoryQuery = categoryQuery;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ChangeShowOnMenu(
            int id,
            [FromQuery(Name = "filter")] string filter,
            [FromQuery(Name = "p")] int pageNumber,
            [FromQuery(Name = "ps")] int pageSize)
        {
            await _blogRepository.ChangeShowOnMenuCategoryAsync(id);
            return Redirect($"{Url.ActionLink("Index", "Categories", new { p = pageNumber, ps = pageSize })}&{filter}");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(
            int id,
            [FromQuery(Name = "filter")] string filter,
            [FromQuery(Name = "p")] int pageNumber,
            [FromQuery(Name = "ps")] int pageSize)
        {
            await _blogRepository.DeleteCategoryByIdAsync(id);
            return Redirect($"{Url.ActionLink("Index", "Categories", new { p = pageNumber, ps = pageSize })}&{filter}");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id = 0)
        {
            var category = id > 0
                ? await _blogRepository.GetCategoryByIdAsync(id)
                : null;

            var model = category == null
                ? new CategoryEditModel()
                : _mapper.Map<CategoryEditModel>(category);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryEditModel model)
        {
            var validatorResult = await this._categoryValidator.ValidateAsync(model);
            if (!validatorResult.IsValid)
            {
                validatorResult.AddToModelState(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var category = model.Id > 0
                ? await _blogRepository.GetCategoryByIdAsync(model.Id) : null;
            if (category == null)
            {
                category = _mapper.Map<Category>(model);
                category.Id = 0;
            }
            else
            {
                _mapper.Map(model, category);
            }
            await _blogRepository.AddOrUpdateCategoryAsync(category);
            return RedirectToAction(nameof(Index));
        }
    }
}
