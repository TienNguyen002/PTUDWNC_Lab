using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;

namespace TatBlog.Core.Entities
{
    public class Author : IEntity                       //Biểu diễn tác giả của một bài viết
    {
        public int Id { get; set; }                     //Mã tác giả bài viết
        
        public string FullName { get; set; }            //Tên tác giả
        
        public string UrlSlug { get; set; }             //Tên định danh dùng để tạo URL
        
        public string ImageUrl { get; set; }            //Đường dẫn tới file hình ảnh
        
        public DateTime JoinedDate { get; set; }        //Ngày bắt đầu
        
        public string Email { get; set; }               //Địa chỉ email
        
        public string Notes { get; set; }               //Ghi chú

        public IList<Post> Posts { get; set; }          //Danh sách các bài viết của tác giả

        public override string ToString()
        {
            return String.Format("{0, -5}{1,-25}{2,-20}{3,-10}{4,-30}{5,-20}{6,-10}",
              Id, FullName, UrlSlug, ImageUrl, JoinedDate, Email, Notes
            );
        }
    }
}
