using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;

namespace TatBlog.Core.Entities
{
    public class Post : IEntity
    {    
        public  int Id { get; set; }                            //Mã bài viết
        
        public string Title { get; set; }                       //Tiêu đề bài viết
        
        public string ShortDescription { get; set; }            //Mô tả hay giới thiệu ngắn về nội dung
        
        public string Description { get; set; }                 //Nội dung chi tiết của bài viết
        
        public string Meta { get; set; }                        //Metadata
        
        public string UrlSlug { get; set; }                     //Tên định dạng để tạo URL
        
        public string ImageUrl { get; set; }                    //Đường dẫn tới file hình ảnh
        
        public int ViewCount { get; set; }                      //Số lượng xem, đọc bài viết
        
        public bool Published { get; set; }                     //Trạng thái của bài viết
       
        public DateTime PostedDate { get; set; }                //Ngày giờ đăng bài
        
        public DateTime? ModifiedDate { get; set; }             //Ngày giờ cập nhật lần cuối
       
        public int CategoryId { get; set; }                     //Mã chuyên mục
       
        public int AuthorId { get; set; }                       //Mã tác giả của bài viết
        
        public Category Category { get; set; }                  //Chuyên mục của bài viết
        
        public Author Author { get; set; }                      //Tác giả của bài viết
        
        public IList<Tag> Tags { get; set; }                    //Danh sách các từ khóa của bài viết
    }
}
