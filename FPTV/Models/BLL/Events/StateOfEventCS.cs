namespace FPTV.Models.BLL.Events
{
    public class StateOfEventCS
    {
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

        public string? GetEventsSearchBy(string name)
        {
            return name.ToLower();
        }

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
