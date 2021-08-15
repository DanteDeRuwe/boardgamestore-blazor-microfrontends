using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using BoardgameStore.Utils;
using Microsoft.AspNetCore.Components;

namespace BoardgameStore.Client.Routing
{
    public class RouteManager
    {
        private readonly ComponentCollection _components;
        public Route[] Routes { get; set; }

        public RouteManager(ComponentCollection components)
        {
            _components = components;
        }

        public void Initialize()
        {
            var pageComponentTypes = _components
                .Select(c => c.Value)
                .Where(c => c.GetCustomAttributes(typeof(RouteAttribute), false).Length > 0);

            Routes = pageComponentTypes
                .Select(pageType =>
                {
                    var routeTemplate = pageType.GetCustomAttribute<RouteAttribute>()!.Template;
                    routeTemplate = routeTemplate.StartsWith("/") ? routeTemplate[1..] : routeTemplate;
                    routeTemplate =
                        Regex.Replace(routeTemplate, @"\?.*?$", ""); //TODO supports only querystrings for now
                    return new Route
                    {
                        UriSegments = routeTemplate.Split('/'),
                        Handler = pageType
                    };
                })
                .ToArray();
        }

        public MatchResult Match(string[] segments)
        {
            if (segments.Length == 0)
            {
                var indexRoute = Routes.SingleOrDefault(x => x.UriSegments.SequenceEqual(new[] { "" }));
                return indexRoute is not null ? MatchResult.Match(indexRoute) : MatchResult.NoMatch();
            }

            foreach (var route in Routes)
            {
                var matchResult = route.Match(segments);
                if (matchResult.IsMatch) return matchResult;
            }

            return MatchResult.NoMatch();
        }
    }
}
