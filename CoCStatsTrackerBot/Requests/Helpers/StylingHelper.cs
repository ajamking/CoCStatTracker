using System.Text;

namespace CoCStatsTrackerBot.Requests;

public static class StylingHelper
{
    /// <summary>
    /// Стилизует текст в соответствии с Telegram MarkdownV2
    /// </summary>
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
    /// Формирует гиперссылку
    /// </summary>
    public static string GetInlineLink(string text, string link)
    {
        return $@"[{text}]({Ecranize(link)})";
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
    /// Центрирует строку, отбивая пробелами слева и справа.
    /// </summary>
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

    /// <summary>
    /// Заменяет различные смайлики и т.д. в строке на символ ! с фиксированной шириной и возвращает корректно отображающееся в MarkDown таблице строку.
    /// </summary>
    public static string GetProperString(string str, int maxStringLength)
    {
        var tempName = new StringBuilder(str.Length);

        var haveInvalidSymbols = false;

        foreach (var symbol in str)
        {
            var ab = char.IsSurrogate(symbol);

            if ((char.IsLetterOrDigit(symbol) || char.IsWhiteSpace(symbol) || char.IsPunctuation(symbol)) && symbol != '\\')
            {
                tempName.Append(symbol);
            }
            else
            {
                haveInvalidSymbols = true;

                tempName.Append("?");
            }
        }

        var result = tempName.ToString();

        if (haveInvalidSymbols)
        {
            result = result.Substring(0, result.Length - 2);
        }

        if (result.Length > maxStringLength)
        {
            return result.Substring(0, maxStringLength);
        }
        else
        {
            return result;
        }
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
