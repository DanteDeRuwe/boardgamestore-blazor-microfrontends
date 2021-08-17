using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MicrofrontendFramework.Blazor.Routing
{
    public static class RouteParser
    {
        public static string[] GetSegments(string routeTemplate)
        {
            var routeTemplateNoLeadingSlash = routeTemplate.StartsWith("/") ? routeTemplate[1..] : routeTemplate;
            var routeTemplateNoQuerystring = RemoveQuerystring(routeTemplateNoLeadingSlash);
            return routeTemplateNoQuerystring.Split('/');
        }

        public static string RemoveQuerystring(string routeTemplateNoLeadingSlash)
        {
            return Regex.Replace(routeTemplateNoLeadingSlash, @"\?.*?$", "");
        }
        
        public static bool IsIndex(IReadOnlyCollection<string> segments)
        {
            return segments.Count == 0 || segments.SequenceEqual(new[] { "" });
        }
    }
}
