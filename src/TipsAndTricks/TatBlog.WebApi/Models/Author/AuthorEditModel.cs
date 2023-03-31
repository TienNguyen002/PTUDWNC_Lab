namespace TatBlog.WebApi.Models.Author
{
    public class AuthorEditModel
    {
        public string FullName { get; set; }
        public string UrlSlug { get; set; }
        public DateTime JoinedDate { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }
    }
}
