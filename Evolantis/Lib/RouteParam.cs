using System.Web;

namespace Evolantis.Lib
{
    public static class RouteParam
    {
        public static string Value(this string value)
        {
            if (HttpContext.Current.Request.RequestContext.RouteData.Values[value] != null)
            {
                return HttpContext.Current.Request.RequestContext.RouteData.Values[value].ToString();
            }

            return string.Empty;
        }
    }
}
