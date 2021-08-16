using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace BoardgameStore.Utils.Routing
{
    public class DynamicRouter : IComponent, IHandleAfterRender, IDisposable
    {
        private RenderHandle _renderHandle;
        private bool _navigationInterceptionEnabled;
        private string _location;

        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public INavigationInterception NavigationInterception { get; set; }
        [Inject] public RouteManager RouteManager { get; set; }

        [Parameter] public RenderFragment NotFound { get; set; }
        [Parameter] public RenderFragment<RouteData> Found { get; set; }


        // Make sure we can implement the IComponent interface
        public void Attach(RenderHandle renderHandle)
        {
            _renderHandle = renderHandle;
            _location = NavigationManager.Uri;
            NavigationManager.LocationChanged += HandleLocationChanged;
        }

        // Make sure we can implement the IComponent interface
        public Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            if (Found is null)
            {
                throw new InvalidOperationException(
                    $"The {nameof(DynamicRouter)} component requires a value for the parameter {nameof(Found)}.");
            }

            if (NotFound is null)
            {
                throw new InvalidOperationException(
                    $"The {nameof(DynamicRouter)} component requires a value for the parameter {nameof(NotFound)}.");
            }

            RouteManager.Initialize();
            Refresh();

            return Task.CompletedTask;
        }


        //Set up navigation interception
        public Task OnAfterRenderAsync()
        {
            if (_navigationInterceptionEnabled) return Task.CompletedTask;

            _navigationInterceptionEnabled = true;
            return NavigationInterception.EnableNavigationInterceptionAsync();
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= HandleLocationChanged;
        }

        private void HandleLocationChanged(object sender, LocationChangedEventArgs args)
        {
            _location = args.Location;
            Refresh();
        }

        private void Refresh()
        {
            var relativeUri = NavigationManager.ToBaseRelativePath(_location);
            var parameters = ParseQueryString(relativeUri);

            var segments = Regex.Replace(relativeUri, @"\?.*?$", "")
                .Trim()
                .Split('/', StringSplitOptions.RemoveEmptyEntries);
            
            var matchResult = RouteManager.Match(segments);

            if (!matchResult.IsMatch)
            {
                _renderHandle.Render(NotFound);
                return;
            }

            var routeData = new RouteData(matchResult.MatchedRoute.Handler, parameters);

            _renderHandle.Render(Found(routeData));
        }

        private Dictionary<string, object> ParseQueryString(string uri)
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
