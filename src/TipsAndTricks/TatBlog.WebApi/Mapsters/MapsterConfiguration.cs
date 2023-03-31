using Mapster;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.WebApi.Models;

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
        }
    }
}
