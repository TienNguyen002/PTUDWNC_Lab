using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models.Post;

namespace TatBlog.WebApi.Validations
{
    public class PostValidator : AbstractValidator<PostEditModel>
    {
        public PostValidator()
        {
            RuleFor(a => a.Title)
                .NotEmpty()
                .WithMessage("Tiêu đề không được để trống")
                .MaximumLength(500)
                .WithMessage("Tiêu đề tối đa 500 ký tự");

            RuleFor(a => a.ShortDescription)
                .NotEmpty()
                .WithMessage("Giới thiệu không được để trống");

            RuleFor(a => a.Description)
                .NotEmpty()
                .WithMessage("Nội dung không được để trống");

            RuleFor(s => s.Meta)
                .NotEmpty()
                .WithMessage("Metadata không được bỏ trống")
                .MaximumLength(1000)
                .WithMessage("Metadata chỉ được tối đa 1000 ký tự");

            RuleFor(a => a.UrlSlug)
                .NotEmpty()
                .WithMessage("UrlSlug không được để trống")
                .MaximumLength(100)
                .WithMessage("UrlSlug tối đa 100 ký tự");

            RuleFor(s => s.CategoryId)
               .NotEmpty()
               .WithMessage("Bạn phải chọn chủ đề cho bài viết");

            RuleFor(s => s.AuthorId)
                .NotEmpty()
                .WithMessage("Bạn phải chọn tác giả bài viết");

            RuleFor(s => s.Tags)
                .Must(HasAtLeastOneTag)
                .WithMessage("Bạn phải nhập ít nhất một thẻ");
        }
        private bool HasAtLeastOneTag(PostEditModel postModel, string selectedTags)
        {
            return postModel.GetSelectedTags().Any();
        }
    }
}
