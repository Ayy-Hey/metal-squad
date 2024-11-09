using System;

public class TimeUtils
{
	public static int GetCurrentTimeInSecond()
	{
		return (int)(DateTime.UtcNow - TimeUtils.Jan1st2015).TotalSeconds;
	}

	public static long GetCurrentTimeInMilisecond()
	{
		return (long)(DateTime.UtcNow - TimeUtils.Jan1st2015).TotalMilliseconds;
	}

	public static DateTime ToUtcDateTime(int timeSecond)
	{
		return TimeUtils.Jan1st2015.Add(new TimeSpan((long)timeSecond * 10000000L));
	}

	public static bool IsToday(int seconds)
	{
		int todayMidnightTimeInSecond = TimeUtils.GetTodayMidnightTimeInSecond();
		int num = todayMidnightTimeInSecond + 86400;
		return seconds > todayMidnightTimeInSecond && seconds <= num;
	}

	public static int GetTodayMidnightTimeInSecond()
	{
		return (int)(DateTime.Today - TimeUtils.Jan1st2015).TotalSeconds;
	}

	public static int GetTodayNoonTimeInSecond()
	{
		return (int)(DateTime.Today.AddHours(12.0) - TimeUtils.Jan1st2015).TotalSeconds;
	}

	private static readonly DateTime Jan1st2015 = new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Utc);
}
