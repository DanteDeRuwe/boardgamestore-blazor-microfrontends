using System;
using System.Linq;

namespace MicrofrontendFramework.Blazor.Routing
{
    public class Route
    {
        public string[] UriSegments { get; set; }
        public Type Handler { get; set; }

        public MatchResult Match(string[] segments)
        {
            var isMatch = segments.Length == UriSegments.Length && !UriSegments
                .Where((t, i) => string.Compare(segments[i], t, StringComparison.OrdinalIgnoreCase) != 0)
                .Any();
            
            return new MatchResult(isMatch, isMatch ? this : null);
        }
    }
}
