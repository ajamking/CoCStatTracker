using CoCStatsTracker.ApiEntities;
using CoCStatsTracker.UIEntities;
using System.ComponentModel;
using System.Text;

namespace CoCStatsTrackerBot.Requests;

public class CurrentStatisticsFunctions
{
    public static string GetCurrentWarShortInfo(CwCwlUi currentClanWarUi)
    {
        var str = new StringBuilder();

        str.AppendLine(StylingHelper.MakeItStyled("Общая информация о последней войне клана", UiTextStyle.Header));
        str.AppendLine(StylingHelper.MakeItStyled(currentClanWarUi.ClanName + " - " + currentClanWarUi.ClanTag, UiTextStyle.Name));
        str.AppendLine();

        str.AppendLine(GetWarMainInfoHat(currentClanWarUi));

        if (currentClanWarUi.NonAttackersCw.Count != currentClanWarUi.MembersResults.Count && currentClanWarUi.NonAttackersCw.Count != 0)
        {
            str.AppendLine(StylingHelper.MakeItStyled("Игроки, проведшие не все атаки:", UiTextStyle.Subtitle));

            foreach (var nonAttacker in currentClanWarUi.NonAttackersCw)
            {
                var telegramUserName = "";

                if (!string.IsNullOrEmpty(nonAttacker.TelegramUserName))
                {
                    telegramUserName = nonAttacker.TelegramUserName;
                }

                str.AppendLine(StylingHelper.MakeItStyled($"{nonAttacker.Name} Атак: {nonAttacker.AttacksCount} {telegramUserName}", UiTextStyle.Name));
            }
        }

        return str.ToString();
    }

    public static string GetCurrentWarMap(WarMapUi warMapUi)
    {
        var maxNameLength = 14;

        var warMembers = warMapUi.WarMembers.OrderBy(x => x.MapPosition).ToList();

        var enemyWarMembers = warMapUi.EnemyWarMembers.OrderBy(x => x.MapPosition).ToList();

        var str = new StringBuilder();

        str.AppendLine(StylingHelper.MakeItStyled("Карта текущей войны клана", UiTextStyle.Header));
        str.AppendLine(StylingHelper.MakeItStyled(warMapUi.ClanName + " - " + warMapUi.ClanTag, UiTextStyle.Name));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Противник:", UiTextStyle.Subtitle));
        str.AppendLine(StylingHelper.MakeItStyled(warMapUi.OpponentClanName + " - " + warMapUi.OpponentClanTag, UiTextStyle.Name));
        str.AppendLine();
        str.AppendLine(warMapUi.UpdatedOn.GetUpdatedOnString());
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Начало подготовки:", UiTextStyle.Subtitle));
        str.AppendLine(StylingHelper.MakeItStyled(warMapUi.PreparationStartTime.FormateToUiDateTime(), UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("Начало войны:", UiTextStyle.Subtitle));
        str.AppendLine(StylingHelper.MakeItStyled(warMapUi.StartedOn.FormateToUiDateTime(), UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("Конец войны:", UiTextStyle.Subtitle));
        str.AppendLine(StylingHelper.MakeItStyled(warMapUi.EndedOn.FormateToUiDateTime(), UiTextStyle.Default));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Карта войны:", UiTextStyle.Subtitle));
        str.AppendLine($@"``` ");

        for (int i = 0; i < warMembers.Count; i++)
        {
            var mate = warMembers[i];

            var opponent = enemyWarMembers[i];

            var properMateName = StylingHelper.GetCenteredString(StylingHelper.GetProperName(mate.Name, maxNameLength), maxNameLength);

            var properOpponentName = StylingHelper.GetProperName(StylingHelper.GetCenteredString(opponent.Name, maxNameLength), maxNameLength);

            var position = StylingHelper.GetCenteredString((i + 1).ToString(), 2);

            var membersThLevel = mate.TownHallLevel.ToString();

            if (mate.TownHallLevel < 10)
            {
                membersThLevel += " ";
            }

            var opponentThLevel = opponent.TownHallLevel.ToString();

            if (opponent.TownHallLevel < 10)
            {
                opponentThLevel += " ";
            }

            var mapStr = $@"{properMateName} {membersThLevel} |{position}| {opponent.TownHallLevel} {properOpponentName}";

            str.AppendLine($@"{StylingHelper.Ecranize(mapStr)}");
        }

        str.AppendLine($@"```");

        return str.ToString();
    }

    public static string GetCurrentRaidShortInfo(RaidUi raidsUi)
    {
        var str = new StringBuilder();

        str.AppendLine(StylingHelper.MakeItStyled("Общая информация о последнем рейде клана", UiTextStyle.Header));
        str.AppendLine(StylingHelper.MakeItStyled(raidsUi.ClanName + " - " + raidsUi.ClanTag, UiTextStyle.Name));
        str.AppendLine();
        str.Append(GetRaidsMainInfoHat(raidsUi));

        if (raidsUi.NonAttackersRaids.Count != 0)
        {
            str.AppendLine(StylingHelper.MakeItStyled("ПРОВЕЛИ НЕ ВСЕ АТАКИ:", UiTextStyle.Subtitle));
            str.AppendLine();

            foreach (var nonAttacker in raidsUi.NonAttackersRaids)
            {
                var telegramUserName = "";

                if (!string.IsNullOrEmpty(nonAttacker.TelegramUserName))
                {
                    telegramUserName = nonAttacker.TelegramUserName;
                }

                str.AppendLine(StylingHelper.MakeItStyled($"{nonAttacker.Name} Атак: {nonAttacker.AttacksCount} {telegramUserName}", UiTextStyle.Name));
            }
        }

        return str.ToString();
    }

    public static string GetDistrictStatistics(RaidUi raidUi, DistrictType districtType)
    {
        var maxNameLength = 18;
        var max2ColumnLength = 3;
        var max3ColumnLength = 3;

        var chosenDistrictName = FunctionsLogicHelper.AllDistricts.First(x => x.Key == districtType).Value;

        var avgPercent = 0.0;
        var counter = 0;

        foreach (var clan in raidUi.DefeatedClans)
        {
            foreach (var district in clan.DefeatedEmemyDistricts.Where(x => x.Name == chosenDistrictName))
            {
                foreach (var attack in district.Attacks)
                {
                    avgPercent += attack.DestructionPercentTo - attack.DestructionPercentFrom;
                    counter++;
                }
            }
        }

        if (counter is not 0)
        {
            avgPercent = Math.Round(avgPercent / counter, 2);
        }

        var str = new StringBuilder();

        str.AppendLine(StylingHelper.MakeItStyled("Показатели игроков клана", UiTextStyle.Header));
        str.AppendLine(StylingHelper.MakeItStyled(raidUi.ClanName + " - " + raidUi.ClanTag, UiTextStyle.Name));
        str.AppendLine();
        str.Append(StylingHelper.MakeItStyled("В атаках на район: ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled($"{chosenDistrictName}", UiTextStyle.Header));
        str.AppendLine();
        str.AppendLine(raidUi.UpdatedOn.GetUpdatedOnString());
        str.AppendLine();
        str.Append(StylingHelper.MakeItStyled("Разрушений за атаку в среднем: ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled($"{avgPercent}%", UiTextStyle.Name));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Показатели атак:", UiTextStyle.Subtitle));

        str.AppendLine($"``` " +
                           $"|{StylingHelper.GetCenteredString("Игрок", maxNameLength)}" +
                           $"|{StylingHelper.GetCenteredString("%От", max2ColumnLength)}" +
                           $"|{StylingHelper.GetCenteredString("%До", max3ColumnLength)}|");

        str.AppendLine($" " +
            $"|{new string('-', maxNameLength)}" +
            $"|{new string('-', max2ColumnLength)}" +
            $"|{new string('-', max3ColumnLength)}|");

        var tableWidth = maxNameLength + max2ColumnLength + max3ColumnLength + 2;

        str.AppendLine($" {StylingHelper.GetCenteredString(" ", tableWidth + 2)}");

        foreach (var clan in raidUi.DefeatedClans)
        {
            str.AppendLine($" |{StylingHelper.GetCenteredStringDash(StylingHelper.GetProperName(clan.Name, maxNameLength), tableWidth)}|");

            foreach (var district in clan.DefeatedEmemyDistricts.Where(x => x.Name == chosenDistrictName))
            {
                foreach (var attack in district.Attacks)
                {
                    var properName = StylingHelper.GetProperName(attack.AttackerName, maxNameLength);

                    str.Append($" |{StylingHelper.GetCenteredString(properName, maxNameLength)}|");

                    str.Append($"{StylingHelper.GetCenteredString(attack.DestructionPercentFrom.ToString(), max2ColumnLength)}|");

                    str.AppendLine($"{StylingHelper.GetCenteredString(attack.DestructionPercentTo.ToString(), max3ColumnLength)}|");
                }

                str.AppendLine($" {StylingHelper.GetCenteredString(" ", tableWidth + 2)}");
            }
        }

        str.Append("```\n");

        return str.ToString();
    }

    public static string GetRaidsMainInfoHat(RaidUi raidUi)
    {
        var str = new StringBuilder();

        var offensiveReward = raidUi.OffensiveReward;

        var totalReward = offensiveReward + raidUi.DefensiveReward;

        str.AppendLine(raidUi.UpdatedOn.GetUpdatedOnString());

        str.Append(StylingHelper.MakeItStyled("\nНачало рейдов:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(raidUi.StartedOn.FormateToUiDateTime(), UiTextStyle.Subtitle));
        str.Append(StylingHelper.MakeItStyled("Конец рейдов:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(raidUi.EndedOn.FormateToUiDateTime(), UiTextStyle.Subtitle));

        var firstColumnLength = 18;
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
            { "Медали за атаку", $"{offensiveReward.GetDividedString()}" },
            { "Медали за защиту", $"{raidUi.DefensiveReward}" },
            { "Медалей суммарно", $"{totalReward.GetDividedString()}" },
            { "Прогноз за атаку", $"{raidMedalsPrediction.OffensePrediction.GetDividedString()}" },
            { "Прогноз за защиту", $"{raidMedalsPrediction.DefensePrediction.GetDividedString()}" },
            { "Прогноз суммарно", $"{raidMedalsPrediction.SummPrediction.GetDividedString()}" },
        };

        foreach (var item in dic)
        {
            str.Append($" |{item.Key.PadRight(firstColumnLength)}|");

            str.AppendLine($"{StylingHelper.GetCenteredString(item.Value.ToString(), secondColumnLength)}|");
        }

        str.Append("```");

        str.AppendLine(StylingHelper.MakeItStyled("\nПояснение таблицы:", UiTextStyle.TableAnnotation));
        str.AppendLine(StylingHelper.MakeItStyled("Атаки - число проведенных атак;", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("Р-н - количество полностью уничтоженных районов;", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("Золото - суммарно награбленное золото;", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("Защиты/Атаки ~123 - среднее количество атак, затрачиваемых на полное уничтожение клана.\n", UiTextStyle.Default));

        var maxClanNameLength = 10;
        var maxAttackLength = 5;
        var maxDDCLength = 3;
        var maxGoldLootedLength = 7;

        var tableLength = maxClanNameLength + maxAttackLength + maxDDCLength + maxGoldLootedLength + 2;

        str.AppendLine($"``` " +
                 $"|{StylingHelper.GetCenteredString("Клан", maxClanNameLength)}" +
                 $"|{StylingHelper.GetCenteredString("Aтаки", maxAttackLength)}" +
                 $"|{StylingHelper.GetCenteredString("Р-н", maxDDCLength)}" +
                 $"|{StylingHelper.GetCenteredString("Золото", maxGoldLootedLength)}|");

        str.AppendLine(StylingHelper.GetTableDeviderLine(DeviderType.Colunmn, maxClanNameLength, maxAttackLength, maxDDCLength, maxGoldLootedLength));

        str.AppendLine(StylingHelper.GetTableDeviderLine(DeviderType.Whitespace, maxClanNameLength, maxAttackLength, maxDDCLength, maxGoldLootedLength));

        var totalLoseDefenses = raidUi.Defenses
            .Where(x => x.DestroyedFriendlyDistrictsCount == FunctionsLogicHelper.AllDistricts.Count);

        var averageDefenses = Math.Round((double)totalLoseDefenses
            .Sum(x => x.TotalAttacksCount)
            / totalLoseDefenses.Count(), 2);

        str.AppendLine($" |{StylingHelper.GetCenteredStringDash($" Защиты ~{averageDefenses} ", tableLength + 1)}|");

        str.AppendLine(StylingHelper.GetTableDeviderLine(DeviderType.Dashes, maxClanNameLength, maxAttackLength, maxDDCLength, maxGoldLootedLength));

        foreach (var defense in raidUi.Defenses.OrderByDescending(x => x.TotalAttacksCount))
        {
            var properName = StylingHelper.GetProperName(defense.EnemyClanName, maxClanNameLength);

            str.Append($" |{StylingHelper.GetCenteredString(properName, maxClanNameLength)}|");

            str.Append($"{StylingHelper.GetCenteredString(defense.TotalAttacksCount.ToString(), maxAttackLength)}|");

            str.Append($"{StylingHelper.GetCenteredString(defense.DestroyedFriendlyDistrictsCount.ToString(), maxDDCLength)}|");

            str.AppendLine($"{StylingHelper.GetCenteredString(defense.TotalEnemyLoot.GetDividedString().ToString(), maxGoldLootedLength)}|");
        }

        str.AppendLine($" {StylingHelper.GetCenteredString(" ", tableLength)}");

        var totalDefeatedClans = raidUi.DefeatedClans
            .Where(x => x.DefeatedEmemyDistricts.Count == FunctionsLogicHelper.AllDistricts.Count);

        var averageAttacks = Math.Round((double)totalDefeatedClans
            .Sum(x => x.TotalAttacksCount)
            / totalDefeatedClans.Count(), 2);

        str.AppendLine($" |{StylingHelper.GetCenteredStringDash($" Атаки ~{averageAttacks} ", tableLength + 1)}|");

        str.AppendLine(StylingHelper.GetTableDeviderLine(DeviderType.Dashes, maxClanNameLength, maxAttackLength, maxDDCLength, maxGoldLootedLength));

        foreach (var defeatedClan in raidUi.DefeatedClans.OrderByDescending(x => x.TotalAttacksCount))
        {
            var destroyedDistrictsCount = 0;

            destroyedDistrictsCount = defeatedClan.DefeatedEmemyDistricts.Where(x => x.Attacks.Any(x => x.DestructionPercentTo == 100)).Count();

            var properName = StylingHelper.GetProperName(defeatedClan.Name, maxClanNameLength);

            str.Append($" |{StylingHelper.GetCenteredString(properName, maxClanNameLength)}|");

            str.Append($"{StylingHelper.GetCenteredString(defeatedClan.TotalAttacksCount.ToString(), maxAttackLength)}|");

            str.Append($"{StylingHelper.GetCenteredString(destroyedDistrictsCount.ToString(), maxDDCLength)}|");

            str.AppendLine($"{StylingHelper.GetCenteredString(defeatedClan.TotalLoot.GetDividedString().ToString(), maxGoldLootedLength)}|");
        }

        str.Append("```\n");

        return str.ToString();
    }

    public static string GetWarMainInfoHat(CwCwlUi cw)
    {
        var str = new StringBuilder();

        str.AppendLine(cw.UpdatedOn.GetUpdatedOnString());


        str.AppendLine(StylingHelper.MakeItStyled("\nНачало подготовки:", UiTextStyle.Subtitle));
        str.AppendLine(StylingHelper.MakeItStyled(cw.PreparationStartTime.FormateToUiDateTime(), UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("Начало войны:", UiTextStyle.Subtitle));
        str.AppendLine(StylingHelper.MakeItStyled(cw.StartedOn.FormateToUiDateTime(), UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("Конец войны:", UiTextStyle.Subtitle));
        str.AppendLine(StylingHelper.MakeItStyled(cw.EndedOn.FormateToUiDateTime(), UiTextStyle.Default));

        str.AppendLine(StylingHelper.MakeItStyled($"\nРезультат: {cw.Result}", UiTextStyle.Subtitle));

        str.AppendLine(StylingHelper.MakeItStyled($"\nСуммарное число атак: {cw.AttacksCount} : {cw.OpponentAttacksCount}", UiTextStyle.Subtitle));
        str.AppendLine(StylingHelper.MakeItStyled($"Суммарно звезд: {cw.TotalStarsEarned} : {cw.OpponentStarsCount}", UiTextStyle.Subtitle));
        str.AppendLine(StylingHelper.MakeItStyled($"Суммарный % разрушений: {Math.Round(cw.DestructionPercentage, 1)} : " +
            $"{Math.Round(cw.OpponentDestructionPercentage, 1)}", UiTextStyle.Subtitle));

        var firstColumnLength = 16;
        var secondColumnLength = 12;

        str.AppendLine($"\n``` " +
                $"|{StylingHelper.GetCenteredString("Параметр", firstColumnLength)}" +
                $"|{StylingHelper.GetCenteredString("Значение", secondColumnLength)}|");

        str.AppendLine(StylingHelper.GetTableDeviderLine(DeviderType.Colunmn, firstColumnLength, secondColumnLength));

        var dic = new Dictionary<string, string>()
        {
            { "Противник", $"{cw.OpponentName.GetProperName(secondColumnLength)}" },
            { "Тег противника", $"{cw.OpponentTag}" },
            { "Проведено КВ", $"{(cw.OppinentWarWins + cw.OppinentWarDraws + cw.OppinentWarLoses).GetDividedString()}" },
            { "Побед в КВ", $"{cw.OppinentWarWins.GetDividedString()}" },
            { "Поражений в КВ", $"{cw.OppinentWarLoses.GetDividedString()}" },
            { "Ничьих в КВ", $"{cw.OppinentWarDraws.GetDividedString()}" },
            { "Винстрик в КВ", $"{cw.OpponentWarWinStreak.GetDividedString()}" },
        };

        foreach (var item in dic)
        {
            str.Append($" |{item.Key.PadRight(firstColumnLength)}|");

            str.AppendLine($"{StylingHelper.GetCenteredString(item.Value.ToString(), secondColumnLength)}|");
        }

        str.Append("```");

        return str.ToString();
    }
}