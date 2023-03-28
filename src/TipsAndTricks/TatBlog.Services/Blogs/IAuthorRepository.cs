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
    public interface IAuthorRepository
    {
        Task<IList<AuthorItem>> GetAuthorAsync(
                CancellationToken cancellationToken = default);
        Task<Author> GetAuthorByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<Author> GetAuthorBySlugAsync(string slug, CancellationToken cancellationToken = default);

        Task<Author> GetCachedAuthorBySlugAsync(
            string slug, CancellationToken cancellationToken = default);

        Task<Author> GetCachedAuthorByIdAsync(int authorId);

        Task<IPagedList<AuthorItem>> GetPagesAuthorsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default);

        Task<IPagedList<Author>> GetPagedAuthorsAsync(AuthorQuery authorQuery,
            int pageNumber,
            int pageSize,
            string sortColumn = "Id",
            string sortOrder = "ASC",
            CancellationToken cancellationToken = default);

        Task<Author> AddOrUpdateAuthorAsync(Author author, CancellationToken cancellationToken = default);

        Task<IPagedList<Author>> GetNPopularAuthors(int n, IPagingParams pagingParams, CancellationToken cancellationToken = default);

        Task<bool> DeleteAuthorByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> CheckExistAuthorSlugByIdAsync(int id, string slug, CancellationToken cancellationToken = default);

        Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(
        IPagingParams pagingParams,
        string name = null,
        CancellationToken cancellationToken = default);

        Task<IPagedList<T>> GetPagedAuthorsAsync<T>(
            Func<IQueryable<Author>, IQueryable<T>> mapper,
            IPagingParams pagingParams,
            string name = null,
            CancellationToken cancellationToken = default);

        Task<bool> IsAuthorExistBySlugAsync(
          int id,
          string slug,
        CancellationToken cancellationToken = default
        );
    }
}
