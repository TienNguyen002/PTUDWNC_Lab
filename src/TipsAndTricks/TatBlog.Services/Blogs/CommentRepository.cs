using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Services.Blogs
{
    public class CommentRepository : ICommentRepository
    {
        private readonly BlogDbContext _context;
        public CommentRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> AddCommentAsync(Comment comment, CancellationToken cancellationToken = default)
        {
            if(comment.Id < 0)
            {
                _context.Set<Comment>().Add(comment);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return comment;
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
                .Where(c => c.Id == id)
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
