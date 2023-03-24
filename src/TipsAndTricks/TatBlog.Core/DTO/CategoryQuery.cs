using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
    public class CategoryQuery
    {
        public string KeyWord { get; set; }
        public bool NotShowOnMenu { get; set; }
    }
}
