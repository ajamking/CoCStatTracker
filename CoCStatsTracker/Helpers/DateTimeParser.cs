using System;
using System.Globalization;

namespace CoCStatsTracker.Helpers;

public static class DateTimeParser
{
    public static DateTime Parse(string apiDateTime)
    {
        return DateTime.ParseExact(apiDateTime, "yyyyMMddTHHmmss.fffZ", CultureInfo.CurrentCulture).ToLocalTime();
    }
}

