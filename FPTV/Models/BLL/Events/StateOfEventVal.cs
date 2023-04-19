namespace FPTV.Models.BLL.Events
{
    /// <summary>
    /// This class is used to store the state of an event.
    /// </summary>
    public class StateOfEventVal
    {
        /// <summary>
        /// Gets the events filter by the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>The filtered events.</returns>
        public string? GetEventsFilterBy(string filter)
        {
            return filter.ToLower() switch
            {
                "timetype" => null,
                _ => null,
            };

            /*
            switch (filter.ToLower())
            {
                case "timetype":
                    return null;
                    break;

                default: return null;
            } 
             */
        }

        /// <summary>
        /// Gets the events search by the given name.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>The lowercase version of the given name.</returns>
        public string? GetEventsSearchBy(string name)
        {
            return name.ToLower();
        }

        /// <summary>
        /// Gets the events sorted by the specified sort parameter.
        /// </summary>
        /// <param name="sort">The sort parameter.</param>
        /// <returns>The sorted events.</returns>
        public string? GetEventsSortBy(string sort)
        {
            return sort.ToLower() switch
            {
                "event_name" => null,
                "oldest" => null,
                "newest" => null,
                "prize_pool" => null,
                "tier" => null,
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

                case "prize_pool":
                    return null;
                    break;

                case "tier":
                    return null;
                    break;

                default: return null;
            } 
             */
        }
    }
}

