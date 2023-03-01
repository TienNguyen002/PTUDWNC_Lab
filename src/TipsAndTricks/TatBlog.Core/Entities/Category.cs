using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;

namespace TatBlog.Core.Entities
{   
    public class Category : IEntity                     //Biểu diễn các chuyên mục hay chủ đề
    {       
        public  int Id { get; set; }                    //Mã chuyên mục
        
        public string Name { get; set; }                //Tên chuyên mục, chủ đề
        
        public string UrlSlug { get; set; }             //Tên định danh dùng để tạo URL
        
        public string Description { get; set; }         //Mô tả thêm về chuyên mục
        
        public bool ShowOnMenu { get; set; }            //Đánh dấu chuyên mục được hiển thị trên menu
        
        public IList<Post> Posts { get; set; }          //Danh sách các bài viết thuộc chuyên mục
    }
}
