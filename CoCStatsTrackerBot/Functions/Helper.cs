using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCStatsTrackerBot;

public class Helper
{
    public static string CenteredString(string s, int width)
    {
        if (s.Length >= width)
        {
            return s;
        }

        int leftPadding = (width - s.Length) / 2;
        int rightPadding = width - s.Length - leftPadding;

        return new string(' ', leftPadding) + s + new string(' ', rightPadding);
    }

    public static ClanMember GetClanMember(ICollection<TrackedClan> trackedClans, string playerTag)
    {
        foreach (var clan in trackedClans)
        {
            if (clan.ClanMembers.FirstOrDefault(x => x.Tag == playerTag) != null)
            {
                return clan.ClanMembers.First(x => x.Tag == playerTag);
            }
        }

        return null;
    }
}
