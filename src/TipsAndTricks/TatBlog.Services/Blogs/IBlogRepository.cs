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
        #region Tag
        #region GetPagesTagsAsync (Lấy ds Tag và phân trang theo pagingParams)
        //Lấy danh sách từ khóa/thẻ và phân trang theo các tham số pagingParams
        Task<IPagedList<TagItem>> GetPagesTagsAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default);
        #endregion

        #region GetTagBySlugAsync (Lấy ds Tag bằng slug)
        Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default);
        #endregion

        #region GetTagByIdAsync (Lấy ds Tag bằng Id)
        Task<Tag> GetTagByIdAsync(int id, CancellationToken cancellationToken = default);
        #endregion

        #region GetAllTagsAsync (Lấy ds tất cả các Tag)
        Task<IList<TagItem>> GetAllTagsAsync(CancellationToken cancellationToken = default);
        #endregion

        #region DeleteTagByIdAsync (Xóa Tag theo Id)
        Task<bool> DeleteTagByIdAsync(int id, CancellationToken cancellationToken = default);
        #endregion

        #region AddOrUpdateTagAsync (Thêm/Cập nhật Tag)
        Task<Tag> AddOrUpdateTagAsync(Tag tag, CancellationToken cancellationToken = default);
        #endregion

        #region CheckExistTagSlugByIdAsync (Kiểm tra slug tồn tại bằng id - Tag)
        Task<bool> CheckExistTagSlugByIdAsync(int id, string slug, CancellationToken cancellationToken = default);
        #endregion

        #region GetPagedTagsAsync (Lấy ds Tag và phân trang theo các tham số Paging)
        Task<IPagedList<Tag>> GetPagedTagsAsync(TagQuery tagQuery,
            int pageNumber,
            int pageSize,
            string sortColumn = "Id",
            string sortOrder = "ASC",
            CancellationToken cancellationToken = default);
        #endregion

        #region GetPagedTagsAsync<T>
        Task<IPagedList<T>> GetPagedTagsAsync<T>(
        TagQuery query,
        int pageNumber,
        int pageSize,
        Func<IQueryable<Tag>, IQueryable<T>> mapper,
        string sortColumn = "Id",
        string sortOrder = "ASC",
        CancellationToken cancellationToken = default);
        #endregion
        #endregion

        #region Category
        #region GetCategoriesAsync (Lấy ds Category và số lượng bài viết thuộc Category)
        //Lấy danh sách chuyên mục và số lượng bài viết nằm thuộc từng chuyên mục/chủ đề
        Task<IList<CategoryItem>> GetCategoriesAsync(
            bool showOnMenu = false,
            CancellationToken cancellationToken = default);
        #endregion

        #region GetCategoryBySlugAsync (Lấy ds Category bằng Slug)
        Task<Category> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default);
        #endregion

        #region GetCategoryByIdAsync (Lấy ds Category bằng Id)
        Task<Category> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default);
        #endregion

        #region CheckExistCategorySlugAsync (Kiểm tra slug tồn tại - Category)
        Task<bool> CheckExistCategorySlugAsync(string slug, CancellationToken cancellationToken = default);
        #endregion

        #region CheckExistCategorySlugByIdAsync (Kiểm tra slug tồn tại bằng id - Category)
        Task<bool> CheckExistCategorySlugByIdAsync(int id, string slug, CancellationToken cancellationToken = default);
        #endregion

        #region AddOrUpdateCategoryAsync (Thêm/Cập nhật Category)
        Task<Category> AddOrUpdateCategoryAsync(Category category, CancellationToken cancellationToken = default);
        #endregion

        #region DeleteCategoryByIdAsync (Xóa Category bằng Id)
        Task<bool> DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken = default);
        #endregion

        #region ChangeShowOnMenuCategoryAsync
        Task ChangeShowOnMenuCategoryAsync(int id, CancellationToken cancellationToken = default);
        #endregion

        #region GetPagesCategoriesAsync (Lấy ds Category và phân trang theo pagingParams)
        Task<IPagedList<CategoryItem>> GetPagesCategoriesAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default);
        #endregion

        #region GetPagedCategoriesAsync (Lấy ds Category và phân trang theo các tham số của Paging)
        Task<IPagedList<Category>> GetPagedCategoriesAsync(
            CategoryQuery categoryQuery,
            int pageNumber,
            int pageSize,
            string sortColumn = "Id",
            string sortOrder = "ASC",
            CancellationToken cancellationToken = default);
        #endregion

        #region GetPagedCategoriesAsync<T>
        Task<IPagedList<T>> GetPagedCategoriesAsync<T>(
      CategoryQuery query,
      int pageNumber,
      int pageSize,
      Func<IQueryable<Category>, IQueryable<T>> mapper,
      string sortColumn = "Id",
      string sortOrder = "ASC",
      CancellationToken cancellationToken = default);
        #endregion
        #endregion

        #region Post
        #region GetPostAsync (Tìm Post theo slug và month year)
        //Tìm bài viết có tên định danh là 'slug' và được đăng vào tháng 'month' năm 'year'
        Task<Post> GetPostAsync(
            int year,
            int month,
            string slug,
            CancellationToken cancellationToken = default);
        #endregion

        #region GetPopularArticlesAsync (Tìm N Post dc nhiều ng xem nhất)
        //Tìm Top N bài viết phổ được nhiều người xem nhất
        Task<IList<Post>> GetPopularArticlesAsync(
            int numPosts,
            CancellationToken cancellationToken = default);
        #endregion

        #region IsPostSlugExistedAsync (Kiểm tra Post đã có slug chưa)
        //Kiểm tra xem tên định danh của bài viết đã có hay chưa
        Task<bool> IsPostSlugExistedAsync(
            int postId, string slug,
            CancellationToken cancellationToken = default);
        #endregion

        #region IncreaseViewCountAsync (Tăng lượt xem của Post)
        //Tăng số lượt xem của một bài viết
        Task IncreaseViewCountAsync(
            int postId,
            CancellationToken cancellationToken = default);
        #endregion

        #region GetNRandomPostsAsync (Lấy ngẫu nhiên N Post)
        Task<IList<Post>> GetNRandomPostsAsync(
            int numPosts,
            CancellationToken cancellationToken = default);
        #endregion

        #region GetPostInNMonthAsync (Lấy ds Post theo tháng)
        Task<IList<PostItems>> GetPostInNMonthAsync(int month, CancellationToken cancellationToken = default);
        #endregion

        #region GetPostInMonthAndYearAsync (Lấy ds Post theo tháng và năm)
        Task<IList<PostItemsByMonth>> GetPostInMonthAndYearAsync(int month, CancellationToken cancellationToken = default);
        #endregion

        #region GetPostByIdAsync (Lấy ds Post bằng Id)
        Task<Post> GetPostByIdAsync(int id, bool includeDetails = false, CancellationToken cancellationToken = default);
        #endregion

        #region AddOrUpdatePostAsync (Thêm/Cập nhật Post)
        Task<Post> AddOrUpdatePostAsync(Post post, IEnumerable<string> tags, CancellationToken cancellationToken = default);
        #endregion

        #region ChangePublishedPostAsync (Thay đổi trạng thái Publish/NotPushlish của Post)
        Task ChangePublishedPostAsync(int id, CancellationToken cancellationToken = default);
        #endregion

        #region Find(All)PostByQueryAsync (Tìm ds Post bằng Query)
        Task<IList<Post>> FindPostByQueryAsync(PostQuery postQuery, CancellationToken cancellationToken = default);

        Task<IList<Post>> FindAllPostByQueryAsync(PostQuery postQuery, CancellationToken cancellationToken = default);
        #endregion

        #region CountPostQueryAsync (Đếm số lượng Post)
        Task<int> CountPostQueryAsync(PostQuery postQuery, CancellationToken cancellationToken = default);
        #endregion

        #region GetPages(All)PostQueryAsync (Lấy ds Post và phân trang theo pagingParams)
        Task<IPagedList<Post>> GetPagesPostQueryAsync(PostQuery postQuery, IPagingParams pagingParams, CancellationToken cancellationToken = default);

        Task<IPagedList<Post>> GetPagesAllPostQueryAsync(PostQuery postQuery, IPagingParams pagingParams, CancellationToken cancellationToken = default);
        #endregion

        #region GetPagesPostsAsync<T> (Lấy ds Post theo T)
        Task<IPagedList<T>> GetPagesPostsAsync<T>(PostQuery postQuery, IPagingParams pagingParams, Func<IQueryable<Post>, IQueryable<T>> mapper, CancellationToken cancellationToken = default);
        #endregion

        #region Get(All)PagedPostQueryAsync (Lấy ds Post và phân trang theo các tham số của Paging)
        Task<IPagedList<Post>> GetPagedPostQueryAsync(PostQuery postQuery,
            int pageNumber,
            int pageSize,
            string sortColumn = "Id",
            string sortOrder = "ASC",
            CancellationToken cancellationToken = default);

        Task<IPagedList<Post>> GetAllPagedPostQueryAsync(PostQuery postQuery,
            int pageNumber,
            int pageSize,
            string sortColumn = "Id",
            string sortOrder = "ASC",
            CancellationToken cancellationToken = default);
        #endregion

        #region DeletePostByIdAsync (Xóa Post theo Id)
        Task<bool> DeletePostByIdAsync(int id, CancellationToken cancellationToken = default);
        #endregion
        #endregion
    }
}
