using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Data.Mappings;
using TatBlog.Services.Extensions;
using static Azure.Core.HttpHeader;

namespace TatBlog.Services.Blogs
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BlogDbContext _context;
        private readonly IMemoryCache _memoryCache;
        public AuthorRepository(BlogDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        #region GetAuthorAsync (Lấy danh sách tác giả)
        public async Task<IList<AuthorItem>> GetAuthorAsync(CancellationToken cancellationToken = default)
        {
            IQueryable<Author> authors = _context.Set<Author>();
            return await authors
                .OrderBy(x => x.FullName)
                .Select(x => new AuthorItem()
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    UrlSlug = x.UrlSlug,
                    ImageUrl = x.ImageUrl,
                    JoinedDate = x.JoinedDate,
                    Email = x.Email,
                    Notes = x.Notes,
                    PostsCount = x.Posts.Count(p => p.Published)
                })
                .OrderByDescending(s => s.PostsCount)
                .ToListAsync(cancellationToken);
        }
        #endregion

        #region GetAuthorByIdAsync (Lấy tác giả theo Id)
        public async Task<Author> GetAuthorByIdAsync(
          int id,
          CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>().FindAsync(id, cancellationToken);
        }
        #endregion

        #region GetCachedAuthorByIdAsync
        public async Task<Author> GetCachedAuthorByIdAsync(int authorId)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"author.by-id.{authorId}",
                async (entry) =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return await GetAuthorByIdAsync(authorId);
                });
        }
        #endregion

        #region GetAuthorBySlugAsync (Lấy tác giả theo slug)
        public async Task<Author> GetAuthorBySlugAsync(
          string slug,
          CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
              .Where(a => a.UrlSlug == slug)
              .FirstOrDefaultAsync(cancellationToken);
        }
        #endregion

        #region GetCachedAuthorBySlugAsync
        public async Task<Author> GetCachedAuthorBySlugAsync(
        string slug, CancellationToken cancellationToken = default)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"author.by-slug.{slug}",
                async (entry) =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return await GetAuthorBySlugAsync(slug, cancellationToken);
                });
        }
        #endregion

        #region IsAuthorExistBySlugAsync (Kiểm tra slug tồn tại của tác giả)
        public async Task<bool> CheckExistAuthorSlugByIdAsync(int id, string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
                .AnyAsync(a => a.Id != id && a.UrlSlug == slug, cancellationToken);
        }

        public async Task<bool> IsAuthorExistBySlugAsync(
          int id,
          string slug,
        CancellationToken cancellationToken = default
        )
        {
            return await _context.Set<Author>()
              .AnyAsync(a => a.Id != id && a.UrlSlug == slug, cancellationToken);
        }
        #endregion

        #region FindAuthorByQueryable (Tìm điều kiện)
        private IQueryable<Author> FindAuthorByQueryable(AuthorQuery query)
        {
            IQueryable<Author> authorQuery = _context.Set<Author>();
            if (!string.IsNullOrEmpty(query.KeyWord))
            {
                authorQuery = authorQuery.Where(a => a.FullName.Contains(query.KeyWord)
                || a.Email.Contains(query.KeyWord));
            }
            if (query.JoinedYear > 0)
            {
                authorQuery = authorQuery.Where(a => a.JoinedDate.Year == query.JoinedYear);
            }
            if (query.JoinedMonth > 0)
            {
                authorQuery = authorQuery.Where(a => a.JoinedDate.Month == query.JoinedMonth);
            }
            return authorQuery;
        }
        #endregion

        #region GetPagesAuthorsAsync <AuthorItem> (Lấy ds tác giả theo pagingParams)
        public async Task<IPagedList<AuthorItem>> GetPagesAuthorsAsync(
          IPagingParams pagingParams,
          CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
              .Select(a => new AuthorItem()
              {
                  Id = a.Id,
                  FullName = a.FullName,
                  UrlSlug = a.UrlSlug,
                  ImageUrl = a.ImageUrl,
                  JoinedDate = a.JoinedDate,
                  Email = a.Email,
                  Notes = a.Notes,
                  PostsCount = a.Posts.Count(p => p.Published),
              })
              .ToPagedListAsync(pagingParams, cancellationToken);
        }
        #endregion

        #region GetPagedAuthorsAsync <Author> (Lấy ds tác giả theo các tham số paging)
        public async Task<IPagedList<Author>> GetPagedAuthorsAsync(AuthorQuery authorQuery, int pageNumber, int pageSize, string sortColumn = "Id", string sortOrder = "ASC", CancellationToken cancellationToken = default)
        {
            var pagingParams = new PagingParams()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortColumn = sortColumn,
                SortOrder = sortOrder,
            };
            IQueryable<Author> authorResult = FindAuthorByQueryable(authorQuery);
            return await authorResult.ToPagedListAsync(pagingParams, cancellationToken);
        }
        #endregion

        #region GetPagedAuthorsAsync <AuthorItem> (Lấy ds tác giả theo các tham số paging)
        public async Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(
        IPagingParams pagingParams,
        string name = null,
        CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
                .AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(name),
                    x => x.FullName.Contains(name))
                .Select(a => new AuthorItem()
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    Email = a.Email,
                    JoinedDate = a.JoinedDate,
                    ImageUrl = a.ImageUrl,
                    UrlSlug = a.UrlSlug,
                    Notes = a.Notes,
                    PostsCount = a.Posts.Count(p => p.Published)
                })
                .ToPagedListAsync(pagingParams, cancellationToken);
        }
        #endregion

        #region GetPagedAuthorsAsync<T> (Lấy ds tác giả theo pagingParams)
        public async Task<IPagedList<T>> GetPagedAuthorsAsync<T>(
        Func<IQueryable<Author>, IQueryable<T>> mapper,
        IPagingParams pagingParams,
        string name = null,
        CancellationToken cancellationToken = default)
        {
            var authorQuery = _context.Set<Author>().AsNoTracking();

            if (!string.IsNullOrEmpty(name))
            {
                authorQuery = authorQuery.Where(x => x.FullName.Contains(name));
            }

            return await mapper(authorQuery)
                .ToPagedListAsync(pagingParams, cancellationToken);
        }
        #endregion

        #region AddOrUpdateAuthorAsync (Thêm/Cập nhật tác giả)
        public async Task<bool> AddOrUpdateAuthorAsync(
          Author author,
          CancellationToken cancellationToken = default)
        {
            if (author.Id > 0)
            {
                _context.Set<Author>().Update(author);
            }
            else
            {
                _context.Set<Author>().Add(author);
            }

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
        #endregion

        #region DeleteAuthorByIdAsync (Xóa tác giả theo id)
        public async Task<bool> DeleteAuthorByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var authorDelete = await _context.Set<Author>()
                .Include(a => a.Posts)
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
            if (authorDelete == null)
            {
                return false;
            }
            _context.Remove(authorDelete);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        #endregion

        #region SetImageUrlAsync (Đặt Avatar cho tác giả)
        public async Task<bool> SetImageUrlAsync(
        int authorId, string imageUrl,
        CancellationToken cancellationToken = default)
        {
            return await _context.Authors
                .Where(x => x.Id == authorId)
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(a => a.ImageUrl, a => imageUrl),
                    cancellationToken) > 0;
        }
        #endregion

        #region GetNPopularAuthorAsync (Lấy ds N tác giả có nhiều bài viết)
        public async Task<IPagedList<Author>> GetNPopularAuthorAsync(
          int n,
          IPagingParams pagingParams,
          CancellationToken cancellationToken = default
        )
        {
            return await _context.Set<Author>()
                      .Include(a => a.Posts)
                      .OrderByDescending(a => a.Posts.Count(p => p.Published))
                      .Take(n)
                      .ToPagedListAsync(pagingParams, cancellationToken);
        }
        #endregion

        #region GetNPopularAuthorAsync<T> (Lấy ds N tác giả có nhiều bài viết)
        public async Task<IPagedList<T>> GetNPopularAuthorAsync<T>(int n, IPagingParams pagingParams, Func<IQueryable<Author>, IQueryable<T>> mapper, CancellationToken cancellationToken = default)
        {
            IQueryable<Author> authors = _context.Set<Author>()
              .Include(a => a.Posts)
              .OrderByDescending(a => a.Posts.Count(p => p.Published))
              .Take(n);
            return await mapper(authors).ToPagedListAsync(pagingParams, cancellationToken);
        }
        #endregion
    }
}
