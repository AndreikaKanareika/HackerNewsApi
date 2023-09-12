namespace HackerNews.Api.Helpers;

public static class DateTimeHelper
{
    public static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        return dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
    }
}
