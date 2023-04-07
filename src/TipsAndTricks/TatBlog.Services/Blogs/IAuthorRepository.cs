using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO.Author;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface IAuthorRepository
    {
        #region GetAuthorAsync (Lấy danh sách tác giả)
        Task<IList<AuthorItem>> GetAuthorAsync(
                CancellationToken cancellationToken = default);
        #endregion

        #region GetAuthorByIdAsync (Lấy tác giả theo Id)
        Task<Author> GetAuthorByIdAsync(int id, CancellationToken cancellationToken = default);
        #endregion

        #region GetCachedAuthorByIdAsync
        Task<Author> GetCachedAuthorByIdAsync(int authorId);
        #endregion

        #region GetAuthorBySlugAsync (Lấy tác giả theo slug)
        Task<Author> GetAuthorBySlugAsync(string slug, CancellationToken cancellationToken = default);
        #endregion

        #region GetCachedAuthorBySlugAsync
        Task<Author> GetCachedAuthorBySlugAsync(
            string slug, CancellationToken cancellationToken = default);

        #endregion

        #region IsAuthorExistBySlugAsync (Kiểm tra slug tồn tại của tác giả)
        Task<bool> CheckExistAuthorSlugByIdAsync(int id, string slug, CancellationToken cancellationToken = default);

        Task<bool> IsAuthorExistBySlugAsync(int id, string slug, CancellationToken cancellationToken = default
        );
        #endregion

        #region GetPagesAuthorsAsync <AuthorItem>(Lấy ds tác giả theo pagingParams)
        Task<IPagedList<AuthorItem>> GetPagesAuthorsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default);
        #endregion

        #region GetPagedAuthorsAsync <Author> (Lấy ds tác giả theo các tham số paging)
        Task<IPagedList<Author>> GetPagedAuthorsAsync(AuthorQuery authorQuery,
            int pageNumber,
            int pageSize,
            string sortColumn = "Id",
            string sortOrder = "ASC",
            CancellationToken cancellationToken = default);
        #endregion

        #region GetPagedAuthorsAsync <AuthorItem> (Lấy ds tác giả with name theo các tham số paging)
        Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(
            IPagingParams pagingParams,
            string name = null,
            CancellationToken cancellationToken = default);
        #endregion

        #region GetPagedAuthorsAsync<T> (Lấy ds tác giả theo pagingParams)
        Task<IPagedList<T>> GetPagedAuthorsAsync<T>(
            Func<IQueryable<Author>, IQueryable<T>> mapper,
            IPagingParams pagingParams,
            string name = null,
            CancellationToken cancellationToken = default);
        #endregion

        #region AddOrUpdateAuthorAsync (Thêm/Cập nhật tác giả)
        Task<bool> AddOrUpdateAuthorAsync(Author author, CancellationToken cancellationToken = default);
        #endregion

        #region DeleteAuthorByIdAsync (Xóa tác giả theo id)
        Task<bool> DeleteAuthorByIdAsync(int id, CancellationToken cancellationToken = default);
        #endregion

        #region SetImageUrlAsync (Đặt Avatar cho tác giả)
        Task<bool> SetImageUrlAsync(
            int authorId, string imageUrl,
            CancellationToken cancellationToken = default);
        #endregion

        #region GetPagedNPopularAuthorAsync (Lấy ds N tác giả có nhiều bài viết)
        Task<IPagedList<Author>> GetPagedNPopularAuthorAsync(int n, IPagingParams pagingParams, CancellationToken cancellationToken = default);
        #endregion

        #region GetNPopularAuthorAsync<T> (Lấy ds N tác giả có nhiều bài viết)
        Task<IPagedList<T>> GetPagedNPopularAuthorAsync<T>(int n, IPagingParams pagingParams, Func<IQueryable<Author>, IQueryable<T>> mapper, CancellationToken cancellationToken = default);
        #endregion

        #region GetNPopularAuthorsAsync (Lấy ds N tác giả có nhiều bài viết)
        Task<List<AuthorItem>> GetNPopularAuthorsAsync(int limit, CancellationToken cancellationToken = default);
        #endregion
    }
}
