namespace FPTV.Models.BLL.Events
{
    /// <summary>
    /// 
    /// </summary>
    public class StateOfEventVal
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public string? GetEventsFilterBy(string filter)
        {
            switch (filter.ToLower())
            {
                case "timetype":
                    return null;
                    break;

                default: return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string? GetEventsSearchBy(string name)
        {
            return name.ToLower();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public string? GetEventsSortBy(string sort)
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

                case "prize_pool":
                    return null;
                    break;

                case "tier":
                    return null;
                    break;

                default: return null;
            }
        }
    }
}

