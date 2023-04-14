using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO.Author;
using TatBlog.Core.DTO.Category;
using TatBlog.Core.DTO.Post;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;
using TatBlog.WebApi.Models.Author;
using TatBlog.WebApi.Models.Post;

namespace TatBlog.WebApi.Endpoints
{
    public static class AuthorEndpoints
    {
        public static WebApplication MapAuthorEndpoints(
            this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/authors");

            routeGroupBuilder.MapGet("/pages", GetAuthors)
              .WithName("GetAuthors")
              .Produces<ApiResponse<PaginationResult<AuthorItem>>>();

            routeGroupBuilder.MapGet("/", GetAllAuthors)
                .WithName("GetAllAuthors")
                .Produces<ApiResponse<PaginationResult<AuthorItem>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetAuthorDetails)
              .WithName("GetAuthorDetails")
              .Produces<ApiResponse<AuthorItem>>();

            routeGroupBuilder.MapGet("/{id:int}/postsList", GetPostsByAuthorId)
              .WithName("GetPostsByAuthorId")
              .Produces<ApiResponse<PaginationResult<PostDto>>>();

            routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9_-]+$)}/posts", GetPostsByAuthorSlug)
              .WithName("GetPostsByAuthorSlug")
              .Produces<ApiResponse<PaginationResult<PostDto>>>();

            routeGroupBuilder.MapPost("/", AddAuthor)
              .WithName("AddAuthor")
              .AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
              .Produces<ApiResponse<AuthorItem>>();

            routeGroupBuilder.MapPost("/{id:int}/avatar", SetAuthorPicture)
              .WithName("SetAuthorPicture")
              .Accepts<IFormFile>("multipart/form-data")
              .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapPut("/{id:int}", UpdateAuthor)
              .WithName("UpdateAuthor")
              .AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
              .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeleteAuthor)
              .WithName("DeleteAuthor")
              .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapGet("/best/{limit:int}", GetPagedNPopularAuthors)
                .WithName("GetNPopularAuthors")
                .Produces<ApiResponse<AuthorItem>>();

            return app;
        }

        private static async Task<IResult> GetAuthors(
          [AsParameters] AuthorFilterModel model,
          IAuthorRepository authorRepository)
        {
            var authors = await authorRepository.GetPagedAuthorsAsync(model, model.Name);
            var paginationResult = new PaginationResult<AuthorItem>(authors);
            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetAllAuthors(
            IAuthorRepository authorRepository)
        {
            var authors = await authorRepository.GetAuthorAsync();
            return Results.Ok(ApiResponse.Success(authors));
        }
        private static async Task<IResult> GetAuthorDetails(
          int id,
          IAuthorRepository authorRepository,
          IMapper mapper)
        {
            var author = await authorRepository.GetCachedAuthorByIdAsync(id);

            return author == null
              ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy tác giả có mã số {id}"))
              : Results.Ok(ApiResponse.Success(mapper.Map<AuthorItem>(author)));
        }

        private static async Task<IResult> GetPostsByAuthorId(
          int id,
          [AsParameters] PagingModel pagingModel,
          IBlogRepository blogRepository,
          ILogger<IResult> logger)
        {
            var postQuery = new PostQuery()
            {
                AuthorId = id,
                PublishedOnly = true,
            };
            var posts = await blogRepository.GetPagedPostsAsync(
              postQuery, pagingModel,
              posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(posts);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetPostsByAuthorSlug(
            [FromRoute] string slug,
            [AsParameters] PagingModel pagingModel,
            IBlogRepository blogRepository,
            ILogger<IResult> logger)
        {
            var postQuery = new PostQuery()
            {
                AuthorSlug = slug,
                PublishedOnly = true,
            };
            var posts = await blogRepository.GetPagedPostsAsync(
              postQuery, pagingModel,
              posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(posts);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> AddAuthor(
            AuthorEditModel model,
            IAuthorRepository authorRepository,
            IMapper mapper,
            ILogger<IResult> logger)
        {
            if (await authorRepository.IsAuthorExistBySlugAsync(0, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, 
                    $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }
            var author = mapper.Map<Author>(model);
            await authorRepository.AddOrUpdateAuthorAsync(author);

            return Results.Ok(ApiResponse.Success(
                mapper.Map<AuthorItem>(author), HttpStatusCode.Created));
        }

        private static async Task<IResult> SetAuthorPicture(
           int id,
           IFormFile imagefile,
           IAuthorRepository authorRepository,
           IMediaManager mediaManager,
           ILogger<IResult> logger)
        {
            var imageUrl = await mediaManager.SaveFileAsync(
              imagefile.OpenReadStream(),
              imagefile.FileName, imagefile.ContentType);

            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Không lưu được tập tin"));
            }
            await authorRepository.SetImageUrlAsync(id, imageUrl);
            return Results.Ok(ApiResponse.Success(imageUrl));
        }

        private static async Task<IResult> UpdateAuthor(
          int id, AuthorEditModel model,
          IAuthorRepository authorRepository,
          IMapper mapper)
        {            
            if (await authorRepository.IsAuthorExistBySlugAsync(0, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
                    $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var author = mapper.Map<Author>(model);
            author.Id = id;

            return await authorRepository.AddOrUpdateAuthorAsync(author)
              ? Results.Ok(ApiResponse.Success("Cập nhật tác giả thành công", HttpStatusCode.NoContent))
              : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy tác giả có id = {id}"));
        }

        private static async Task<IResult> DeleteAuthor(
          int id,
          IAuthorRepository authorRepository)
        {
            return await authorRepository.DeleteAuthorByIdAsync(id)
              ? Results.Ok(ApiResponse.Success("Xóa tác giả thành công", HttpStatusCode.NoContent))
              : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy tác giả có id = {id}"));
        }

        private static async Task<IResult> GetPagedNPopularAuthors(int limit,
            IAuthorRepository authorRepository)
        {
            var popularAuthors = await authorRepository.GetNPopularAuthorsAsync(limit);
            return Results.Ok(ApiResponse.Success(popularAuthors));
        }
    }
}
