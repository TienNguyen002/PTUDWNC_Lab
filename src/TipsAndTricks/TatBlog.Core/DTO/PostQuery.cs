using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
    public class PostQuery
    {
        public string KeyWord { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategorySlug { get; set; }
        public int AuthorId { get; set; }
        public string AuthorSlug { get; set; }
        public string TagSlug { get; set; }
        public int PostMonth { get; set; }
        public int PostYear { get; set; }
        public string Tags { get; set; }
        public bool PublishedOnly { get; set; }
        public bool NotPublished { get; set; }
        public List<string> GetTag()
        {
            return (Tags ?? "")
                .Split(new[] { ',', ';', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }

    }
}
