using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
    public class AuthorQuery
    {
        public string KeyWord { get; set; }
        public int? JoinedMonth { get; set; }
        public int? JoinedYear { get; set; }
    }
}
