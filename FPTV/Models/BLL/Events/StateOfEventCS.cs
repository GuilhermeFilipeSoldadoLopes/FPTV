using RestSharp;

namespace FPTV.Models.BLL.Events
{
    /// <summary>
    /// This class provides a set of constants representing the state of an event.
    /// </summary>
    public static class StateOfEventCS
    {
        /// <summary>
        /// Gets the events filter by.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// The events filter by.
        /// </returns>
        public static string? GetEventsFilterBy(string filter)
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
        public static string? GetEventsSearchBy(string name)
        {
            return name.ToLower();
        }

        /// <summary>
        /// Returns a string based on the sort parameter.
        /// </summary>
        /// <param name="sort">The sort parameter.</param>
        /// <returns>A string based on the sort parameter.</returns>
        public static string? GetEventsSortBy(string sort)
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

            /**
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
