using CoCStatsTracker.UIEntities;
using System.Text;

namespace CoCStatsTrackerBot.Requests;

public static class StylingHelper
{

    /// Стилизует текст в соответствии с Telegram MarkdownV2
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

    /// Формирует гиперссылку
    public static string GetInlineLink(string text, string link)
    {
        return $@"[{text}]({Ecranize(link)})";
    }

    /// Возвращает первое слово в строке
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

    /// Центрирует строку, отбивая пробелами слева и справа.
    public static string GetCenteredString(string str, int maxStringWidth)
    {
        if (str.Length >= maxStringWidth)
        {
            return str;
        }

        int leftPadding = (maxStringWidth - str.Length) / 2;

        int rightPadding = maxStringWidth - str.Length - leftPadding;

        return new string(' ', leftPadding) + str + new string(' ', rightPadding);
    }

    /// Центрирует строку, отбивая тире слева и справа.
    public static string GetCenteredStringDash(string str, int maxStringWidth)
    {
        if (str.Length >= maxStringWidth)
        {
            return str;
        }

        int leftPadding = (maxStringWidth - str.Length) / 2;

        int rightPadding = maxStringWidth - str.Length - leftPadding;

        return new string('-', leftPadding) + str + new string('-', rightPadding);
    }

    /// Экранирует символы в строке. В ТГ разметке MarkdownV2 некоторые символы зарезервированы и их приходится экранировать
    public static string Ecranize(string str)
    {
        var reservedSymbols = @"_*,`.[]()~'><#+-/=|{}!""№;%:?*\";

        if (!str.Any(x => reservedSymbols.Contains(x)))
        {
            return str;
        }

        StringBuilder newStr = new StringBuilder(str.Length);

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

    /// Определяет максимальную длину строки для двух ячеек UI таблицы.
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

    /// Заменяет различные смайлики и т.д. в строке на символ ! с фиксированной шириной и возвращает корректно отображающееся в MarkDown таблице строку.
    public static string GetProperName(this string str, int maxStringLength)
    {
        var tempName = new StringBuilder(str.Length);

        foreach (var symbol in str)
        {
            if (char.IsDigit(symbol) ||
                char.IsWhiteSpace(symbol) ||
                char.IsPunctuation(symbol) ||
                (symbol >= 'a' && symbol <= 'z') ||
                (symbol >= 'A' && symbol <= 'Z') ||
                (symbol >= 'а' && symbol <= 'я') ||
                (symbol >= 'А' && symbol <= 'Я'))
            {
                tempName.Append(symbol);
            }
            else
            {
                tempName.Append("?");
            }
        }

        var result = tempName.ToString();

        if (result.Length > maxStringLength)
        {
            return result.Substring(0, maxStringLength);
        }
        else
        {
            return result;
        }
    }

    public static string GetDividedString(this int value)
    {
        var devidedString = string.Format("{0:N}", value);

        return devidedString.Remove(devidedString.Length - 3);
    }

    public static string GetDividedString(this double value)
    {
        var devidedString = string.Format("{0:N}", value);

        return devidedString.Remove(devidedString.Length - 3);
    }

    public static string FormateToUiDateTime(this DateTime dateTime)
    {
        var answer = $"{dateTime.ToString("dd:MM")} числа, в {dateTime.ToString("HH:mm")}";

        return answer;
    }

    public static string GetTimeLeft(this DateTime endenOn)
    {
        return $"{Math.Round(endenOn.Subtract(DateTime.Now).TotalHours, 0)}ч. {endenOn.Subtract(DateTime.Now).Minutes}м.";
    }

    public static string GetUpdatedOnString(this DateTime updatedOn)
    {
        var answer = StylingHelper.MakeItStyled($"Обновлено: {updatedOn.FormateToUiDateTime()}", UiTextStyle.Subtitle);

        return answer;
    }

    public static string GetTableDeviderLine(DeviderType deviderType, params int[] widths)
    {
        var str = new StringBuilder(widths.Sum() + widths.Count());

        str.Append(" ");

        var symbol = '-';

        switch (deviderType)
        {
            case DeviderType.Colunmn:
                {
                    foreach (var width in widths)
                    {
                        str.Append($"|{new string(symbol, width)}");
                    }

                    str.Append("|");

                    break;
                }
            case DeviderType.Dashes:
                {
                    str.Append("|");

                    foreach (var width in widths)
                    {
                        str.Append($"{new string(symbol, width + 1)}");
                    }

                    str.Remove(str.Length - 1, 1);

                    str.Append("|");

                    break;
                }
            case DeviderType.Whitespace:
                {
                    symbol = ' ';

                    foreach (var width in widths)
                    {
                        str.Append($"{new string(symbol, width + 1)}");
                    }

                    str.Append(' ');

                    break;
                }

            default:
                break;
        }

        return str.ToString();
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

public enum DeviderType
{
    Colunmn,
    Dashes,
    Whitespace
}


//Позвляет вычислить длину символа и тд. Это очень сложно адекватно применить, так что забыли. Но тут пока оставим.
public static class InlineCharCounter
{
    private static string GetProperStringV2(string str, int maxStringLength)
    {
        var result = new StringBuilder(maxStringLength);

        int currentStringLength = 0;

        foreach (var symbol in str)
        {
            var currentSynbolLength = symbol.GetLength();

            if (currentStringLength + currentSynbolLength < maxStringLength)
            {
                result.Append(symbol);

                currentStringLength += currentSynbolLength;
            }
        }

        return result.ToString();
    }

    private static bool IsFullWidthChar(this char c)
    {
        if (c >= 'ᄀ')
        {
            if (c > 'ᅟ' && c != '〈' && c != '〉' && (c < '⺀' || c > '\ua4cf' || c == '〿') && (c < '가' || c > '힣') && (c < '豈' || c > '\ufaff') && (c < '︐' || c > '︙') && (c < '︰' || c > '\ufe6f') && (c < '\uff00' || c > '｠') && (c < '￠' || c > '￦') && (c < 131072 || c > 196605))
            {
                if (c >= 196608)
                {
                    return c <= 262141;
                }

                return false;
            }

            return true;
        }

        return false;
    }

    private static int GetLength(this char c)
    {
        if (c == '\0')
        {
            return 0;
        }

        return (!IsFullWidthChar(c)) ? 1 : 2;
    }

}