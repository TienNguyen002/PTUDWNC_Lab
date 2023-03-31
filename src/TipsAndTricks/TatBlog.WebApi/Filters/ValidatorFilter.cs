using FluentValidation;
using System.Net;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Filters
{
    public class ValidatorFilter<T> : IEndpointFilter where T : class
    {

        private readonly IValidator<T> _validator;
        public ValidatorFilter(IValidator<T> validator)
        {
            _validator = validator;
        }

        public async ValueTask<object> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            var model = context.Arguments.SingleOrDefault(x => x?.GetType() == typeof(T)) as T;
            if (model == null)
            {
                return Results.Ok(
                    ApiResponse.FailWithResult(
                        HttpStatusCode.BadRequest,
                        new ValidationFailureResponse(new[]
                        {
                            "Could not create model object"
                        })));
            }
            var validationResult = await _validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return Results.Ok(
                    ApiResponse.FailWithResult(
                        HttpStatusCode.BadRequest,
                        validationResult.Errors.ToResponse()));
            }
            return await next(context);
        }
    }
}

