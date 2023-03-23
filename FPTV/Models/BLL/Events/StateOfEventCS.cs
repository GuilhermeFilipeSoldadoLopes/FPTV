using RestSharp;

namespace FPTV.Models.BLL.Events
{
    public static class StateOfEventCS
    {
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

		public static string? GetEventsSearchBy(string name)
        {
            return name.ToLower();
        }

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
