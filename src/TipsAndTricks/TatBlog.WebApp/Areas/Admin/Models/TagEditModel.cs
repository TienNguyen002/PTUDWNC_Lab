using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TatBlog.WebApp.Areas.Admin.Models
{
    public class TagEditModel
    {
        public int Id { get; set; }

        [DisplayName("Tên chủ đề")]
        public string Name { get; set; }

        [DisplayName("Slug")]
        public string UrlSlug { get; set; }

        [DisplayName("Mô tả")]
        public string Description { get; set; }
    }
}
