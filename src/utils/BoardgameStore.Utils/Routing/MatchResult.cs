namespace BoardgameStore.Utils.Routing
{
    public class MatchResult
    {
        public bool IsMatch { get; init; }
        public Route MatchedRoute { get; init; }

        public static MatchResult Match(Route matchedRoute) => new() { IsMatch = true, MatchedRoute = matchedRoute };
        public static MatchResult NoMatch() => new() { IsMatch = false, MatchedRoute = null };
    }
}
