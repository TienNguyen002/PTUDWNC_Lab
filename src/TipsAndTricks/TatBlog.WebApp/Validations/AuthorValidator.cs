using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TatBlog.Services.Blogs;
using System.Globalization;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations;

public class AuthorValidator : AbstractValidator<AuthorEditModel>
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorValidator(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;

        RuleFor(s => s.FullName)
            .NotEmpty()
            .WithMessage("Tên tác giả không được bỏ trống")
            .MaximumLength(500)
            .WithMessage("Tên tác giả chỉ được tối đa 500 ký tự");

        RuleFor(s => s.Email)
            .NotEmpty()
            .WithMessage("Email không được bỏ trống")
            .EmailAddress();

        RuleFor(s => s.UrlSlug)
            .NotEmpty()
            .WithMessage("Slug không được bỏ trống")
            .MaximumLength(1000)
            .WithMessage("Slug chỉ được tối đa 1000 ký tự");

        RuleFor(s => s.UrlSlug)
            .MustAsync(async (authorModel, slug, cancellationToken) =>
                !await _authorRepository.CheckExistAuthorSlugByIdAsync(authorModel.Id, slug, cancellationToken))
            .WithMessage("Slug '{PropertyValue}' đã được sử dụng");

        When(s => s.Id <= 0, () =>
        {
            RuleFor(s => s.ImageFile)
                .Must(s => s is { Length: > 0 })
                .WithMessage("Bạn phải chọn hình ảnh cho tác giả");
        })
            .Otherwise(() =>
            {
                RuleFor(s => s.ImageFile)
                    .MustAsync(SetImageIfNotExist)
                    .WithMessage("Bạn phải chọn hình ảnh cho tác giả");
            });
    }

    private async Task<bool> SetImageIfNotExist(
        AuthorEditModel authorModel,
        IFormFile imageFile,
        CancellationToken cancellationToken)
    {
        var post = await _authorRepository.GetAuthorByIdAsync(authorModel.Id, cancellationToken);
        if (!string.IsNullOrWhiteSpace(post?.ImageUrl))
        {
            return true;
        }

        return imageFile is { Length: > 0 };
    }
}