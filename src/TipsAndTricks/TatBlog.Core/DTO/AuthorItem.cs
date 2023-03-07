using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;

namespace TatBlog.Core.DTO
{
  public class AuthorItem
  {
    public int Id { get; set; }
    public string FullName { get; set; }
    public string UrlSlug { get; set; }
    public string ImageUrl { get; set; }
    public DateTime JoinedDate { get; set; }
    public string Email { get; set; }
    public string Notes { get; set; }
    public int PostsCount { get; set; }

    public override string ToString()
    {
      return String.Format("{0, -5}{1,-25}{2,-20}{3,-10}{4,-30}{5,-20}{6,-10}{7,-20}",
        Id, FullName, UrlSlug, ImageUrl, JoinedDate, Email, Notes, PostsCount
      );
    }
  }
}
