namespace System;

internal static class DateTimeExtensions
{
    public static string ToFriendlyDeltaStringNow(this DateTime time)
    {
        return time.ToFriendlyDeltaString(DateTime.Now);
    }

    public static string ToFriendlyDeltaStringUtcNow(this DateTime time)
    {
        return time.ToFriendlyDeltaString(DateTime.UtcNow);
    }

    public static string ToFriendlyDeltaString(this DateTime time, DateTime baseTime)
    {
        var secondsTimeSpan = baseTime - time;
        return (int)secondsTimeSpan.TotalSeconds switch
        {
            < 30 => $"a few seconds ago",
            < 60 => $"{(int)secondsTimeSpan.TotalSeconds} seconds ago",
            < 120 => "1 minute ago",
            < 60 * 60 => $"{(int)secondsTimeSpan.TotalMinutes} minutes ago",
            < 60 * 60 * 2 => "1 hour ago",
            < 60 * 60 * 24 => $"{(int)secondsTimeSpan.TotalHours} hours ago",
            < 60 * 60 * 24 * 2 => "1 day ago",
            < 60 * 60 * 24 * 7 => $"{(int)secondsTimeSpan.TotalDays} days ago",
            < 60 * 60 * 24 * 7 * 2 => "1 week ago",
            < 60 * 60 * 24 * 30 => $"{(int)(secondsTimeSpan.TotalDays / 7)} weeks ago",
            < 60 * 60 * 24 * 30 * 2 => "1 month ago",
            < 60 * 60 * 24 * 365 => $"{(int)(secondsTimeSpan.TotalDays / 30)} months ago",
            < 60 * 60 * 24 * 365 * 2 => "1 year ago",
            _ => $"{(int)(secondsTimeSpan.TotalDays / 365)} years ago"
        };
    }
}
