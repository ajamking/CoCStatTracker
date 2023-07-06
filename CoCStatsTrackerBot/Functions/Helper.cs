using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        Console.WriteLine("Не удалось вытянуть игрока с таким тегом, возвращаю пустого");

        return new ClanMember();
    }

    public static string ChangeInvalidSymbols(string name)
    {
        var ebala = Regex.Matches(name, @"\W+");

        var result = name;

        foreach (Match item in ebala)
        {
            result = result.Replace(item.Value, "▯");
        }

        return result;
    }

}

public static class Extensions
{
    public static void Append<K, V>(this Dictionary<K, V> first, Dictionary<K, V> second)
    {
        List<KeyValuePair<K, V>> pairs = second.ToList();
        pairs.ForEach(pair => first.Add(pair.Key, pair.Value));
    }
}
