namespace FPTV.Models.BLL.Matches_Stats
{
    /// <summary>
    /// 
    /// </summary>
    public class StateOfMatchesVal
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public string? GetMatchesFilterBy(string filter)
        {
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public string? GetMatchesSortBy(string sort)
        {
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
        }
    }
}
