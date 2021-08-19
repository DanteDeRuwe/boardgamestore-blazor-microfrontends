using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MicrofrontendFramework.Blazor.Routing
{
    public static class UriParser
    {
        public static (string[] segments, Dictionary<string, object> parameters) Parse(string relativeUri)
        {
            return(GetSegments(relativeUri), ParseQueryString(relativeUri));
        }

        public static string[] GetSegments(string routeTemplate)
        {
            var routeTemplateNoLeadingSlash = routeTemplate.StartsWith("/") ? routeTemplate[1..] : routeTemplate;
            var routeTemplateNoQuerystring = RemoveQuerystring(routeTemplateNoLeadingSlash);
            return routeTemplateNoQuerystring.Split('/');
        }

        private static string RemoveQuerystring(string route)
        {
            return Regex.Replace(route, @"\?.*?$", "");
        }

        private static Dictionary<string, object> ParseQueryString(string uri)
        {
            if (!uri.Contains("?")) return new Dictionary<string, object>();

            var parameterStrings = uri[(uri.IndexOf("?", StringComparison.Ordinal) + 1)..]
                .Split('&', StringSplitOptions.RemoveEmptyEntries);

            return parameterStrings
                .Select(p => p.Split('=', StringSplitOptions.RemoveEmptyEntries))
                .ToDictionary(p => p[0], p => p[1] as object);
        }
    }
}
