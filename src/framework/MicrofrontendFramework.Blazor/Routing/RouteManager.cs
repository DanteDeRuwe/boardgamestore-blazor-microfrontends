using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace MicrofrontendFramework.Blazor.Routing
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
            var pageComponentTypes = _components.Where(c => c.GetCustomAttributes<RouteAttribute>(false).Any());
            Routes = pageComponentTypes.Select(GetRouteFromPageComponent).ToArray();
        }

        public MatchResult Match(string[] segments)
        {
            if (RouteParser.IsIndex(segments))
            {
                var indexRoute = Routes.SingleOrDefault(r => RouteParser.IsIndex(r.UriSegments));
                return new MatchResult(indexRoute is not null, indexRoute);
            }

            foreach (var route in Routes)
            {
                var matchResult = route.Match(segments);
                if (matchResult.IsMatch) return matchResult;
            }

            return new MatchResult(false);
        }
        
        private static Route GetRouteFromPageComponent(Type pageComponent)
        {
            var routeTemplate = pageComponent.GetCustomAttribute<RouteAttribute>(false)!.Template;
            var segments = RouteParser.GetSegments(routeTemplate);
            return new Route { UriSegments = segments, Handler = pageComponent };
        }
    }
}
