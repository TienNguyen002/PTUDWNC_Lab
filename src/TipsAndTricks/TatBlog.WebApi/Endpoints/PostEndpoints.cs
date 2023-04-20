using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SlugGenerator;
using System;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO.Post;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;
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
              .Produces<ApiResponse<IList<PostDto>>>();

            routeGroupBuilder.MapGet("/random/{limit:int}", GetNRandomPosts)
              .WithName("GetNRandomPosts")
              .Produces<ApiResponse<IList<PostDto>>>();

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
              .Accepts<PostEditModel>("multipart/form-data")
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

            routeGroupBuilder.MapGet("/chage-post-published/{id:int}", ChangePublishedStatus)
              .WithName("ChangePublishedStatus")
              .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapGet("/get-posts-filter", GetFilteredPosts)
                .WithName("GetFilteredPost")
                .Produces<ApiResponse<PaginationResult<PostDto>>>();

            routeGroupBuilder.MapGet("/get-filter", GetFilter)
                .WithName("GetFilter")
                .Produces<ApiResponse<PostFilterModel>>();
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
                posts => posts.ProjectToType<PostDto>());
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
            HttpContext context,
            IBlogRepository blogRepository,
            IAuthorRepository authorRepository,
            IMapper mapper,
            IMediaManager mediaManager)
        {
            var model = await PostEditModel.BindAsync(context);
            var slug = model.Title.GenerateSlug();
            if (await blogRepository.IsPostSlugExistedAsync(0, slug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
                    $"Slug '{slug}' đã được sử dụng"));
            }
            var post = model.Id > 0 ? await blogRepository.GetPostByIdAsync(model.Id) : null;
            if(post == null)
            {
                post = new Post()
                {
                    PostedDate = DateTime.Now,
                };
            }
            post.Title = model.Title;
            post.AuthorId = model.AuthorId;
            post.CategoryId = model.CategoryId;
            post.ShortDescription = model.ShortDescription;
            post.Description = model.Description;
            post.Meta = model.Meta;
            post.Published = model.Published;
            post.ModifiedDate = DateTime.Now;
            post.UrlSlug = model.Title.GenerateSlug();

            if(model.ImageFile?.Length > 0)
            {
                string hostname = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}/",
                    uploadedPath = await mediaManager.SaveFileAsync(model.ImageFile.OpenReadStream(),
                    model.ImageFile.FileName,
                    model.ImageFile.ContentType);
                if (!string.IsNullOrWhiteSpace(uploadedPath))
                {
                    post.ImageUrl = uploadedPath;
                }
            }
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

        //private static async Task<IResult> UpdatePost(
        //    int id, PostEditModel model,
        //    IBlogRepository blogRepository,
        //    IAuthorRepository authorRepository,
        //    IMapper mapper)
        //{
        //    var post = await blogRepository.GetPostByIdAsync(id);

        //    if (post == null)
        //    {
        //        return Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
        //                $"Không tìm thấy bài viết có id = {id}"));
        //    }
        //    if (await blogRepository.IsPostSlugExistedAsync(0, model.UrlSlug))
        //    {
        //        return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
        //            $"Slug '{model.UrlSlug}' đã được sử dụng"));
        //    }
        //    if (await authorRepository.GetAuthorByIdAsync(model.AuthorId) == null)
        //    {
        //        return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
        //            $"Không tìm thấy tác giả có id '{model.AuthorId}'"));
        //    }
        //    if (await blogRepository.GetCategoryByIdAsync(model.CategoryId) == null)
        //    {
        //        return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
        //            $"Không tìm thấy chủ đề có id '{model.CategoryId}'"));
        //    }
        //    post = mapper.Map<Post>(model);
        //    post.Id = id;
        //    post.ModifiedDate = DateTime.Now;

        //    return await blogRepository.AddOrUpdatePostAsync(post, model.GetSelectedTags())
        //      ? Results.Ok(ApiResponse.Success("Cập nhật bài viết thành công", HttpStatusCode.NoContent))
        //      : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy bài viết có id = {id}"));
        //}

        private static async Task<IResult> UpdatePost(
             int id,
             PostEditModel model,
             IMapper mapper,
             IBlogRepository blogRepository,
             IAuthorRepository authorRepository,
             IMediaManager mediaManager)
        {
            var post = await blogRepository.GetPostByIdAsync(id);
            if (post == null)
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
                        $"Không tìm thấy bài viết có id = {id}"));
            }
            //if (await blogRepository.IsPostSlugExistedAsync(0, model.UrlSlug))
            //{
            //    return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
            //        $"Slug '{model.slug}' đã được sử dụng"));
            //}
            if (await authorRepository.GetAuthorByIdAsync(model.AuthorId) == null)
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
                    $"Không tìm thấy tác giả id = {model.AuthorId}"));
            }
            if (await blogRepository.GetCategoryByIdAsync(model.CategoryId) == null)
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
                    $"Không tìm thấy chủ đề id = {model.CategoryId}"));
            }

            mapper.Map(model, post);
            post.Id = id;
            post.ModifiedDate = DateTime.Now;

            return await blogRepository.AddOrUpdatePostAsync(post, model.GetSelectedTags())
               ? Results.Ok(ApiResponse.Success($"Thay đổi bài viết id = {id} thành công"))
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

        private static async Task<IResult> GetFilter(
            IAuthorRepository authorRepository,
            IBlogRepository blogRepository)
        {
            var model = new PostFilterModel()
            {
                AuthorList = (await authorRepository.GetAuthorAsync())
                .Select(a => new SelectListItem()
                {
                    Text = a.FullName,
                    Value = a.Id.ToString()
                }),
                CategoryList = (await blogRepository.GetCategoriesAsync())
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
            };
            return Results.Ok(ApiResponse.Success(model));
        }

        private static async Task<IResult> GetFilteredPosts(
            [AsParameters] PostFilterModel model,
            IMapper mapper,
            IBlogRepository blogRepository)
        {
            var postQuery = mapper.Map<PostQuery>(model);
            var postsList = await blogRepository.GetPagedPostsAsync(postQuery, model, posts =>
            posts.ProjectToType<PostDto>());
            var paginationResult = new PaginationResult<PostDto>(postsList);
            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> ChangePublishedStatus(
           int id,
           IBlogRepository blogRepository)
        {
            var post = await blogRepository.GetPostByIdAsync(id);
            if(post == null) 
            {
                Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy bài viết có id = {id}"));
            }
            await blogRepository.ChangePublishedPostAsync(id);
            return Results.Ok(ApiResponse.Success("Chỉnh trạng thái thành công", HttpStatusCode.NoContent));
        }
    }
}
