using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Services.Blogs
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BlogDbContext _context;

        public AuthorRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<Author> GetAuthorByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
               .Where(c => c.Id == id)
               .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Author> GetAuthorBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            IQueryable<Author> authorsQuery = _context.Set<Author>();
            if (!string.IsNullOrWhiteSpace(slug))
            {
                authorsQuery = authorsQuery.Where(x => x.UrlSlug == slug);
            }
            return await authorsQuery.FirstOrDefaultAsync(cancellationToken);
        }
    }
}
