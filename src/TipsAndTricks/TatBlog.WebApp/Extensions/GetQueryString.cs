using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;
using System.Web;

namespace TatBlog.WebApp.Extensions
{
  public static class GetQueryString
  {
    public static string GetURLQueryString(this object obj)
    {
      var properties = from p in obj.GetType().GetProperties()
                       where p.GetValue(obj, null) != null && p.PropertyType!= typeof(IEnumerable<SelectListItem>)
                       select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null) == null ? "" : p.GetValue(obj, null).ToString());
      if (properties.IsNullOrEmpty())
      {
        return "";
      }
      return "&" + String.Join("&", properties.ToArray());
    }
  }
}
