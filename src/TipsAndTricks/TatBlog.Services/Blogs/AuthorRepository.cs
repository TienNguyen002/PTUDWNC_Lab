using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;
using static Azure.Core.HttpHeader;

namespace TatBlog.Services.Blogs
{
    public class AuthorRepository : IAuthorRepository
  {

    private readonly BlogDbContext _context;
    public AuthorRepository(BlogDbContext context)
    {
      _context = context;
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

    public async Task AddOrUpdateAuthorAsync(
      Author author,
      CancellationToken cancellationToken = default)
    {
      if (author.Id > 0)
      {
        Author authorEdit = await Task.Run(() =>
          GetAuthorByIdAsync(author.Id, cancellationToken));

        if (authorEdit == null)
        {
          await Console.Out.WriteLineAsync("Khong tim thay tac gia");
          return;
        }
        if (authorEdit.UrlSlug != author.UrlSlug
          && await IsAuthorExistBySlugAsync(author.Id, author.UrlSlug, cancellationToken))
        {
          await Console.Out.WriteLineAsync("Da ton tai UrlSlug");
          return;
        }

        _context.Entry(authorEdit).CurrentValues.SetValues(author);
      }
      else
      {
        if (await IsAuthorExistBySlugAsync(author.Id, author.UrlSlug, cancellationToken))
        {
          await Console.Out.WriteLineAsync("Da ton tai UrlSlug");
          return;
        }
        _context.Set<Author>().Add(author);
      }
      await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Author> GetAuthorByIdAsync(
      int id,
      CancellationToken cancellationToken = default)
    {
      return await _context.Set<Author>().FindAsync(id, cancellationToken);
    }

    public async Task<Author> GetAuthorBySlugAsync(
      string slug,
      CancellationToken cancellationToken = default)
    {
      return await _context.Set<Author>()
        .Where(a => a.UrlSlug == slug)
        .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(
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

    public async Task<IPagedList<Author>> GetNPopularAuthors(
      int n,
      IPagingParams pagingParams,
      CancellationToken cancellationToken = default
    )
    {
      return await _context.Set<Author>()
                .Include (a => a.Posts)
                .OrderByDescending (a => a.Posts.Count(p =>p.Published))
                .Take(n)
                .ToPagedListAsync(pagingParams, cancellationToken);
    }

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
    }
}
