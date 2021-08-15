using System;
using System.Linq;

namespace BoardgameStore.Client.Routing
{
    public class Route
    {
        public string[] UriSegments { get; set; }
        public Type Handler { get; set; }

        public MatchResult Match(string[] segments)
        {
            if (segments.Length != UriSegments.Length)
            {
                return MatchResult.NoMatch();
            }

            return UriSegments
                .Where((t, i) => string.Compare(segments[i], t, StringComparison.OrdinalIgnoreCase) != 0)
                .Any()
                ? MatchResult.NoMatch()
                : MatchResult.Match(this);
        }
    }
}
