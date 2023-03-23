using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface ICommentRepository
    {
        Task<Comment> GetCommentByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IList<Comment>> GetCommentByPostAsync(int postId, CancellationToken cancellationToken = default);
        Task<IList<Comment>> GetCommentByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<Comment> AddCommentAsync(Comment comment, CancellationToken cancellationToken = default);
        Task<bool> DeleteCommentAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ApproveCommentAsync(int id, bool isApproved = true, CancellationToken cancellationToken = default);
    }
}
