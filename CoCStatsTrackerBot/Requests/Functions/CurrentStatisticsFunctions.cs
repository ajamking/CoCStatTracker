using CoCStatsTracker.UIEntities;
using System.Text;

namespace CoCStatsTrackerBot.Requests;

public class CurrentStatisticsFunctions
{
    public static string GetCurrentWarShortInfo(ClanWarUi currentClanWarUi)
    {
        var str = new StringBuilder();

        str.AppendLine(StylingHelper.MakeItStyled("Общая информация о последней войне клана", UiTextStyle.Header));

        str.AppendLine(StylingHelper.MakeItStyled($"{currentClanWarUi.ClanName} - {currentClanWarUi.ClanTag}\n", UiTextStyle.Name));

        str.AppendLine(GetWarMainInfoHat(currentClanWarUi));

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

    public static string GetCurrentWarMap(WarMapUi warMapUi)
    {
        var maxNameLength = 16;

        var warMembers = warMapUi.WarMembers.OrderBy(x => x.MapPosition).ToList();

        var enemyWarMembers = warMapUi.EnemyWarMembers.OrderBy(x => x.MapPosition).ToList();

        var str = new StringBuilder();

        str.AppendLine(StylingHelper.MakeItStyled("Карта текущей войны клана", UiTextStyle.Header));
        str.AppendLine(StylingHelper.MakeItStyled($"{warMapUi.ClanName} - {warMapUi.ClanTag}\n", UiTextStyle.Name));

        str.AppendLine(StylingHelper.MakeItStyled("Противник:", UiTextStyle.Subtitle));
        str.AppendLine(StylingHelper.MakeItStyled($"{warMapUi.OpponentClanName} - {warMapUi.OpponentClanTag}\n", UiTextStyle.Name));

        str.AppendLine(warMapUi.UpdatedOn.GetUpdatedOnString());

        str.Append(StylingHelper.MakeItStyled("\nНачало подготовки:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(warMapUi.PreparationStartTime.FormateToUiDateTime(), UiTextStyle.Subtitle));
        str.Append(StylingHelper.MakeItStyled("Начало войны:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(warMapUi.StartedOn.FormateToUiDateTime(), UiTextStyle.Subtitle));
        str.Append(StylingHelper.MakeItStyled("Конец войны:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(warMapUi.EndedOn.FormateToUiDateTime(), UiTextStyle.Subtitle));

        if (Math.Round(warMapUi.EndedOn.Subtract(DateTime.Now).TotalHours, 0) > 0)
        {
            str.Append(StylingHelper.MakeItStyled("\nОсталось времени до конца:  ", UiTextStyle.Default));
            str.AppendLine(StylingHelper.MakeItStyled(warMapUi.EndedOn.GetTimeLeft(), UiTextStyle.Subtitle));
        }

        str.AppendLine(StylingHelper.MakeItStyled("\nКарта войны:", UiTextStyle.Subtitle));

        str.AppendLine($"``` ");

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

            var mapStr = $"{properMateName.Replace("\\\\", "\\")} {membersThLevel} |{position}| {opponent.TownHallLevel} {properOpponentName}";

            str.AppendLine($@"{StylingHelper.MakeItStyled(mapStr, UiTextStyle.Default)}");
        }

        str.AppendLine($@"```");

        return str.ToString();
    }

    public static string GetCurrentRaidShortInfo(CapitalRaidUi raidsUi)
    {
        var str = new StringBuilder();

        str.AppendLine(StylingHelper.MakeItStyled("Общая информация о последнем рейде клана", UiTextStyle.Header));
        str.AppendLine(StylingHelper.MakeItStyled($"{raidsUi.ClanName} - {raidsUi.ClanTag}\n", UiTextStyle.Name));

        str.Append(GetRaidsMainInfoHat(raidsUi));

        if (raidsUi.NonAttackersRaids.Count != 0)
        {
            str.AppendLine(StylingHelper.MakeItStyled("ПРОВЕЛИ МЕНЕЕ 6 ИЛИ НЕ ПРОВЕЛИ АТАК:", UiTextStyle.Subtitle));
            str.AppendLine(StylingHelper.MakeItStyled("Формат: Игрок  ﴾ Атак проведено ﴿\n", UiTextStyle.Default));

            foreach (var nonAttacker in raidsUi.NonAttackersRaids)
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

    public static string GetDistrictStatistics(CapitalRaidUi raidUi, DistrictType districtType)
    {
        var maxNameLength = 18;
        var max2ColumnLength = 5;
        var max3ColumnLength = 5;

        var chosenDistrictName = FunctionsLogicHelper.AllDistrictsEn.First(x => x.Key == districtType).Value;

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
        str.AppendLine(StylingHelper.MakeItStyled($"{raidUi.ClanName} - {raidUi.ClanTag}\n", UiTextStyle.Name));

        str.Append(StylingHelper.MakeItStyled("В атаках на район: ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled($"{FunctionsLogicHelper.AllDistrictsRU.First(x => x.Key == districtType).Value}\n", UiTextStyle.Header));

        str.AppendLine(raidUi.UpdatedOn.GetUpdatedOnString());

        str.Append(StylingHelper.MakeItStyled("\nРазрушений за атаку в среднем: ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled($"{avgPercent}%", UiTextStyle.Name));

        str.AppendLine(StylingHelper.MakeItStyled("\nПоказатели атак:", UiTextStyle.Subtitle));

        str.AppendLine($"``` " +
                           $"|{StylingHelper.GetCenteredString("Игрок", maxNameLength)}" +
                           $"|{StylingHelper.GetCenteredString("%От", max2ColumnLength)}" +
                           $"|{StylingHelper.GetCenteredString("%До", max3ColumnLength)}|");

        str.AppendLine(StylingHelper.GetTableDeviderLine(DeviderType.Colunmn, maxNameLength, max2ColumnLength, max3ColumnLength));

        str.AppendLine(StylingHelper.GetTableDeviderLine(DeviderType.Whitespace, maxNameLength, max2ColumnLength, max3ColumnLength));

        var tableWidth = maxNameLength + max2ColumnLength + max3ColumnLength + 2;

        foreach (var clan in raidUi.DefeatedClans)
        {
            var totalDistrictLoot = 0;

            if (clan.DefeatedEmemyDistricts.FirstOrDefault(x => x.Name == chosenDistrictName) != null)
            {
                totalDistrictLoot = clan.DefeatedEmemyDistricts.FirstOrDefault(x => x.Name == chosenDistrictName).Loot;
            }

            str.AppendLine($" |{StylingHelper.GetCenteredStringDash($" {StylingHelper.GetProperName(clan.Name, maxNameLength)} - {totalDistrictLoot.GetDividedString()} ", tableWidth)}|");

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

    public static string GetRaidsMainInfoHat(CapitalRaidUi raidUi)
    {
        var str = new StringBuilder();

        var offensiveReward = raidUi.OffensiveReward;

        var totalReward = offensiveReward + raidUi.DefensiveReward;

        str.AppendLine(raidUi.UpdatedOn.GetUpdatedOnString());

        str.Append(StylingHelper.MakeItStyled("\nНачало рейдов:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(raidUi.StartedOn.FormateToUiDateTime(), UiTextStyle.Subtitle));
        str.Append(StylingHelper.MakeItStyled("Конец рейдов:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(raidUi.EndedOn.FormateToUiDateTime(), UiTextStyle.Subtitle));

        if (Math.Round(raidUi.EndedOn.Subtract(DateTime.Now).TotalHours, 0) > 0)
        {
            str.Append(StylingHelper.MakeItStyled("\nОсталось времени до конца:  ", UiTextStyle.Default));
            str.AppendLine(StylingHelper.MakeItStyled(raidUi.EndedOn.GetTimeLeft(), UiTextStyle.Subtitle));
        }

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

        str.Append("```");

        str.AppendLine(StylingHelper.MakeItStyled("\nПояснение таблицы:", UiTextStyle.TableAnnotation));
        str.AppendLine(StylingHelper.MakeItStyled("Атаки - число проведенных атак;", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("Р-н - количество полностью уничтоженных районов;", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("Золото - суммарно награбленное золото;", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("Защиты/Атаки ~123 - среднее количество атак, затрачиваемых на полное уничтожение клана.\n", UiTextStyle.Default));

        var maxClanNameLength = 16;
        var maxAttackLength = 5;
        var maxDDCLength = 3;
        var maxGoldLootedLength = 9;

        var tableLength = maxClanNameLength + maxAttackLength + maxDDCLength + maxGoldLootedLength + 2;

        str.AppendLine($"``` " +
                 $"|{StylingHelper.GetCenteredString("Клан", maxClanNameLength)}" +
                 $"|{StylingHelper.GetCenteredString("Aтаки", maxAttackLength)}" +
                 $"|{StylingHelper.GetCenteredString("Р-н", maxDDCLength)}" +
                 $"|{StylingHelper.GetCenteredString("Золото", maxGoldLootedLength)}|");

        str.AppendLine(StylingHelper.GetTableDeviderLine(DeviderType.Colunmn, maxClanNameLength, maxAttackLength, maxDDCLength, maxGoldLootedLength));

        str.AppendLine(StylingHelper.GetTableDeviderLine(DeviderType.Whitespace, maxClanNameLength, maxAttackLength, maxDDCLength, maxGoldLootedLength));

        var totalLoseDefenses = raidUi.Defenses
            .Where(x => x.DestroyedFriendlyDistrictsCount == FunctionsLogicHelper.AllDistrictsEn.Count);

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
            .Where(x => x.DefeatedEmemyDistricts.Count == FunctionsLogicHelper.AllDistrictsEn.Count);

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

    public static string GetWarMainInfoHat(ClanWarUi cw)
    {
        var str = new StringBuilder();

        str.AppendLine(cw.UpdatedOn.GetUpdatedOnString());

        str.Append(StylingHelper.MakeItStyled("\nНачало подготовки:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(cw.PreparationStartTime.FormateToUiDateTime(), UiTextStyle.Subtitle));
        str.Append(StylingHelper.MakeItStyled("Начало войны:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(cw.StartedOn.FormateToUiDateTime(), UiTextStyle.Subtitle));
        str.Append(StylingHelper.MakeItStyled("Конец войны:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(cw.EndedOn.FormateToUiDateTime(), UiTextStyle.Subtitle));

        if (Math.Round(cw.EndedOn.Subtract(DateTime.Now).TotalHours, 0) > 0)
        {
            str.Append(StylingHelper.MakeItStyled("\nОсталось времени до конца:  ", UiTextStyle.Default));
            str.AppendLine(StylingHelper.MakeItStyled(cw.EndedOn.GetTimeLeft(), UiTextStyle.Subtitle));
        }

        str.Append(StylingHelper.MakeItStyled($"\nСуммарное число атак:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled($"{cw.AttacksCount} : {cw.OpponentAttacksCount}", UiTextStyle.Subtitle));

        str.Append(StylingHelper.MakeItStyled($"Суммарно звезд:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled($"{cw.TotalStarsEarned} : {cw.OpponentStarsCount}", UiTextStyle.Subtitle));

        str.Append(StylingHelper.MakeItStyled($"Суммарный % разрушений:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled($"{Math.Round(cw.DestructionPercentage, 1)}% : " +
            $"{Math.Round(cw.OpponentDestructionPercentage, 1)}%", UiTextStyle.Subtitle));

        str.Append(StylingHelper.MakeItStyled($"\nРезультат:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled($"{cw.Result}", UiTextStyle.Subtitle));

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