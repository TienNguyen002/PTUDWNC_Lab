using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TatBlog.Services.Blogs;
using System.Globalization;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations;

public class TagValidator : AbstractValidator<TagEditModel>
{
    private readonly IBlogRepository _blogRepository;

    public TagValidator(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;

        RuleFor(s => s.Name)
            .NotEmpty()
            .WithMessage("Tên chủ đề không được bỏ trống")
            .MaximumLength(200)
            .WithMessage("Tên chủ đề chỉ được tối đa 200 ký tự");

        RuleFor(s => s.Description)
            .NotEmpty()
            .WithMessage("Nội dung không được bỏ trống");

        RuleFor(s => s.UrlSlug)
            .NotEmpty()
            .WithMessage("Slug không được bỏ trống")
            .MaximumLength(1000)
            .WithMessage("Slug chỉ được tối đa 1000 ký tự");

        RuleFor(s => s.UrlSlug)
            .MustAsync(async (categoryModel, slug, cancellationToken) =>
                !await _blogRepository.CheckExistTagSlugByIdAsync(categoryModel.Id, slug, cancellationToken))
            .WithMessage("Slug '{PropertyValue}' đã được sử dụng");
    }
}