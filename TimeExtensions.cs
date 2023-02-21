// https://stackoverflow.com/a/35750677
public static class TimeExtensions
{
    public static TimeSpan StripSeconds(this TimeSpan time)
    {
        return new TimeSpan(time.Hours, time.Minutes, 0);
    }
}