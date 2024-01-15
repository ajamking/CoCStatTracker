using Microsoft.CodeAnalysis.CSharp;
using System.Text;

namespace CoCStatsTrackerBot.Requests;

public static class StylingHelper
{
    /// Стилизует текст в соответствии с Telegram MarkdownV2
    public static string MakeItStyled(string str, UiTextStyle textStyle)
    {
        return textStyle switch
        {
            UiTextStyle.Header => $@"_*{Ecranize(str)}*_".ToUpper(),
            UiTextStyle.Subtitle => $@"_*{Ecranize(str)}*_",
            UiTextStyle.TableAnnotation => $@"__*{Ecranize(str)}*__",
            UiTextStyle.Name => $@"*{Ecranize(str)}*",
            UiTextStyle.Default => Ecranize(str),
            _ => Ecranize($@"Text Style Error"),
        };
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
        try
        {
            var newStr = SymbolDisplay.FormatLiteral(str, false);

            //if (newStr.Length >= maxStringWidth)
            //{
            //    return str;
            //}

            int leftPadding = (maxStringWidth - str.Length) / 2;

            int rightPadding = maxStringWidth - str.Length - leftPadding;

            var answer = new string(' ', leftPadding) + newStr + new string(' ', rightPadding);

            return answer;
        }
        catch (Exception e)
        {
            return "Exc";
        }
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

        StringBuilder newStr = new(str.Length);

        foreach (var c in str)
        {
            if (reservedSymbols.Contains(c))
            {
                newStr.Append('\\');
                newStr.Append(c);
            }
            else
            {
                newStr.Append(c);
            }
        }

        return newStr.ToString();
    }

    /// Заменяет различные смайлики и т.д. в строке на символ ! с фиксированной шириной и возвращает корректно отображающееся в MarkDown таблице строку.
    public static string GetProperName(this string str, int maxStringLength)
    {
        var tempName = new StringBuilder();

        foreach (var symbol in SymbolDisplay.FormatLiteral(str, false))
        {
            if (char.IsDigit(symbol) ||
                char.IsWhiteSpace(symbol) ||
                char.IsPunctuation(symbol) ||
                (symbol >= 'a' && symbol <= 'z') ||
                (symbol >= 'A' && symbol <= 'Z') ||
                (symbol >= 'а' && symbol <= 'я') ||
                (symbol >= 'А' && symbol <= 'Я'))
            {
                //if (symbol == '\\')
                //{
                //    tempName.Append($" {symbol}");
                //}
                //else
                //{
                //    tempName.Append(symbol);
                //}

                tempName.Append(symbol);
            }
            else
            {
                tempName.Append('?');
            }
        }

        var result = tempName.ToString();

        if (result.Length > maxStringLength)
        {
            return result[..maxStringLength];
        }
        else
        {
            return result;
        }
    }

    /// Заменяет различные смайлики и т.д. в строке на символ ! с фиксированной шириной и возвращает корректно отображающееся в Telegram таблице строку.
    public static string RemoveInvalidSymbols(this string str)
    {
        var tempName = new StringBuilder();

        foreach (var symbol in SymbolDisplay.FormatLiteral(str, false))
        {
            if (symbol == '⃢')
            {
                tempName.Append('?');
            }
            else
            {
                tempName.Append(symbol);
            }
        }

        return tempName.ToString();
    }

    public static string GetDividedString(this int value)
    {
        var devidedString = string.Format("{0:N}", value);

        return devidedString.Split(',')[0];
    }

    public static string GetDividedString(this double value)
    {
        var devidedString = string.Format("{0:N}", value);

        return devidedString.Split(',')[0];
    }

    public static string FormateToUiDateTime(this DateTime dateTime)
    {
        var answer = $"{dateTime:dd\\ MMM} в {dateTime:HH:mm}";

        return answer;
    }

    public static string GetUpdatedOnString(this DateTime updatedOn)
    {
        var answer = MakeItStyled($"Обновлено:  ", UiTextStyle.Default);

        answer += MakeItStyled($"{updatedOn.FormateToUiDateTime()}", UiTextStyle.Subtitle);

        return answer;
    }

    public static string GetTableDeviderLine(DeviderType deviderType, params int[] widths)
    {
        var str = new StringBuilder(widths.Sum() + widths.Length);

        str.Append(' ');

        var symbol = '-';

        switch (deviderType)
        {
            case DeviderType.Colunmn:
                {
                    foreach (var width in widths)
                    {
                        str.Append($"|{new string(symbol, width)}");
                    }

                    str.Append('|');

                    break;
                }
            case DeviderType.Dashes:
                {
                    str.Append('|');

                    foreach (var width in widths)
                    {
                        str.Append($"{new string(symbol, width + 1)}");
                    }

                    str.Remove(str.Length - 1, 1);

                    str.Append('|');

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

    public static int ReturnZeroIfLess(this int param)
    {
        if (param < 0)
        {
            return 0;
        }

        return param;
    }
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