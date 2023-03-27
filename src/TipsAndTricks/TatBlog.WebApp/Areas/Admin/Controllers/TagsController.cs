using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;
using TatBlog.WebApp.Validations;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class TagsController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<TagEditModel> _tagValidator;
        public TagsController(
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
            _tagValidator = new TagValidator(_blogRepository);
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            TagFilterModel model,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 10)
        {
            var tagQuery = _mapper.Map<TagQuery>(model);
            ViewBag.Items = await _blogRepository.GetPagedTagsAsync(tagQuery: tagQuery, pageNumber: pageNumber, pageSize: pageSize);
            ViewBag.TagQuery = tagQuery;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(
            int id,
            [FromQuery(Name = "filter")] string filter,
            [FromQuery(Name = "p")] int pageNumber,
            [FromQuery(Name = "ps")] int pageSize)
        {
            await _blogRepository.DeleteTagByIdAsync(id);
            return Redirect($"{Url.ActionLink("Index", "Tags", new { p = pageNumber, ps = pageSize })}&{filter}");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id = 0)
        {
            var tag = id > 0
                ? await _blogRepository.GetTagByIdAsync(id)    
                : null;

            var model = tag == null
                ? new TagEditModel()
                : _mapper.Map<TagEditModel>(tag);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TagEditModel model)
        {
            var validationResult = await this._tagValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var tag = model.Id > 0
              ? await _blogRepository.GetTagByIdAsync(model.Id)
              : null;
            if (tag == null)
            {
                tag = _mapper.Map<Tag>(model);
                tag.Id = 0;
            }
            else
            {
                _mapper.Map(model, tag);
            }
            await _blogRepository.AddOrUpdateTagAsync(tag);
            return RedirectToAction(nameof(Index));
        }
    }
}
