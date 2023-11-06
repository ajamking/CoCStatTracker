using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCStatsTrackerBot.Requests;

public static class StringDevider
{
    public static string GetDividedString(this int value)
    {
        var devidedString = string.Format("{0:N}", value);

        return devidedString.Remove(devidedString.Length - 3); 
    }
}