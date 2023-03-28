using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace TatBlog.WebApp.Extensions
{
    public static class EnumerableExtension
    {
        public static IEnumerable<SelectListItem> PopulateMonthNames(
      this IEnumerable<SelectListItem> enumerable)
        {
            return Enumerable.Range(1, 12)
                    .Select(m => new SelectListItem()
                    {
                        Value = m.ToString(),
                        Text = CultureInfo.CurrentCulture
                      .DateTimeFormat.GetMonthName(m)
                    })
                    .ToList();
        }
    }
}
