﻿using System;
using System.Globalization;

namespace CoCStatsTracker.Items.Helpers;

public static class DateTimeParser
{
    public static DateTime ParseToDateTime(this string apiDateTime)
    {
        return DateTime.ParseExact(apiDateTime, "yyyyMMddTHHmmss.fffZ", CultureInfo.CurrentCulture).ToLocalTime();
    }
}