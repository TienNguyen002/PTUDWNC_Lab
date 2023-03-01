using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;

namespace TatBlog.Core.Entities
{
    public class Tag : IEntity
    {
        public int Id { get; set; }                     //Mã từ khóa
        
        public string Name { get; set; }                //Nội dung từ khóa
        
        public string UrlSlug { get; set; }             //Tên định dạng để tạo URL
        
        public string Description { get; set; }         //Mô tả thêm về từ khóa
        
        public IList<Post> Posts { get; set; }          //Danh sách bài viết có chứa từ khóa
    }
}
