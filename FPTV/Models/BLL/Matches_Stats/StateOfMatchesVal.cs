namespace FPTV.Models.BLL.Matches_Stats
{
    public class StateOfMatchesVal
    {
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
