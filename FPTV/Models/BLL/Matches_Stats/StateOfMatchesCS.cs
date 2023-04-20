namespace FPTV.Models.BLL.Matches_Stats
{
    /// <summary>
    /// This class is used to store the state of matches in a C# application.
    /// </summary>
    public class StateOfMatchesCS
    {
        /// <summary>
        /// Gets the matches filter by the given filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>The matches filter.</returns>
        public string? GetMatchesFilterBy(string filter)
        {
            return filter.ToLower() switch
            {
                "timetype" => null,
                "have_stats" => null,
                "live_suported" => null,
                "event_name" => null,
                _ => null,
            };

            /*
            switch (filter.ToLower())
            {
                case "timetype":
                    return null;
                    break;

                case "have_stats":
                    return null;
                    break;

                case "live_suported":
                    return null;
                    break;

                case "event_name":
                    return null;
                    break;

                default: return null;
            } 
             */
        }

        /// <summary>
        /// Gets the matches sorted by the specified sort option.
        /// </summary>
        /// <param name="sort">The sort option.</param>
        /// <returns>The sorted matches.</returns>
        public string? GetMatchesSortBy(string sort)
        {
            return sort.ToLower() switch
            {
                "event_name" => null,
                "oldest" => null,
                "newest" => null,
                _ => null,
            };

            /*
            switch (sort.ToLower())
            {
                case "event_name":
                    return null;
                    break;

                case "oldest":
                    return null;
                    break;

                case "newest":
                    return null;
                    break;

                default: return null;
            } 
             */
        }
    }
}
