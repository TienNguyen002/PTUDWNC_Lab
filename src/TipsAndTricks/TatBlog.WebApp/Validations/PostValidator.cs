using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TatBlog.Services.Blogs;
using System.Globalization;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations;

public class PostValidator : AbstractValidator<PostEditModel>
{
    private readonly IBlogRepository _blogRepository;

    public PostValidator(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;

        RuleFor(s => s.Title)
            .NotEmpty()
            .WithMessage("Chủ đề không được bỏ trống")
            .MaximumLength(500)
            .WithMessage("Chủ đề chỉ được tối đa 500 ký tự");

        RuleFor(s => s.ShortDescription)
            .NotEmpty()
            .WithMessage("Giới thiệu không được bỏ trống");

        RuleFor(s => s.Description)
            .NotEmpty()
            .WithMessage("Nội dung không được bỏ trống");

        RuleFor(s => s.Meta)
            .NotEmpty()
            .WithMessage("Metadata không được bỏ trống")
            .MaximumLength(1000)
            .WithMessage("Metadata chỉ được tối đa 1000 ký tự");

        RuleFor(s => s.UrlSlug)
            .NotEmpty()
            .WithMessage("Slug không được bỏ trống")
            .MaximumLength(1000)
            .WithMessage("Slug chỉ được tối đa 1000 ký tự");

        RuleFor(s => s.UrlSlug)
            .MustAsync(async (postModel, slug, cancellationToken) =>
                !await _blogRepository.IsPostSlugExistedAsync(postModel.Id, slug, cancellationToken))
            .WithMessage("Slug '{PropertyValue}' đã được sử dụng");

        RuleFor(s => s.CategoryId)
            .NotEmpty()
            .WithMessage("Bạn phải chọn chủ đề cho bài viết");

        RuleFor(s => s.AuthorId)
            .NotEmpty()
            .WithMessage("Bạn phải chọn tác giả bài viết");

        RuleFor(s => s.SelectedTags)
            .Must(HasAtLeastOneTag)
            .WithMessage("Bạn phải nhập ít nhất một thẻ");

        When(s => s.Id <= 0, () =>
        {
            RuleFor(s => s.ImageFile)
                .Must(s => s is { Length: > 0 })
                .WithMessage("Bạn phải chọn hình ảnh cho bài viết");
        })
            .Otherwise(() =>
            {
                RuleFor(s => s.ImageFile)
                    .MustAsync(SetImageIfNotExist)
                    .WithMessage("Bạn phải chọn hình ảnh cho bài viết");
            });
    }

    private bool HasAtLeastOneTag(PostEditModel postModel, string selectedTags)
    {
        return postModel.GetSelectedTags().Any();
    }

    private async Task<bool> SetImageIfNotExist(
        PostEditModel postModel,
        IFormFile imageFile,
        CancellationToken cancellationToken)
    {
        var post = await _blogRepository.GetPostByIdAsync(postModel.Id, false, cancellationToken);
        if (!string.IsNullOrWhiteSpace(post?.ImageUrl))
        {
            return true;
        }

        return imageFile is { Length: > 0 };
    }
}