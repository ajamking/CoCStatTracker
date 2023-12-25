using CoCStatsTracker.UIEntities;
using System.Text;

namespace CoCStatsTrackerBot.Requests;

public class NewsLetterFunctions
{
    public static string GetRaidStartShortMessage(CapitalRaidUi raidUi)
    {
        var str = new StringBuilder();

        str.AppendLine(StylingHelper.MakeItStyled("Дни рейдов начались! Пора в бой!", UiTextStyle.Header));

        str.AppendLine(raidUi.UpdatedOn.GetUpdatedOnString());

        str.Append(StylingHelper.MakeItStyled("\nНачало рейдов:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(raidUi.StartedOn.FormateToUiDateTime(), UiTextStyle.Subtitle));
        str.Append(StylingHelper.MakeItStyled("Конец рейдов:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(raidUi.EndedOn.FormateToUiDateTime(), UiTextStyle.Subtitle));

        return str.ToString();
    }

    public static string GetRaidEndShortMessage(CapitalRaidUi raidUi)
    {
        var str = new StringBuilder();

        var offensiveReward = raidUi.OffensiveReward;

        var totalReward = offensiveReward + raidUi.DefensiveReward;

        str.AppendLine(StylingHelper.MakeItStyled("Дни рейдов окончены! Подведем итоги!", UiTextStyle.Header));

        str.AppendLine(raidUi.UpdatedOn.GetUpdatedOnString());

        str.Append(StylingHelper.MakeItStyled("\nНачало рейдов:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(raidUi.StartedOn.FormateToUiDateTime(), UiTextStyle.Subtitle));
        str.Append(StylingHelper.MakeItStyled("Конец рейдов:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(raidUi.EndedOn.FormateToUiDateTime(), UiTextStyle.Subtitle));

        var firstColumnLength = 21;
        var secondColumnLength = 9;

        str.AppendLine($"\n``` " +
                $"|{StylingHelper.GetCenteredString("Параметр", firstColumnLength)}" +
                $"|{StylingHelper.GetCenteredString("Значение", secondColumnLength)}|");

        str.AppendLine(StylingHelper.GetTableDeviderLine(DeviderType.Colunmn, firstColumnLength, secondColumnLength));

        var raidMedalsPrediction = FunctionsLogicHelper.GetCurrentRaidMedalsRewardPrediction(raidUi);

        var dic = new Dictionary<string, string>()
        {
            { "Награблено золота", $"{raidUi.TotalCapitalLoot.GetDividedString()}" },
            { "Разрушено районов", $"{raidUi.DefeatedDistrictsCount}" },
            { "Проведено атак", $"{raidUi.TotalAttacksCount}" },
            { "Атаковано кланов", $"{raidUi.DefeatedClans.Count}" },
        };

        foreach (var item in dic)
        {
            str.Append($" |{item.Key.PadRight(firstColumnLength)}|");

            str.AppendLine($"{StylingHelper.GetCenteredString(item.Value.ToString(), secondColumnLength)}|");
        }

        str.AppendLine(StylingHelper.GetTableDeviderLine(DeviderType.Whitespace, firstColumnLength, secondColumnLength));

        str.AppendLine($" |{StylingHelper.GetCenteredStringDash("Прогноз наград", firstColumnLength)}|{new string('-', secondColumnLength)}|");

        var predictDic = new Dictionary<string, string>()
        {
            { "Медали за атаку", $"{raidMedalsPrediction.OffensePrediction.GetDividedString()}" },
            { "Медали за защиту", $"{raidMedalsPrediction.DefensePrediction.GetDividedString()}" },
            { "Медалей суммарно", $"{raidMedalsPrediction.SummPrediction.GetDividedString()}" },
        };

        foreach (var item in predictDic)
        {
            str.Append($" |{item.Key.PadRight(firstColumnLength)}|");

            str.AppendLine($"{StylingHelper.GetCenteredString(item.Value.ToString(), secondColumnLength)}|");
        }

        str.AppendLine(StylingHelper.GetTableDeviderLine(DeviderType.Whitespace, firstColumnLength, secondColumnLength));

        str.AppendLine($" |{StylingHelper.GetCenteredStringDash("Фактические награды", firstColumnLength)}|{new string('-', secondColumnLength)}|");

        var lootDic = new Dictionary<string, string>()
        {
            { "Медали за атаку", $"{offensiveReward.GetDividedString()}" },
            { "Медали за защиту", $"{raidUi.DefensiveReward}" },
            { "Медалей суммарно", $"{totalReward.GetDividedString()}" },
        };

        foreach (var item in lootDic)
        {
            str.Append($" |{item.Key.PadRight(firstColumnLength)}|");

            str.AppendLine($"{StylingHelper.GetCenteredString(item.Value.ToString(), secondColumnLength)}|");
        }

        str.Append("```\n");

        return str.ToString();
    }

    public static string GetClanWarStartShortMessage(ClanWarUi currentClanWarUi)
    {
        var str = new StringBuilder();

        str.AppendLine(StylingHelper.MakeItStyled("Война началась! Пора в бой!", UiTextStyle.Header));

        str.AppendLine(StylingHelper.MakeItStyled($"{currentClanWarUi.ClanName} - {currentClanWarUi.ClanTag}\n", UiTextStyle.Name));

        str.AppendLine(currentClanWarUi.UpdatedOn.GetUpdatedOnString());

        str.Append(StylingHelper.MakeItStyled("\nНачало подготовки:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(currentClanWarUi.PreparationStartTime.FormateToUiDateTime(), UiTextStyle.Subtitle));
        str.Append(StylingHelper.MakeItStyled("Начало войны:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(currentClanWarUi.StartedOn.FormateToUiDateTime(), UiTextStyle.Subtitle));
        str.Append(StylingHelper.MakeItStyled("Конец войны:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(currentClanWarUi.EndedOn.FormateToUiDateTime(), UiTextStyle.Subtitle));

        if (Math.Round(currentClanWarUi.EndedOn.Subtract(DateTime.Now).TotalHours, 0) > 0)
        {
            str.Append(StylingHelper.MakeItStyled("\nОсталось времени до конца:  ", UiTextStyle.Default));
            str.AppendLine(StylingHelper.MakeItStyled(currentClanWarUi.EndedOn.GetTimeLeft(), UiTextStyle.Subtitle));
        }

        str.AppendLine(StylingHelper.MakeItStyled("\nИнформация о противнике:", UiTextStyle.Header));

        var firstColumnLength = 16;
        var secondColumnLength = 12;

        str.AppendLine($"``` " +
                $"|{StylingHelper.GetCenteredString("Параметр", firstColumnLength)}" +
                $"|{StylingHelper.GetCenteredString("Значение", secondColumnLength)}|");

        str.AppendLine(StylingHelper.GetTableDeviderLine(DeviderType.Colunmn, firstColumnLength, secondColumnLength));

        var dic = new Dictionary<string, string>()
        {
            { "Противник", $"{currentClanWarUi.OpponentName.GetProperName(secondColumnLength)}" },
            { "Тег противника", $"{currentClanWarUi.OpponentTag}" },
            { "Проведено КВ", $"{(currentClanWarUi.OppinentWarWins + currentClanWarUi.OppinentWarDraws + currentClanWarUi.OppinentWarLoses).GetDividedString()}" },
            { "Побед в КВ", $"{currentClanWarUi.OppinentWarWins.GetDividedString()}" },
            { "Поражений в КВ", $"{currentClanWarUi.OppinentWarLoses.GetDividedString()}" },
            { "Ничьих в КВ", $"{currentClanWarUi.OppinentWarDraws.GetDividedString()}" },
            { "Винстрик в КВ", $"{currentClanWarUi.OpponentWarWinStreak.GetDividedString()}" },
        };

        foreach (var item in dic)
        {
            str.Append($" |{item.Key.PadRight(firstColumnLength)}|");

            str.AppendLine($"{StylingHelper.GetCenteredString(item.Value.ToString(), secondColumnLength)}|");
        }

        str.Append("```");

        return str.ToString();
    }

    public static string GetClanWarEndShortMessage(ClanWarUi currentClanWarUi)
    {
        var str = new StringBuilder();

        str.AppendLine(StylingHelper.MakeItStyled("Война окончена! Подведем итоги!", UiTextStyle.Header));

        str.AppendLine(StylingHelper.MakeItStyled($"{currentClanWarUi.ClanName} - {currentClanWarUi.ClanTag}\n", UiTextStyle.Name));

        str.AppendLine(currentClanWarUi.UpdatedOn.GetUpdatedOnString());

        str.Append(StylingHelper.MakeItStyled("\nНачало подготовки:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(currentClanWarUi.PreparationStartTime.FormateToUiDateTime(), UiTextStyle.Subtitle));
        str.Append(StylingHelper.MakeItStyled("Начало войны:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(currentClanWarUi.StartedOn.FormateToUiDateTime(), UiTextStyle.Subtitle));
        str.Append(StylingHelper.MakeItStyled("Конец войны:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(currentClanWarUi.EndedOn.FormateToUiDateTime(), UiTextStyle.Subtitle));

        str.Append(StylingHelper.MakeItStyled($"\nСуммарное число атак:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled($"{currentClanWarUi.AttacksCount} : {currentClanWarUi.OpponentAttacksCount}", UiTextStyle.Subtitle));

        str.Append(StylingHelper.MakeItStyled($"Суммарно звезд:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled($"{currentClanWarUi.TotalStarsEarned} : {currentClanWarUi.OpponentStarsCount}", UiTextStyle.Subtitle));

        str.Append(StylingHelper.MakeItStyled($"Суммарный % разрушений:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled($"{Math.Round(currentClanWarUi.DestructionPercentage, 1)}% : " +
            $"{Math.Round(currentClanWarUi.OpponentDestructionPercentage, 1)}%", UiTextStyle.Subtitle));

        str.Append(StylingHelper.MakeItStyled($"\nРезультат:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled($"{currentClanWarUi.Result}", UiTextStyle.Subtitle));

        if (currentClanWarUi.NonAttackersCw.Count != currentClanWarUi.MembersResults.Count && currentClanWarUi.NonAttackersCw.Count != 0)
        {
            str.AppendLine(StylingHelper.MakeItStyled("ПРОВЕЛИ НЕ ВСЕ АТАКИ: ", UiTextStyle.Subtitle));
            str.AppendLine(StylingHelper.MakeItStyled("Формат: Игрок  ﴾ Атак проведено ﴿\n", UiTextStyle.Default));

            foreach (var nonAttacker in currentClanWarUi.NonAttackersCw)
            {
                var telegramUserName = "";

                if (!string.IsNullOrEmpty(nonAttacker.TelegramUserName))
                {
                    telegramUserName = nonAttacker.TelegramUserName;
                }

                str.AppendLine(StylingHelper.MakeItStyled($"{nonAttacker.Name} ﴾ {nonAttacker.AttacksCount} ﴿ {telegramUserName}", UiTextStyle.Name));
            }
        }

        return str.ToString();
    }
}