using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Feedback { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime DateComment { get; set; }
        public bool? IsApproved { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
