using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
    public class PostItemsByMonth
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int PostCount { get; set; }
        public string MonthName
        {
            get => CultureInfo
              .CurrentCulture
              .DateTimeFormat
              .GetMonthName(Month);
        }
    }
}
