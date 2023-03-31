using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO.Post
{
    public class PostItems
    {
        public string CategoryName { get; set; }
        public string Tags { get; set; }
        public int PostYear { get; set; }
        public int PostMonth { get; set; }
        public int PostCount { get; set; }
    }
}
