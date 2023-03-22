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
    internal class CommentRepository : ICommentRepository
    {
        private readonly BlogDbContext _context;
        public CommentRepository(BlogDbContext context)
        {
            _context = context;
        }

        public Task<Comment> AddCommentAsync(Comment comment, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ApproveCommentAsync(int id, bool isApproved = true, CancellationToken cancellationToken = default)
        {
            Comment comment = await GetCommentByIdAsync(id, cancellationToken);
            if (comment == null)
            {
                return false;
            }
            comment.IsApproved = isApproved;
            await _context.SaveChangesAsync(cancellationToken);
            return true;

        }

        public async Task<bool> DeleteCommentAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Comment>()
                .Where(comment => comment.Id == id)
                .ExecuteDeleteAsync(cancellationToken) > 0;
        }

        public async Task<IList<Comment>> GetCommentByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Comment>()
                .Where(c => c.Email == email)
                .ToListAsync(cancellationToken);
        }

        public async Task<Comment> GetCommentByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Comment>().FindAsync(id, cancellationToken);
        }

        public async Task<IList<Comment>> GetCommentByPostAsync(int postId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Comment>()
                .Where(c => c.PostId == postId)
                .ToListAsync(cancellationToken);
        }
    }
}
