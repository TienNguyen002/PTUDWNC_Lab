using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface IAuthorRepository
    {
        Task<Author> GetAuthorByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<Author> GetAuthorBySlugAsync(string slug, CancellationToken cancellationToken = default);
    }
}
