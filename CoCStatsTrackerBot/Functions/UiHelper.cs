using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoCStatsTrackerBot;

public class UiHelper
{

    public static string GetTrimmedString(string str)
    {
        try
        {
            string[] cleaned = str.Split(new char[] { ' ' });
            return cleaned[0];
        }
        catch (Exception e)
        {
            return str;
        }
    }


    /// <summary>
    /// Позволяет центрировать строку по заданной ширине.
    /// </summary>
    public static string GetCenteredString(string s, int width)
    {
        if (s.Length >= width)
        {
            return s;
        }

        int leftPadding = (width - s.Length) / 2;

        int rightPadding = width - s.Length - leftPadding;

        return new string(' ', leftPadding) + s + new string(' ', rightPadding);
    }

    /// <summary>
    /// Метод пытается найти игрока с заданным тегом в отслеживаемом кланах, если такого нет - возвращает пустого игрока.
    /// </summary>
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

    /// <summary>
    /// Заменяет различные смайлики и т.д. в строке на символ ▯ с фиксированной шириной.
    /// </summary>
    public static string ChangeInvalidSymbols(string name)
    {
        var result = new StringBuilder(name.Length);

        foreach (var symbol in name)
        {
            if (Char.IsLetter(symbol) || Char.IsDigit(symbol))
            {
                result.Append(symbol);
            }
            else
            {
                result.Append("!");
            }
        }

        return result.ToString();
    }

    /// <summary>
    /// Экранирует символы в строке. В ТГ разметке MarkdownV2 некоторые символы зарезервированы и их приходится экранировать
    /// </summary>
    public static string Ecranize(string str)
    {
        var reservedSymbols = @"_*,`.[]()~'><#+-/=|{}!""№;%:?*\";
        StringBuilder newStr = new StringBuilder("");

        foreach (var c in str)
        {
            if (reservedSymbols.Contains(c))
            {
                newStr.Append(@"\");
                newStr.Append(c);
            }
            else
            {
                newStr.Append(c);
            }
        }

        return newStr.ToString();
    }

    public static UiTableMaxSize DefineTableMaxSize(Dictionary<string, string> dic, string firstColumnName, string secondColumnName) 
    {
        var uiTablemaxSize = new UiTableMaxSize();

        var maxKeyLength = dic.Keys.Select(x => x.Length).Max();

        var maxValueLength = dic.Values.Select(x => x.Length).Max();

        if (maxKeyLength >= firstColumnName.Length)
        {
            uiTablemaxSize.KeyMaxLength = maxKeyLength;
        }
        else
        {
            uiTablemaxSize.KeyMaxLength = firstColumnName.Length;
        }

        if (maxValueLength >= secondColumnName.Length)
        {
            uiTablemaxSize.ValueMaxLength = maxValueLength;
        }
        else
        {
            uiTablemaxSize.ValueMaxLength = secondColumnName.Length;
        }

        return uiTablemaxSize;
    }
}

public class UiTableMaxSize
{
    public int KeyMaxLength { get; set; }
    public int ValueMaxLength { get; set; }
}


public static class Extensions
{
    public static void Append<K, V>(this Dictionary<K, V> first, Dictionary<K, V> second)
    {
        List<KeyValuePair<K, V>> pairs = second.ToList();
        pairs.ForEach(pair => first.Add(pair.Key, pair.Value));
    }
}
