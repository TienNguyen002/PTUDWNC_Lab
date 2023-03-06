using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface ISubscriberRepository
    {
        Task<Subscriber> SubscribeAsync(string email, CancellationToken cancellationToken = default);


    }
}
