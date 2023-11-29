using CoCStatsTracker.UIEntities;
using System.Text;

namespace CoCStatsTrackerBot.Requests;

public static class ClanFunctions
{
    public static string GetClanShortInfo(TrackedClanUi clanUi)
    {
        var dic = new Dictionary<string, string>()
        {
            { "Уровень", $"{clanUi.ClanLevel}" },
            { "Участники", $"{clanUi.ClanMembersCount}" },
            { "Очки клана", $"{clanUi.ClanPoints.GetDividedString()}" },
            { "Очки клана ДС", $"{clanUi.ClanVersusPoints.GetDividedString()}" },
            { "История КВ", $"{clanUi.IsWarLogPublic}" },
            { "Лига ЛВК", $"{clanUi.WarLeague}" },
            { "Проведено КВ", $"{(clanUi.WarWins + clanUi.WarDraws + clanUi.WarLoses).GetDividedString()}" },
            { "Побед КВ", $"{clanUi.WarWins.GetDividedString()}" },
            { "Поражений КВ", $"{clanUi.WarLoses.GetDividedString()}" },
            { "Ничьих КВ", $"{clanUi.WarDraws.GetDividedString()}" },
            { "Винстрик КВ", $"{clanUi.WarWinStreak}" },
            { "Уровень столицы", $"{clanUi.CapitalHallLevel}" },
            { "Лига столицы", $"{clanUi.CapitalLeague}" },
            { "Очки столицы", $"{clanUi.ClanCapitalPoints.GetDividedString()}" },
        };

        var str = new StringBuilder();

        var firstColumnName = "Параметр";

        var secondColumnName = "Значение";

        var firstColumnLength = 16;

        var secondColumnLength = 15;

        str.AppendLine(StylingHelper.MakeItStyled("Краткая информация о клане", UiTextStyle.Header));
        str.AppendLine(StylingHelper.MakeItStyled(clanUi.Name + " - " + clanUi.Tag, UiTextStyle.Name));
        str.AppendLine();
        str.AppendLine(clanUi.UpdatedOn.GetUpdatedOnString());
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Шапка клана:", UiTextStyle.Subtitle));
        str.AppendLine(StylingHelper.MakeItStyled(clanUi.Description, UiTextStyle.Default));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Основные показатели:", UiTextStyle.Subtitle));
        str.AppendLine();
        str.AppendLine($"``` |{firstColumnName.PadRight(firstColumnLength)}|{StylingHelper.GetCenteredString(secondColumnName, secondColumnLength)}|");

        str.AppendLine(StylingHelper.GetTableDeviderLine(DeviderType.Colunmn, firstColumnLength, secondColumnLength));

        foreach (var item in dic)
        {
            str.Append($" |{item.Key.PadRight(firstColumnLength)}|");

            str.AppendLine($"{StylingHelper.GetCenteredString(item.Value.ToString(), secondColumnLength)}|");
        }

        str.Append("```");

        return str.ToString();
    }

    public static string GetClanMembers(List<ClanMemberUi> clanMembersUi)
    {
        var str = new StringBuilder();

        str.AppendLine(StylingHelper.MakeItStyled("Список членов клана", UiTextStyle.Header));
        str.AppendLine(StylingHelper.MakeItStyled($"{clanMembersUi.First().ClanName} - {clanMembersUi.First().ClanTag}\n", UiTextStyle.Name));

        str.AppendLine(clanMembersUi.First().UpdatedOn.GetUpdatedOnString());
        str.AppendLine(StylingHelper.MakeItStyled("Список отсортирован по уровню ТХ.\n", UiTextStyle.Subtitle));

        var counter = 0;

        foreach (var member in clanMembersUi.OrderByDescending(x => x.TownHallLevel))
        {
            counter++;

            var newCounter = "";

            if (counter < 10)
            {
                newCounter = $"0{counter}";
            }
            else
            {
                newCounter = counter.ToString();
            }

            str.AppendLine(StylingHelper.MakeItStyled($"{newCounter}| {member.Name}  [{member.Tag}]  {member.TelegramUserName}", UiTextStyle.Name));
        }

        return str.ToString();
    }

    public static string GetClanSiegeMachines(List<ArmyUi> allArmysUi)
    {
        var str = new StringBuilder();

        var allMachines = FunctionsLogicHelper.SiegeMachinesMapper.Keys.ToList();
        var maxNameLength = 14;
        var maxMachineLevelLength = 1;

        str.AppendLine(StylingHelper.MakeItStyled("Осадные машины игроков клана", UiTextStyle.Header));
        str.AppendLine(StylingHelper.MakeItStyled($"{allArmysUi.First().ClanName} - {allArmysUi.First().ClanTag}\n", UiTextStyle.Name));

        str.AppendLine(allArmysUi.First().UpdatedOn.GetUpdatedOnString());

        str.AppendLine(StylingHelper.MakeItStyled("\nЦифры в столбцах - осадные машины по типу:", UiTextStyle.TableAnnotation));
        str.AppendLine(StylingHelper.MakeItStyled(("1 - " + FunctionsLogicHelper.SiegeMachinesMapper.ElementAt(0).Value), UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(("2 - " + FunctionsLogicHelper.SiegeMachinesMapper.ElementAt(1).Value), UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(("3 - " + FunctionsLogicHelper.SiegeMachinesMapper.ElementAt(2).Value), UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(("4 - " + FunctionsLogicHelper.SiegeMachinesMapper.ElementAt(3).Value), UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(("5 - " + FunctionsLogicHelper.SiegeMachinesMapper.ElementAt(4).Value), UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(("6 - " + FunctionsLogicHelper.SiegeMachinesMapper.ElementAt(5).Value), UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(("7 - " + FunctionsLogicHelper.SiegeMachinesMapper.ElementAt(6).Value), UiTextStyle.Default));

        str.AppendLine(StylingHelper.MakeItStyled("\nЦифры в строках - уровни осадных машин.", UiTextStyle.TableAnnotation));

        str.AppendLine(StylingHelper.MakeItStyled("\nПеречень доступных игрокам машин:\n", UiTextStyle.Subtitle));

        str.AppendLine($"``` " +
                  $"|{StylingHelper.GetCenteredString("Игрок", maxNameLength)}" +
                  $"|{StylingHelper.GetCenteredString("1", maxMachineLevelLength)}" +
                  $"|{StylingHelper.GetCenteredString("2", maxMachineLevelLength)}" +
                  $"|{StylingHelper.GetCenteredString("3", maxMachineLevelLength)}" +
                  $"|{StylingHelper.GetCenteredString("4", maxMachineLevelLength)}" +
                  $"|{StylingHelper.GetCenteredString("5", maxMachineLevelLength)}" +
                  $"|{StylingHelper.GetCenteredString("6", maxMachineLevelLength)}" +
                  $"|{StylingHelper.GetCenteredString("7", maxMachineLevelLength)}|");

        str.AppendLine(StylingHelper.GetTableDeviderLine(DeviderType.Colunmn, maxNameLength, maxMachineLevelLength, maxMachineLevelLength,
            maxMachineLevelLength, maxMachineLevelLength, maxMachineLevelLength, maxMachineLevelLength, maxMachineLevelLength));

        foreach (var memberArmy in allArmysUi.OrderByDescending(x => x.TownHallLevel))
        {
            var memberMachines = FunctionsLogicHelper.GetAllMachineLevels(memberArmy, FunctionsLogicHelper.SiegeMachinesMapper.Keys.ToList());

            if (memberMachines.All(x => x.Value == "0"))
            {
                break;
            }

            var properName = StylingHelper.GetProperName(memberArmy.PlayerName, maxNameLength);

            str.Append($" |{StylingHelper.GetCenteredString(properName, maxNameLength)}|");
            str.Append($"{StylingHelper.GetCenteredString(memberMachines.First(x => x.Key == allMachines[0]).Value, maxMachineLevelLength)}|");
            str.Append($"{StylingHelper.GetCenteredString(memberMachines.First(x => x.Key == allMachines[1]).Value, maxMachineLevelLength)}|");
            str.Append($"{StylingHelper.GetCenteredString(memberMachines.First(x => x.Key == allMachines[2]).Value, maxMachineLevelLength)}|");
            str.Append($"{StylingHelper.GetCenteredString(memberMachines.First(x => x.Key == allMachines[3]).Value, maxMachineLevelLength)}|");
            str.Append($"{StylingHelper.GetCenteredString(memberMachines.First(x => x.Key == allMachines[4]).Value, maxMachineLevelLength)}|");
            str.Append($"{StylingHelper.GetCenteredString(memberMachines.First(x => x.Key == allMachines[5]).Value, maxMachineLevelLength)}|");
            str.AppendLine($"{StylingHelper.GetCenteredString(memberMachines.First(x => x.Key == allMachines[6]).Value, maxMachineLevelLength)}|");
        }

        str.Append("```\n");

        return str.ToString();
    }

    public static string GetClanActiveeSuperUnits(List<ArmyUi> allArmysUi)
    {
        var str = new StringBuilder();

        var maxNameLength = 14;

        str.AppendLine(StylingHelper.MakeItStyled("Активные супер юниты клана", UiTextStyle.Header));
        str.AppendLine(StylingHelper.MakeItStyled($"{allArmysUi.First().ClanName} - {allArmysUi.First().ClanTag}\n", UiTextStyle.Name));

        str.AppendLine(allArmysUi.First().UpdatedOn.GetUpdatedOnString());
        str.AppendLine();

        foreach (var superUnit in FunctionsLogicHelper.SuperUnitsMapper)
        {
            if (allArmysUi.Any(x => x.SuperUnits.FirstOrDefault(x => x.Name == superUnit.Key)?.SuperTroopIsActivated == true))
            {
                str.AppendLine(StylingHelper.MakeItStyled(superUnit.Value + ":", UiTextStyle.TableAnnotation));

                var members = "";

                foreach (var memberArmy in allArmysUi)
                {
                    if (memberArmy.SuperUnits.FirstOrDefault(x => x.Name == superUnit.Key)?.SuperTroopIsActivated == true)
                    {
                        var properName = memberArmy.PlayerName;

                        if (properName.Length > maxNameLength)
                        {
                            properName = properName[..maxNameLength];
                        }

                        members += properName + ";  ";
                    }
                }

                str.AppendLine(StylingHelper.MakeItStyled(members, UiTextStyle.Name));

                str.AppendLine();
            }
        }

        return str.ToString();
    }

    public static string GetSeasonClanMembersStatistcs(List<SeasonStatisticsUi> seasonStatisticsUistring)
    {
        var str = new StringBuilder();

        var maxNameLength = 10;
        var maxStarsLength = 2;
        var maxDonationsLength = 6;
        var maxDonationsRecievedLength = 6;
        var attacskWinsLength = 3;
        var versusAttacksWinsLength = 3;
        var maxCapitalLootLength = 7;

        str.AppendLine(StylingHelper.MakeItStyled("Сезонная статистика игроков клана", UiTextStyle.Header));
        str.AppendLine(StylingHelper.MakeItStyled($"{seasonStatisticsUistring.First().ClanName} - {seasonStatisticsUistring.First().ClanTag}\n", UiTextStyle.Name));

        str.Append(StylingHelper.MakeItStyled("Сбор статистики начат:  ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(seasonStatisticsUistring.First().InitializedOn.FormateToUiDateTime(), UiTextStyle.Subtitle));

        str.AppendLine(seasonStatisticsUistring.First().UpdatedOn.GetUpdatedOnString());

        str.AppendLine(StylingHelper.MakeItStyled("\nПояснение таблицы:", UiTextStyle.TableAnnotation));
        str.AppendLine(StylingHelper.MakeItStyled("Зв - боевые звезды, заработанные за сезон.", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("Дон.о - отправлено войск за сезон.", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("Дон.п - получено войск за сезон.", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("Атх - успешных атак в глобале за сезон.", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("Адс - успешных атак на ДС за сезон.", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("З.с. - золото столицы, вложенное за сезон.\n", UiTextStyle.Default));

        str.AppendLine(StylingHelper.MakeItStyled("Сбор статистики начинается заново каждые " +
            "30 дней или по усмотрению главы вашего клана.\n", UiTextStyle.Subtitle));

        str.AppendLine($"``` " +
                  $"|{StylingHelper.GetCenteredString("Игрок", maxNameLength)}" +
                  $"|{StylingHelper.GetCenteredString("Зв", maxStarsLength)}" +
                  $"|{StylingHelper.GetCenteredString("Дон.о", maxDonationsLength)}" +
                  $"|{StylingHelper.GetCenteredString("Дон.п", maxDonationsRecievedLength)}" +
                  $"|{StylingHelper.GetCenteredString("Атх", attacskWinsLength)}" +
                  $"|{StylingHelper.GetCenteredString("Адс", versusAttacksWinsLength)}" +
                  $"|{StylingHelper.GetCenteredString("З.с.", maxCapitalLootLength)}|");

        str.AppendLine(StylingHelper.GetTableDeviderLine(DeviderType.Colunmn, maxNameLength, maxStarsLength, maxDonationsLength,
            maxDonationsRecievedLength, attacskWinsLength, versusAttacksWinsLength, maxCapitalLootLength));

        foreach (var member in seasonStatisticsUistring)
        {
            var properName = StylingHelper.GetProperName(member.Name, maxNameLength);

            str.Append($" |{StylingHelper.GetCenteredString(properName, maxNameLength)}|");

            str.Append($"{StylingHelper.GetCenteredString(member.WarStarsEarned.ReturnZeroIfLess().ToString(), maxStarsLength)}|");

            str.Append($"{StylingHelper.GetCenteredString(member.DonationsSend.ReturnZeroIfLess().GetDividedString(), maxDonationsLength)}|");

            str.Append($"{StylingHelper.GetCenteredString(member.DonationRecieved.ReturnZeroIfLess().GetDividedString(), maxDonationsRecievedLength)}|");

            str.Append($"{StylingHelper.GetCenteredString(member.AttackWins.ReturnZeroIfLess().ToString(), attacskWinsLength)}|");

            str.Append($"{StylingHelper.GetCenteredString(member.VersusBattleWins.ReturnZeroIfLess().ToString(), versusAttacksWinsLength)}|");

            str.AppendLine($"{StylingHelper.GetCenteredString(member.CapitalContributions.ReturnZeroIfLess().GetDividedString(), maxCapitalLootLength)}|");
        }

        str.Append("```\n");

        return str.ToString();
    }

    public static string GetRaidsHistory(List<CapitalRaidUi> raidsUi, int recordsCount, string messageSplitToken)
    {
        var str = new StringBuilder();

        var maxNameLength = 18;
        var maxDistrictLength = 9;
        var maxAttackLenght = 6;

        str.AppendLine(StylingHelper.MakeItStyled("Результаты рейдов клана", UiTextStyle.Header));

        str.AppendLine(StylingHelper.MakeItStyled($"{raidsUi.First().ClanName} - {raidsUi.First().ClanTag}\n", UiTextStyle.Name));

        var counter = 0;

        foreach (var raid in raidsUi.OrderByDescending(x => x.StartedOn))
        {
            if (counter < recordsCount)
            {
                str.AppendLine(CurrentStatisticsFunctions.GetRaidsMainInfoHat(raid));

                str.AppendLine($@"{messageSplitToken}");

                str.AppendLine(StylingHelper.MakeItStyled("Показатели атак:\n", UiTextStyle.Subtitle));

                var defeatedClansCounter = 0;

                foreach (var defeatedClan in raid.DefeatedClans.OrderByDescending(x => x.TotalAttacksCount))
                {
                    str.AppendLine($"``` ");

                    var tableWidth = maxNameLength + maxDistrictLength + maxAttackLenght + 2;

                    var properClanName = StylingHelper.GetProperName(defeatedClan.Name, maxNameLength);

                    var defClanInfo = $"{properClanName} A-[{defeatedClan.TotalAttacksCount}] З-[{defeatedClan.TotalLoot.GetDividedString()}]";

                    str.AppendLine($" {StylingHelper.GetCenteredString(defClanInfo, tableWidth + 2)}");

                    str.AppendLine(StylingHelper.GetTableDeviderLine(DeviderType.Colunmn, maxNameLength, maxDistrictLength, maxAttackLenght));

                    str.AppendLine(" " +
                   $"|{StylingHelper.GetCenteredString("Игрок", maxNameLength)}" +
                   $"|{StylingHelper.GetCenteredString("Район", maxDistrictLength)}" +
                   $"|{StylingHelper.GetCenteredString("От-До%", maxAttackLenght)}|");

                    str.AppendLine(StylingHelper.GetTableDeviderLine(DeviderType.Colunmn, maxNameLength, maxDistrictLength, maxAttackLenght));

                    var districtCounter = 0;

                    foreach (var district in defeatedClan.DefeatedEmemyDistricts.SortDistrictsAsOnMap())
                    {
                        foreach (var attack in district.Attacks)
                        {
                            var properName = StylingHelper.GetProperName(attack.AttackerName, maxNameLength);

                            str.Append($" |{StylingHelper.GetCenteredString(properName, maxNameLength)}|");

                            str.Append($"{StylingHelper.GetCenteredString(StylingHelper.GetFirstWord(district.Name), maxDistrictLength)}|");

                            str.AppendLine($"{StylingHelper.GetCenteredString(attack.DestructionPercentFrom + "-" + attack.DestructionPercentTo, maxAttackLenght)}|");
                        }

                        districtCounter++;

                        if (districtCounter != defeatedClan.DefeatedEmemyDistricts.Count)
                        {
                            str.AppendLine(StylingHelper.GetTableDeviderLine(DeviderType.Colunmn, maxNameLength, maxDistrictLength, maxAttackLenght));
                        }
                    }

                    str.Append("```\n");

                    defeatedClansCounter++;

                    var optimalRecordsCount = 2;

                    if (defeatedClansCounter > 0 && defeatedClansCounter % optimalRecordsCount == 0)
                    {
                        str.Append($@"{messageSplitToken}");
                    }
                }

                counter++;
            }
        }

        return str.ToString();
    }

    public static string GetClanWarHistory(List<ClanWarUi> clanWarsUi, int recordsCount, string messageSplitToken)
    {
        var str = new StringBuilder();

        var maxAttackLenght = 8;

        var maxNameLength = 12;

        if (recordsCount != 1)
        {
            str.AppendLine(StylingHelper.MakeItStyled("Результаты войн клана", UiTextStyle.Header));
        }
        else
        {
            str.AppendLine(StylingHelper.MakeItStyled("Результ последней войны клана", UiTextStyle.Header));
        }

        str.AppendLine(StylingHelper.MakeItStyled($"{clanWarsUi.First().ClanName} - {clanWarsUi.First().ClanTag}\n", UiTextStyle.Name));

        var counter = 0;

        foreach (var cw in clanWarsUi.OrderByDescending(x => x.StartedOn))
        {
            if (counter < recordsCount)
            {
                if (recordsCount != 1)
                {
                    str.AppendLine($@"{messageSplitToken}");
                }

                str.AppendLine(CurrentStatisticsFunctions.GetWarMainInfoHat(cw));

                str.AppendLine(StylingHelper.MakeItStyled("Показатели атак:", UiTextStyle.Subtitle));

                if (cw.MembersResults.All(x => x.FirstEnemyThLevel == 0 && x.SecondEnemyThLevel == 0))
                {
                    str.AppendLine(StylingHelper.MakeItStyled("Никто еще не провел атак.", UiTextStyle.Default));

                    counter++;

                    continue;
                }

                str.AppendLine(StylingHelper.MakeItStyled("Пояснение таблицы:", UiTextStyle.TableAnnotation));

                str.AppendLine(StylingHelper.MakeItStyled("Атаки - Уровень ТХ / Проценты / Звезды\n", UiTextStyle.Default));

                str.AppendLine($"``` " +
                    $"|{StylingHelper.GetCenteredString("Игрок", maxNameLength)}" +
                    $"|{StylingHelper.GetCenteredString("Атака 1", maxAttackLenght)}" +
                    $"|{StylingHelper.GetCenteredString("Атака 2", maxAttackLenght)}|");

                str.AppendLine(StylingHelper.GetTableDeviderLine(DeviderType.Colunmn, maxNameLength, maxAttackLenght, maxAttackLenght));

                foreach (var attack in cw.MembersResults.OrderByDescending(x => x.ThLevel))
                {
                    var membersThLevel = attack.ThLevel.ToString();

                    if (attack.ThLevel < 10)
                    {
                        membersThLevel += "  ";
                    }
                    else
                    {
                        membersThLevel += " ";
                    }

                    var properName = StylingHelper.GetProperName(attack.PlayerName, maxNameLength - membersThLevel.Length);

                    str.Append($" |{membersThLevel}{StylingHelper.GetCenteredString(properName, maxNameLength - membersThLevel.Length)}|");

                    str.Append($"{StylingHelper.GetCenteredString(attack.FirstEnemyThLevel + "/" + attack.FirstDestructionPercent + "/" + attack.FirstStarsCount, maxAttackLenght)}|");

                    if (cw.AttackPerMember != 1)
                    {
                        str.AppendLine($"{StylingHelper.GetCenteredString(attack.SecondEnemyThLevel + "/" + attack.SecondDestructionpercent + "/" + attack.SecondStarsCount, maxAttackLenght)}|");
                    }
                    else
                    {
                        str.AppendLine($"{StylingHelper.GetCenteredString("-", maxAttackLenght)}|");
                    }

                }

                str.Append("```\n");

                counter++;
            }
        }

        return str.ToString();
    }

    public static string GetMembersAverageRaidsPerfomance(List<MedianRaidPerfomanseUi> playersPerfomances)
    {
        var maxNameLength = 12;
        var maxDestructionPercentLength = 5;
        var maxCapitalLootLength = 6;
        var maxNumberOfRaidsCount = 3;

        var str = new StringBuilder();

        str.AppendLine(StylingHelper.MakeItStyled("Средние показатели рейдеров клана:", UiTextStyle.Header));
        str.AppendLine(StylingHelper.MakeItStyled($"{playersPerfomances.First().ClanName} - {playersPerfomances.First().ClanTag}\n", UiTextStyle.Name));

        str.AppendLine(playersPerfomances.First().UpdatedOn.GetUpdatedOnString());

        str.AppendLine(StylingHelper.MakeItStyled("\nПояснение таблицы:", UiTextStyle.TableAnnotation));
        str.AppendLine(StylingHelper.MakeItStyled("N - учтенное количество участий в рейдах.", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("Ср.% - средний процент разрушений за атаку.", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("Золото - среднее количество золота, добываемого за рейд.\n", UiTextStyle.Default));

        str.AppendLine(StylingHelper.MakeItStyled("*Таблица отсортирована по убыванию среднего процента разрушений.\n", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("**На самом деле для большей наглядности в таблице выведены не усредненные значения, а медианные.\n", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("Средние показатели рейдеров:\n", UiTextStyle.Subtitle));

        str.AppendLine($"``` " +
                  $"|{StylingHelper.GetCenteredString("Игрок", maxNameLength)}" +
                  $"|{StylingHelper.GetCenteredString("N", maxNumberOfRaidsCount)}" +
                  $"|{StylingHelper.GetCenteredString("Ср.%", maxDestructionPercentLength)}" +
                  $"|{StylingHelper.GetCenteredString("Золото", maxCapitalLootLength)}|");

        str.AppendLine(StylingHelper.GetTableDeviderLine(DeviderType.Dashes, maxNameLength, maxNumberOfRaidsCount, maxDestructionPercentLength, maxCapitalLootLength));

        foreach (var perfomance in playersPerfomances.OrderByDescending(x => x.MedianDestructionPersent))
        {
            var properName = StylingHelper.GetProperName(perfomance.Name, maxNameLength);

            str.Append($" |{StylingHelper.GetCenteredString(properName, maxNameLength)}|");

            str.Append($"{StylingHelper.GetCenteredString(perfomance.RaidMembershipsCount.ToString(), maxNumberOfRaidsCount)}|");

            str.Append($"{StylingHelper.GetCenteredString(perfomance.MedianDestructionPersent.ToString(), maxDestructionPercentLength)}|");

            str.AppendLine($"{StylingHelper.GetCenteredString(perfomance.MedianLoot.GetDividedString(), maxCapitalLootLength)}|");
        }

        str.Append("```\n");

        return str.ToString();
    }
}