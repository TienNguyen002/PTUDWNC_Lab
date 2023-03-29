using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class AuthorEndpoints
    {
        public static WebApplication MapAuthorEndpoints(
            this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/authors");

            routeGroupBuilder.MapGet("/", GetAuthors)
              .WithName("GetAuthors")
              .Produces<PaginationResult<AuthorItem>>();


            routeGroupBuilder.MapGet("/{id:int}", GetAuthorDetails)
              .WithName("GetAuthorDetails")
              .Produces<AuthorItem>()
              .Produces(404);


            routeGroupBuilder.MapGet("/{id:int}/posts", GetPostsByAuthorId)
              .WithName("GetPostsByAuthorId")
              .Produces<PaginationResult<PostDto>>();


            routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9_-]+$)}/posts", GetPostsByAuthorSlug)
              .WithName("GetPostsByAuthorSlug")
              .Produces<PaginationResult<PostDto>>();


            routeGroupBuilder.MapPost("/", AddAuthor)
              .WithName("AddAuthor")
              .Produces(201)
              .Produces(400)
              .Produces(409);

            routeGroupBuilder.MapPost("/{id:int}/avatar", SetAuthorPicture)
              .WithName("SetAuthorPicture")
              .Accepts<IFormFile>("multipart/form-data")
              .Produces<string>()
              .Produces(400);


            routeGroupBuilder.MapPut("/{id:int}", UpdateAuthor)
              .WithName("UpdateAnAuthor")
              .Produces(204)
              .Produces(400)
              .Produces(409);

            routeGroupBuilder.MapDelete("/{id:int}", DeleteAuthor)
              .WithName("DeleteAnAuthor")
              .Produces(204)
              .Produces(404);

            return app;
        }

        private static async Task<IResult> GetAuthors(
          [AsParameters] AuthorFilterModel model,
          IAuthorRepository authorRepository,
          ILogger<IResult> logger)
        {
            var authors = await authorRepository.GetPagedAuthorsAsync(model, model.Name);
            var paginationResult = new PaginationResult<AuthorItem>(authors);
            return Results.Ok(paginationResult);
        }

        private static async Task<IResult> GetAuthorDetails(
          int id,
          IAuthorRepository authorRepository,
          IMapper mapper,
          ILogger<IResult> logger)
        {
            var author = await authorRepository.GetCachedAuthorByIdAsync(id);

            return author == null
              ? Results.NotFound($"Không tìm thấy tác giả có mã số {id}")
              : Results.Ok(mapper.Map<AuthorItem>(author));
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

            return Results.Ok(paginationResult);
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

            logger.LogInformation("Trả về kết quả");
            return Results.Ok(paginationResult);
        }

        private static async Task<IResult> AddAuthor(
            AuthorEditModel model,
            IValidator<AuthorEditModel> validator,
            IAuthorRepository authorRepository,
            IMapper mapper,
            ILogger<IResult> logger)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid) 
            {
                 return Results.BadRequest(validationResult.Errors.ToResponse());
            }
            if(await authorRepository.IsAuthorExistBySlugAsync(0, model.UrlSlug))
            {
                return Results.Conflict($"Slug '{model.UrlSlug}' đã được sử dụng");
            }
            var author = mapper.Map<Author>(model);
            await authorRepository.AddOrUpdateAuthorAsync(author);

            return Results.CreatedAtRoute(
                "GetAuthorDetails", new { author.Id },
                mapper.Map<AuthorItem>(author));
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
                return Results.BadRequest("Không lưu được tập tin");
            }
            await authorRepository.SetImageUrlAsync(id, imageUrl);
            return Results.Ok(imageUrl);
        }

        private static async Task<IResult> UpdateAuthor(
          int id, AuthorEditModel model,
          IValidator<AuthorEditModel> validator,
          IAuthorRepository authorRepository,
          IMapper mapper)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors.ToResponse());
            }
            if (await authorRepository.IsAuthorExistBySlugAsync(0, model.UrlSlug))
            {
                return Results.Conflict($"Slug '{model.UrlSlug}' đã được sử dụng");
            }

            var author = mapper.Map<Author>(model);
            author.Id = id;

            return await authorRepository.AddOrUpdateAuthorAsync(author)
              ? Results.NoContent()
              : Results.NotFound();
        }

        private static async Task<IResult> DeleteAuthor(
          int id,
          IAuthorRepository authorRepository)
        {
            return await authorRepository.DeleteAuthorByIdAsync(id)
              ? Results.NoContent()
              : Results.NotFound($"Không tìm thấy tác giả có id = {id}");
        }
    }
}
