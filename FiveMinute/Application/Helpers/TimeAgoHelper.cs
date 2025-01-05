namespace FiveMinute.Application.Helpers
{
	public static class TimeAgoHelper
	{

		public static string GetTimeAgoString(DateTime dateTime, TimeZoneInfo timeZone)
		{
			var localTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZone);
			var timeSpan = DateTime.Now - localTime;

			if (timeSpan.TotalSeconds < 60)
				return $"{(int)timeSpan.TotalSeconds} секунд назад";
			else if (timeSpan.TotalMinutes < 60)
				return $"{(int)timeSpan.TotalMinutes} минут назад";
			else if (timeSpan.TotalHours < 24)
				return $"{(int)timeSpan.TotalHours} час{(timeSpan.TotalHours < 5 ? "" : "ов")} назад";
			else if (timeSpan.TotalDays < 7)
				return $"{(int)timeSpan.TotalDays} день{(timeSpan.TotalDays < 2 ? "" : "ей")} назад";
			else if (timeSpan.TotalDays < 30)
				return $"{(int)(timeSpan.TotalDays / 7)} недел{((int)(timeSpan.TotalDays / 7) == 1 ? "ю" : "ь")} назад";
			else if (timeSpan.TotalDays < 365)
				return $"{(int)(timeSpan.TotalDays / 30)} месяц{((int)(timeSpan.TotalDays / 30) == 1 ? "" : "ев")} назад";
			else
				return $"{(int)(timeSpan.TotalDays / 365)} год{((int)(timeSpan.TotalDays / 365) == 1 ? "" : "а")} назад";
		}

	}
}
