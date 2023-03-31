namespace TatBlog.WebApi.Models.Post
{
    public class PostFilterModel : PagingModel
    {
        public string KeyWord { get; set; }
        public int? AuthorId { get; set; }
        public int? CategoryId { get; set; }
        public bool? Published { get; set; }
        public int? PostYear { get; set; }
        public int? PostMonth { get; set; }
    }
}
