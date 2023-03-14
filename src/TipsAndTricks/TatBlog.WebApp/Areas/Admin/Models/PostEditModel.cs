using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TatBlog.WebApp.Areas.Admin.Models
{
    public class PostEditModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public string? Meta { get; set; }
        public string? UrlSlug { get; set; }
        public IFormFile ImageFile { get; set; }
        public string? ImageUrl { get; set; }
        public bool Published { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
        public string? SelectedTags { get; set; }

        public IEnumerable<SelectListItem> AuthorList { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public List<string> GetSelectedTags()
        {
            return (SelectedTags ?? "")
                .Split(new[] { ',', ';', '\r', '\n'},
                StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }
    }
}
