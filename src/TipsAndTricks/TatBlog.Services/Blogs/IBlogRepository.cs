using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface IBlogRepository
    {
        //Tìm bài viết có tên định danh là 'slug' và được đăng vào tháng 'month' năm 'year'
        Task<Post> GetPostAsync(
            int year,
            int month,
            string slug,
            CancellationToken cancellationToken = default);

        //Tìm Top N bài viết phổ được nhiều người xem nhất
        Task<IList<Post>> GetPopularArticlesAsync(
            int numPosts,
            CancellationToken cancellationToken = default);

        //Kiểm tra xem tên định danh của bài viết đã có hay chưa
        Task<bool> IsPostSlugExistedAsync(
            int postId, string slug,
            CancellationToken cancellationToken = default);

        //Tăng số lượt xem của một bài viết
        Task IncreaseViewCountAsync(
            int postId,
            CancellationToken cancellationToken = default);

        //Lấy danh sách chuyên mục và số lượng bài viết nằm thuộc từng chuyên mục/chủ đề
        Task<IList<CategoryItem>> GetCategoriesAsync(
            bool showOnMenu = false,
            CancellationToken cancellationToken = default);

        //Lấy danh sách từ khóa/thẻ và phân trang theo các tham số pagingParams
        Task<IPagedList<TagItem>> GetPagedTagsAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default);

        //Tag
        Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default);

        Task<IList<TagItem>> GetAllTagsAsync(CancellationToken cancellationToken = default);

        Task<bool> DeleteTagByIdAsync(int id, CancellationToken cancellationToken = default);

        //Category
        Task<Category> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default);

        Task<Category> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> CheckExistCategorySlugAsync(string slug, CancellationToken cancellationToken = default);

        Task AddOrUpdateCategoryAsync(Category category , CancellationToken cancellationToken = default);

        Task<bool> DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default);

        //Post
        Task<IList<PostItems>> GetPostInNMonthAsync(int month, CancellationToken cancellationToken = default);
        Task<IList<PostItemsByMonth>> GetPostInMonthAndYearAsync(int month, CancellationToken cancellationToken = default);

        Task<Post> GetPostByIdAsync(int id, bool includeDetails = false, CancellationToken cancellationToken = default);

        Task<Post> AddOrUpdatePostAsync(Post post, IEnumerable<string> tags, CancellationToken cancellationToken = default);

        Task ChangePublishedPostAsync(int id, bool published, CancellationToken cancellationToken = default);

        Task<IList<Post>> GetNRandomPostsAsync(
            int numPosts,
            CancellationToken cancellationToken = default);

        Task<IList<Post>> FindPostByQueryAsync(PostQuery postQuery, CancellationToken cancellationToken = default);

        Task<IList<Post>> FindAllPostByQueryAsync(PostQuery postQuery, CancellationToken cancellationToken = default);

        Task<int> CountPostQueryAsync(PostQuery postQuery, CancellationToken cancellationToken= default);

        Task<IPagedList<Post>> GetPagesPostQueryAsync(PostQuery postQuery, IPagingParams pagingParams, CancellationToken cancellationToken = default);

        Task<IPagedList<Post>> GetPagesAllPostQueryAsync(PostQuery postQuery, IPagingParams pagingParams, CancellationToken cancellationToken = default);

        Task<IPagedList<T>> GetPagesPostsAsync<T>(PostQuery postQuery, IPagingParams pagingParams, Func<IQueryable<Post>, IQueryable<T>> mapper, CancellationToken cancellationToken = default);
    }
}
