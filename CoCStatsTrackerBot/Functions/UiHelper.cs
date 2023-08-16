using CoCStatsTracker;
using CoCStatsTracker.UIEntities;
using Domain.Entities;
using System.Text;

namespace CoCStatsTrackerBot;

public class UiHelper
{
    public static string MakeItStyled(string str, UiTextStyle textStyle)
    {
        switch (textStyle)
        {
            case UiTextStyle.Header:
                return $@"_*{Ecranize(str)}*_".ToUpper();
            case UiTextStyle.Subtitle:
                return $@"_*{Ecranize(str)}*_";
            case UiTextStyle.TableAnnotation:
                return $@"__*{Ecranize(str)}*__";
            case UiTextStyle.Name:
                return $@"*{Ecranize(str)}*";
            case UiTextStyle.Default:
                return Ecranize(str);
            default:
                return Ecranize($@"Text Style Error");
        }
    }

    /// <summary>
    /// Возвращает первое слово в строке
    /// </summary>
    public static string GetFirstWord(string str)
    {
        try
        {
            string[] cleaned = str.Split(new char[] { ' ' });
            return cleaned[0];
        }
        catch (Exception)
        {
            return str;
        }
    }

    /// <summary>
    /// Вовзращает центрированную по заданной ширине строку
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
    /// Пытается найти игрока с заданным тегом в отслеживаемом кланах, если такого нет - возвращает null.
    /// </summary>
    public static ClanMember? GetClanMember(ICollection<TrackedClan> trackedClans, string playerTag)
    {
        foreach (var clan in trackedClans)
        {
            if (clan.ClanMembers.FirstOrDefault(x => x.Tag == playerTag) != null)
            {
                return clan.ClanMembers.First(x => x.Tag == playerTag);
            }
        }

        Console.WriteLine("Не удалось вытянуть игрока с таким тегом, возвращаю пустого");

        return null;
    }

    /// <summary>
    /// Пытается найти активный клан с заданным тегом в отслеживаемом кланах, если такого нет - возвращает null.
    /// </summary>
    public static ClanUi? GetActiveClanUi(string clanTag, ICollection<TrackedClan> trackedClans)
    {
        var activeTrackedClans = trackedClans.Where(x => x.IsCurrent).ToList();

        var targetClan = activeTrackedClans.FirstOrDefault(x => x.Tag == clanTag);

        if (targetClan != null)
        {
            return Mapper.MapToUi(targetClan);
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Заменяет различные смайлики и т.д. в строке на символ ! с фиксированной шириной.
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

    /// <summary>
    /// Определяет максимальную длину строки для двух ячеек UI таблицы.
    /// </summary>
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

/// <summary>
/// Вспомогательный класс, определяющий длину(ширину) двух столбцов.
/// </summary>
public class UiTableMaxSize
{
    public int KeyMaxLength { get; set; }
    public int ValueMaxLength { get; set; }
}

public enum UiTextStyle
{
    Header,
    TableAnnotation,
    Name,
    Subtitle,
    Default
}
