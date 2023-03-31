using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class CategoryEndpoints
    {
        public static WebApplication MapCategoryEndpoints(
            this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/categories");

            routeGroupBuilder.MapGet("/", GetCategories)
              .WithName("GetCategories")
              .Produces<ApiResponse<PaginationResult<CategoryItem>>>();


            routeGroupBuilder.MapGet("/{id:int}", GetCategoryDetails)
              .WithName("GetCategoryDetails")
              .Produces<ApiResponse<CategoryItem>>();


            routeGroupBuilder.MapGet("/{id:int}/postsList", GetPostsByCategoryId)
              .WithName("GetPostsByCategoryId")
              .Produces<ApiResponse<PaginationResult<PostDto>>>();


            routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9_-]+$)}/posts", GetPostsByCategorySlug)
              .WithName("GetPostsByCategorySlug")
              .Produces<ApiResponse<PaginationResult<PostDto>>>();


            routeGroupBuilder.MapPost("/", AddCategory)
              .WithName("AddCategory")
              .AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
              .Produces<ApiResponse<AuthorItem>>();

            routeGroupBuilder.MapPut("/{id:int}", UpdateCategory)
              .WithName("UpdateCategory")
              .AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
              .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeleteCategory)
              .WithName("DeleteCategory")
              .Produces<ApiResponse<string>>();

            return app;
        }

        private static async Task<IResult> GetCategories(
          [AsParameters] CategoryFilterModel model,
          IBlogRepository blogRepository)
        {
            var categories = await blogRepository.GetPagedCategoriesAsync(model, model.Name);
            var paginationResult = new PaginationResult<CategoryItem>(categories);
            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetCategoryDetails(
          int id,
          IBlogRepository blogRepository,
          IMapper mapper)
        {
            var category = await blogRepository.GetCachedCategoryByIdAsync(id);

            return category == null
              ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy chủ đề có mã số {id}"))
              : Results.Ok(ApiResponse.Success(mapper.Map<CategoryItem>(category)));
        }

        private static async Task<IResult> GetPostsByCategoryId(
          int id,
          [AsParameters] PagingModel pagingModel,
          IBlogRepository blogRepository,
          ILogger<IResult> logger)
        {
            var postQuery = new PostQuery()
            {
                CategoryId = id,
                PublishedOnly = true,
            };
            var posts = await blogRepository.GetPagedPostsAsync(
              postQuery, pagingModel,
              posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(posts);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetPostsByCategorySlug(
            [FromRoute] string slug,
            [AsParameters] PagingModel pagingModel,
            IBlogRepository blogRepository,
            ILogger<IResult> logger)
        {
            var postQuery = new PostQuery()
            {
                CategorySlug = slug,
                PublishedOnly = true,
            };
            var posts = await blogRepository.GetPagedPostsAsync(
              postQuery, pagingModel,
              posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(posts);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> AddCategory(
            CategoryEditModel model,
            IBlogRepository blogRepository,
            IMapper mapper,
            ILogger<IResult> logger)
        {
            if (await blogRepository.IsCategoryExistBySlugAsync(0, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, 
                    $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }
            var category = mapper.Map<Category>(model);
            await blogRepository.AddOrUpdateCategoryAsync(category);

            return Results.Ok(ApiResponse.Success(
                mapper.Map<CategoryItem>(category), HttpStatusCode.Created));
        }

        private static async Task<IResult> UpdateCategory(
          int id, CategoryEditModel model,
          IBlogRepository blogRepository,
          IMapper mapper)
        {            
            if (await blogRepository.IsCategoryExistBySlugAsync(0, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
                    $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var category = mapper.Map<Category>(model);
            category.Id = id;

            return await blogRepository.AddOrUpdateCategoryAsync(category)
              ? Results.Ok(ApiResponse.Success("Cập nhật chủ đề thành công", HttpStatusCode.NoContent))
              : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy chủ đề có id = {id}"));
        }

        private static async Task<IResult> DeleteCategory(
          int id,
          IBlogRepository blogRepository)
        {
            return await blogRepository.DeleteCategoryByIdAsync(id)
              ? Results.Ok(ApiResponse.Success("Xóa chủ đề thành công", HttpStatusCode.NoContent))
              : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy chủ đề có id = {id}"));
        }
    }
}
