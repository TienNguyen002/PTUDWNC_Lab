using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Globalization;

namespace TatBlog.WebApp.Areas.Admin.Models
{
    public class AuthorFilterModel
    {
        [DisplayName("Từ khóa")]
        public string KeyWord { get; set; }

        [DisplayName("Năm")]
        public int? JoinedYear { get; set; }

        [DisplayName("Tháng")]
        public int? JoinedMonth { get; set; }
        public IEnumerable<SelectListItem>? Months { get; set; }
        public AuthorFilterModel() 
        {
            Months = Enumerable.Range(1,12)
                .Select(m => new SelectListItem()
                {
                    Value = m.ToString(),
                    Text = CultureInfo.CurrentCulture
                    .DateTimeFormat.GetMonthName(m),
                })
                .ToList();
        }
    }
}
