using FluentValidation;
using TatBlog.WebApi.Models.Tag;

namespace TatBlog.WebApi.Validations
{
    public class TagValidator : AbstractValidator<TagEditModel>
    {
        public TagValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty()
                .WithMessage("Tên thẻ không được để trống")
                .MaximumLength(100)
                .WithMessage("Tên thẻ tối đa 100 ký tự");

            RuleFor(a => a.UrlSlug)
                .NotEmpty()
                .WithMessage("UrlSlug không được để trống")
                .MaximumLength(100)
                .WithMessage("UrlSlug tối đa 100 ký tự");

            RuleFor(a => a.Description)
                .NotEmpty()
                .WithMessage("Mô tả không được để trống")
                .MaximumLength(500)
                .WithMessage("Tên thẻ tối đa 500 ký tự");
        }
    }
}
