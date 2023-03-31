using Mapster;
using TatBlog.Core.DTO.Category;
using TatBlog.Core.DTO.Post;
using TatBlog.Core.DTO.Tag;
using TatBlog.Core.Entities;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Mapsters
{
    public class MapsterConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Post, PostItems>()
                .Map(dest => dest.CategoryName, src => src.Category.Name)
                .Map(dest => dest.Tags, src => src.Tags.Select(x => x.Name));

            config.NewConfig<PostFilterModel, PostQuery>()
                .Map(dest => dest.PublishedOnly, src => false);

            config.NewConfig<PostEditModel, Post>()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.ImageUrl);

            config.NewConfig<Post, PostEditModel>()
                .Map(dest => dest.SelectedTags,
                     src => string.Join("\r\n", src.Tags.Select(x => x.Name)))
                .Ignore(dest => dest.CategoryList)
                .Ignore(dest => dest.AuthorList)
                .Ignore(dest => dest.ImageFile);

            config.NewConfig<Category, CategoryItem>()
                .Map(dest => dest.PostCount, src => src.Posts.Count);

            config.NewConfig<Tag, TagItem>()
                .Map(dest => dest.PostCount, src => src.Posts.Count);

            config.NewConfig<AuthorEditModel, Author>()
               .Ignore(dest => dest.Id)
               .Ignore(dest => dest.ImageUrl);
        }
    }
}
