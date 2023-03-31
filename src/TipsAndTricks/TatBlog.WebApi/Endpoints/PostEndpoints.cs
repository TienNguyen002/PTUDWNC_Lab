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
    public static class PostEndpoints
    {
        public static WebApplication MapPostEndpoints(
            this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/posts");

            routeGroupBuilder.MapGet("/", GetPosts)
              .WithName("GetPosts")
              .Produces<ApiResponse<PaginationResult<PostDto>>>();

            routeGroupBuilder.MapGet("/feature/{limit:int}", GetNPopularPosts)
              .WithName("GetNPopularPosts")
              .Produces<ApiResponse<IList<PostItems>>>();

            routeGroupBuilder.MapGet("/random/{limit:int}", GetNRandomPosts)
              .WithName("GetNRandomPosts")
              .Produces<ApiResponse<IList<PostItems>>>();

            routeGroupBuilder.MapGet("/archives/{limit:int}", GetPostsInNMonth)
              .WithName("GetPostsInNMonth")
              .Produces<ApiResponse<IList<PostItemsByMonth>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetPostDetail)
              .WithName("GetPostDetail")
              .Produces<ApiResponse<PostDetail>>();

            routeGroupBuilder.MapGet("/byslug/{slug:regex(^[a-z0-9_-]+$)}/posts", GetPostBySlug)
              .WithName("GetPostBySlug")
              .Produces<ApiResponse<PostDetail>>();

            routeGroupBuilder.MapPost("/", AddPost)
              .WithName("AddPost")
              .AddEndpointFilter<ValidatorFilter<PostEditModel>>()
              .Produces<ApiResponse<PostDto>>();

            routeGroupBuilder.MapPost("/{id:int}/picture", SetPostPicture)
              .WithName("SetPostPicture")
              .Accepts<IFormFile>("multipart/form-data")
              .Produces<ApiResponse<string>>();


            routeGroupBuilder.MapPut("/{id:int}", UpdatePost)
              .WithName("UpdatePost")
              .AddEndpointFilter<ValidatorFilter<PostEditModel>>()
              .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeletePost)
              .WithName("DeletePost")
              .Produces<ApiResponse<string>>();

            //routeGroupBuilder.MapGet("/{id:int}/comments", GetCommentsByPostId)
            //    .WithName("GetCommentsByPostId")
            //    .Produces<ApiResponse<IList<Comment>>>();

            return app;
        }

        private static async Task<IResult> GetPosts(
          [AsParameters] PostFilterModel model,
          IBlogRepository blogRepository,
          IMapper mapper)
        {
            var query = mapper.Map<PostQuery>(model);
            var posts = await blogRepository.GetPagedPostsAsync<PostDto>(query, model,
                posts => posts.ProjectToType<PostDto>());
            var paginationResult = new PaginationResult<PostDto>(posts);
            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetNPopularPosts(
            int limit,
            IBlogRepository blogRepository)
        {
            var posts = await blogRepository.GetNPopularPostsAsync(limit,
                posts => posts.ProjectToType<PostItems>());
            return Results.Ok(ApiResponse.Success(posts));
        }

        private static async Task<IResult> GetNRandomPosts(
            int limit,
            IBlogRepository blogRepository)
        {
            var posts = await blogRepository.GetNRandomPostsAsync(limit,
                posts => posts.ProjectToType<PostDto>());
            return Results.Ok(ApiResponse.Success(posts));
        }

        private static async Task<IResult> GetPostsInNMonth(
            int limit,
            IBlogRepository blogRepository)
        {
            var posts = await blogRepository.GetPostInNMonthAsync(limit);
            return Results.Ok(ApiResponse.Success(posts));
        }

        private static async Task<IResult> GetPostDetail(int id,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            var posts = await blogRepository.GetPostByIdAsync(id, true);
            return posts == null
                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy bài viết có mã số {id}"))
                : Results.Ok(ApiResponse.Success(mapper.Map<PostDetail>(posts)));
        }

        private static async Task<IResult> GetPostBySlug(
            [FromRoute] string slug,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            var posts = await blogRepository.GetPostBySlugAsync(slug, true);
            return posts == null
                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy bài viết có slug {slug}"))
                : Results.Ok(ApiResponse.Success(mapper.Map<PostDetail>(posts)));
        }

        private static async Task<IResult> AddPost(
            PostEditModel model,
            IBlogRepository blogRepository,
            IAuthorRepository authorRepository,
            IMapper mapper)
        {
            if(await blogRepository.IsPostSlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
                    $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }
            if(await authorRepository.GetAuthorByIdAsync(model.AuthorId) == null)
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
                    $"Không tìm thấy tác giả có id '{model.AuthorId}'"));
            }
            if (await blogRepository.GetCategoryByIdAsync(model.CategoryId) == null)
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
                    $"Không tìm thấy chủ đề có id '{model.CategoryId}'"));
            }
            var post = mapper.Map<Post>(model);
            post.PostedDate = DateTime.Now;
            await blogRepository.AddOrUpdatePostAsync(post, model.GetSelectedTags());

            return Results.Ok(ApiResponse.Success(
                mapper.Map<PostDto>(post), HttpStatusCode.Created));
        }

        private static async Task<IResult> SetPostPicture(
            int id,
            IFormFile imageFile,
            IBlogRepository blogRepository,
            IMediaManager mediaManager)
        {
            var imageUrl = await mediaManager.SaveFileAsync(
                imageFile.OpenReadStream(),
                imageFile.FileName, imageFile.ContentType);
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Không lưu được tập tin"));
            }
            await blogRepository.SetImageUrlPostAsync(id, imageUrl);
            return Results.Ok(ApiResponse.Success(imageUrl));
        }

        private static async Task<IResult> UpdatePost(
            int id, PostEditModel model,
            IBlogRepository blogRepository,
            IAuthorRepository authorRepository,
            IMapper mapper)
        {
            if (await blogRepository.IsPostSlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
                    $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }
            if (await authorRepository.GetAuthorByIdAsync(model.AuthorId) == null)
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
                    $"Không tìm thấy tác giả có id '{model.AuthorId}'"));
            }
            if (await blogRepository.GetCategoryByIdAsync(model.CategoryId) == null)
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
                    $"Không tìm thấy chủ đề có id '{model.CategoryId}'"));
            }
            var post = mapper.Map<Post>(model);
            post.Id = id;
            post.ModifiedDate = DateTime.Now;

            return await blogRepository.AddOrUpdatePostAsync(post, model.GetSelectedTags())
              ? Results.Ok(ApiResponse.Success("Cập nhật bài viết thành công", HttpStatusCode.NoContent))
              : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy bài viết có id = {id}"));
        }

        private static async Task<IResult> DeletePost(
            int id,
            IBlogRepository blogRepository)
        {
            return await blogRepository.DeletePostByIdAsync(id)
              ? Results.Ok(ApiResponse.Success("Xóa bài viết thành công", HttpStatusCode.NoContent))
              : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy bài viết có id = {id}"));
        }

        //private static async Task<IResult> GetCommentsByPostId(
        //    int id,
        //    ICommentRepository commentRepository)
        //{
        //    var comments = await commentRepository.GetCommentByPostAsync(id);
        //    return Results.Ok(ApiResponse.Success(comments));
        //}
    }
}
