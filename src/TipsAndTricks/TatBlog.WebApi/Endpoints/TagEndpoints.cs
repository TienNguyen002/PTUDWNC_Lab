using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.DTO.Author;
using TatBlog.Core.DTO.Category;
using TatBlog.Core.DTO.Post;
using TatBlog.Core.DTO.Tag;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;
using TatBlog.WebApi.Models.Category;
using TatBlog.WebApi.Models.Post;
using TatBlog.WebApi.Models.Tag;

namespace TatBlog.WebApi.Endpoints
{
    public static class TagEndpoints
    {
        public static WebApplication MapTagEndpoints(
            this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/tags");

            routeGroupBuilder.MapGet("/", GetTags)
              .WithName("GetTags")
              .Produces<ApiResponse<PaginationResult<TagItem>>>();

            routeGroupBuilder.MapGet("/alltags", GetAllTags)
              .WithName("GetAllTags")
              .Produces<ApiResponse<PaginationResult<TagItem>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetTagDetails)
              .WithName("GetTagDetails")
              .Produces<ApiResponse<TagItem>>();

            routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9_-]+$)}/posts", GetTagBySlug)
              .WithName("GetTagBySlug")
              .Produces<ApiResponse<TagItem>>();

            routeGroupBuilder.MapPost("/", AddTag)
              .WithName("AddTag")
              .AddEndpointFilter<ValidatorFilter<TagEditModel>>()
              .Produces<ApiResponse<TagItem>>();

            routeGroupBuilder.MapPut("/{id:int}", UpdateTag)
              .WithName("UpdateTag")
              .AddEndpointFilter<ValidatorFilter<TagEditModel>>()
              .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeleteTag)
              .WithName("DeleteTag")
              .Produces<ApiResponse<string>>();

            return app;
        }

        private static async Task<IResult> GetTags(
          [AsParameters] TagFilterModel model,
          IBlogRepository blogRepository,
          IMapper mapper)
        {
            var query = mapper.Map<TagQuery>(model);
            var tags = await blogRepository.GetPagedTagsAsync(query, model,
                tags => tags.ProjectToType<TagItem>());
            var paginationResult = new PaginationResult<TagItem>(tags);
            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetAllTags(
            IBlogRepository blogRepository)
        {
            var tags = await blogRepository.GetAllTagsAsync();
            return Results.Ok(ApiResponse.Success(tags));
        }

        private static async Task<IResult> GetTagDetails(
          int id,
          IBlogRepository blogRepository,
          IMapper mapper)
        {
            var tag = await blogRepository.GetTagByIdAsync(id);

            return tag == null
              ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy thẻ có mã số {id}"))
              : Results.Ok(ApiResponse.Success(mapper.Map<TagItem>(tag)));
        }

        private static async Task<IResult> GetTagBySlug(
            [FromRoute] string slug,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            var tag = await blogRepository.GetTagBySlugAsync(slug);
            return tag == null
             ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy thẻ có slug {slug}"))
             : Results.Ok(ApiResponse.Success(mapper.Map<TagItem>(tag)));
        }

        private static async Task<IResult> AddTag(
            TagEditModel model,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            if (await blogRepository.IsTagExistBySlugAsync(0, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, 
                    $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }
            var tag = mapper.Map<Tag>(model);
            await blogRepository.AddOrUpdateTagAsync(tag);

            return Results.Ok(ApiResponse.Success(
                mapper.Map<TagItem>(tag), HttpStatusCode.Created));
        }

        private static async Task<IResult> UpdateTag(
          int id, TagEditModel model,
          IBlogRepository blogRepository,
          IMapper mapper)
        {            
            if (await blogRepository.IsTagExistBySlugAsync(0, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
                    $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var tag = mapper.Map<Tag>(model);
            tag.Id = id;

            return await blogRepository.AddOrUpdateTagAsync(tag)
              ? Results.Ok(ApiResponse.Success("Cập nhật thẻ thành công", HttpStatusCode.NoContent))
              : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy thẻ có id = {id}"));
        }

        private static async Task<IResult> DeleteTag(
          int id,
          IBlogRepository blogRepository)
        {
            return await blogRepository.DeleteTagByIdAsync(id)
              ? Results.Ok(ApiResponse.Success("Xóa thẻ thành công", HttpStatusCode.NoContent))
              : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy thẻ có id = {id}"));
        }
    }
}
