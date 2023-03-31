using Mapster;
using TatBlog.Core.DTO.Author;
using TatBlog.Core.DTO.Category;
using TatBlog.Core.DTO.Post;
using TatBlog.Core.DTO.Tag;
using TatBlog.Core.Entities;
using TatBlog.WebApi.Models.Author;
using TatBlog.WebApi.Models.Category;
using TatBlog.WebApi.Models.Post;
using TatBlog.WebApi.Models.Tag;

namespace TatBlog.WebApi.Mapsters
{
    public class MapsterConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Author, AuthorDto>();
            config.NewConfig<Author, AuthorItem>()
                .Map(desc => desc.PostsCount,
                    src => src.Posts == null ? 0 : src.Posts.Count);
            config.NewConfig<AuthorEditModel, Author>();

            config.NewConfig<Category, CategoryDto>();
            config.NewConfig<Category, CategoryItem>()
                .Map(desc => desc.PostCount,
                    src => src.Posts == null ? 0 : src.Posts.Count);

            config.NewConfig<Tag, TagDto>();
            config.NewConfig<Tag, TagItem>()
                .Map(desc => desc.PostCount,
                    src => src.Posts == null ? 0 : src.Posts.Count);

            config.NewConfig<Post, PostDto>();
            config.NewConfig<Post, PostDetail>();
            config.NewConfig<PostFilterModel, PostQuery>()
                .Map(desc => desc.PublishedOnly, src => false);
            config.NewConfig<PostEditModel, Post>()
                .Ignore(desc => desc.ImageUrl);
        }
    }
}
