using CoCStatsTracker.UIEntities;
using System.Text;

namespace CoCStatsTrackerBot.Requests;

public static class ClanFunctions
{
    public static string GetClanShortInfo(ClanUi clanUi)
    {
        var newCWLeagueString = clanUi.WarLeague.Replace("League ", "");
        var newCapitalLeagueString = clanUi.CapitalLeague.Replace("League ", "");

        var dic = new Dictionary<string, string>()
        {
            { "Уровень", $"{clanUi.ClanLevel}" },
            { "Участники", $"{clanUi.ClanMembersCount}" },
            { "Очки клана", $"{clanUi.ClanPoints.GetDividedString()}" },
            { "Очки клана ДС", $"{clanUi.ClanVersusPoints.GetDividedString()}" },
            { "История КВ", $"{clanUi.IsWarLogPublic}" },
            { "Лига ЛВК", $"{newCWLeagueString}" },
            { "Винстрик КВ", $"{clanUi.WarWinStreak}" },
            { "Побед КВ", $"{clanUi.WarWins.GetDividedString()}" },
            { "Ничьих КВ", $"{clanUi.WarTies.GetDividedString()}" },
            { "Поражений КВ", $"{clanUi.WarLoses.GetDividedString()}" },
            { "Уровень столицы", $"{clanUi.CapitalHallLevel}" },
            { "Лига столицы", $"{newCapitalLeagueString}" },
            { "Очки столицы", $"{clanUi.ClanCapitalPoints.GetDividedString()}" },
        };

        var str = new StringBuilder();

        var firstColumnName = "Параметр";

        var secondColumnName = "Значение";

        var tableSize = StylingHelper.DefineTableMaxSize(dic, firstColumnName, secondColumnName);

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
        str.AppendLine($"``` |{firstColumnName.PadRight(tableSize.KeyMaxLength)}|{StylingHelper.GetCenteredString(secondColumnName, tableSize.ValueMaxLength)}|");
        str.AppendLine($" |{new string('-', tableSize.KeyMaxLength)}|{new string('-', tableSize.ValueMaxLength)}|");

        foreach (var item in dic)
        {
            str.Append($" |{item.Key.PadRight(tableSize.KeyMaxLength)}|");

            str.AppendLine($"{StylingHelper.GetCenteredString(item.Value.ToString(), tableSize.ValueMaxLength)}|");
        }

        str.Append("```");

        return str.ToString();
    }

    public static string GetClanMembers(List<ClanMemberUi> clanMembersUi)
    {
        var str = new StringBuilder();

        str.AppendLine(StylingHelper.MakeItStyled("Список членов клана", UiTextStyle.Header));
        str.AppendLine(StylingHelper.MakeItStyled(clanMembersUi.First().ClanName + " - " + clanMembersUi.First().ClanTag, UiTextStyle.Name));
        str.AppendLine();
        str.AppendLine(clanMembersUi.FirstOrDefault().UpdatedOn.GetUpdatedOnString());
        str.AppendLine(StylingHelper.MakeItStyled("Список отсортирован по уровню ТХ.", UiTextStyle.Subtitle));
        str.AppendLine();

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

        var machineMapper = new Dictionary<string, string>(){
                { "Wall Wrecker", "Разрушитель стен"},
                { "Battle Blimp", "Боевой дирижабль"},
                { "Stone Slammer", "Камнебросатель"},
                { "Siege Barracks", "Осадные казармы"},
                { "Log Launcher", "Бревномет"},
                { "Flame Flinger", "Огнеметатель"},
                { "Battle Drill", "Боевой бур"},};

        var allMachines = machineMapper.Keys.ToList();
        var maxNameLength = 14;
        var maxMachineLevelLength = 1;

        str.AppendLine(StylingHelper.MakeItStyled("Осадные машины игроков клана", UiTextStyle.Header));
        str.AppendLine(StylingHelper.MakeItStyled(allArmysUi.First().ClanName + " - " + allArmysUi.First().ClanTag, UiTextStyle.Name));
        str.AppendLine();
        str.AppendLine(allArmysUi.First().UpdatedOn.GetUpdatedOnString());
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Пояснение таблицы", UiTextStyle.TableAnnotation));
        str.AppendLine(StylingHelper.MakeItStyled(("1 - " + machineMapper.ElementAt(0).Value), UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(("2 - " + machineMapper.ElementAt(1).Value), UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(("3 - " + machineMapper.ElementAt(2).Value), UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(("4 - " + machineMapper.ElementAt(3).Value), UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(("5 - " + machineMapper.ElementAt(4).Value), UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(("6 - " + machineMapper.ElementAt(5).Value), UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(("7 - " + machineMapper.ElementAt(6).Value), UiTextStyle.Default));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Цифры в столбцах - уровни осадных машин.", UiTextStyle.Default));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Перечень доступных игрокам машин:", UiTextStyle.Subtitle));
        str.AppendLine();

        str.AppendLine($"``` " +
                  $"|{StylingHelper.GetCenteredString("Игрок", maxNameLength)}" +
                  $"|{StylingHelper.GetCenteredString("1", maxMachineLevelLength)}" +
                  $"|{StylingHelper.GetCenteredString("2", maxMachineLevelLength)}" +
                  $"|{StylingHelper.GetCenteredString("3", maxMachineLevelLength)}" +
                  $"|{StylingHelper.GetCenteredString("4", maxMachineLevelLength)}" +
                  $"|{StylingHelper.GetCenteredString("5", maxMachineLevelLength)}" +
                  $"|{StylingHelper.GetCenteredString("6", maxMachineLevelLength)}" +
                  $"|{StylingHelper.GetCenteredString("7", maxMachineLevelLength)}|");

        str.AppendLine($" " +
            $"|{new string('-', maxNameLength)}" +
            $"|{new string('-', maxMachineLevelLength)}" +
            $"|{new string('-', maxMachineLevelLength)}" +
            $"|{new string('-', maxMachineLevelLength)}" +
            $"|{new string('-', maxMachineLevelLength)}" +
            $"|{new string('-', maxMachineLevelLength)}" +
            $"|{new string('-', maxMachineLevelLength)}" +
            $"|{new string('-', maxMachineLevelLength)}|");

        foreach (var memberArmy in allArmysUi.OrderByDescending(x => x.TownHallLevel))
        {
            var memberMachines = GetAllMachineLevels(memberArmy, machineMapper.Keys.ToList());

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

        var superUnitsMapper = new Dictionary<string, string>(){
                { "Super Barbarian", "Суперварвар"},
                { "Super Archer", "Суперлучница"},
                { "Super Giant", "Супергигант"},

                { "Sneaky Goblin", "Коварный гоблин"},
                { "Super Wall Breaker", "Суперстенобой"},
                { "Rocket Balloon", "Ракетный шар"},

                { "Super Wizard", "Суперколдун"},
                { "Super Dragon", "Супердракон"},
                { "Inferno Dragon", "Пламенный дракон"},

                { "Super Minion", "Суперминьон"},
                { "Super Valkyrie", "Супервалькирия"},
                { "Super Witch", "Суперведьма"},

                { "Ice Hound", "Ледяная гончая"},
                { "Super Bowler", "Супервышибала"},
                { "Super Miner", "Супершахтер"},
                { "Super Hog Rider", "Супервсадник на кабане"},};

        str.AppendLine(StylingHelper.MakeItStyled("Активные супер юниты клана", UiTextStyle.Header));
        str.AppendLine(StylingHelper.MakeItStyled(allArmysUi.First().ClanName + " - " + allArmysUi.First().ClanTag, UiTextStyle.Name));
        str.AppendLine();
        str.AppendLine(allArmysUi.First().UpdatedOn.GetUpdatedOnString());
        str.AppendLine();

        foreach (var superUnit in superUnitsMapper)
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
                            properName = properName.Substring(0, maxNameLength);
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
        var maxDonationsLength = 5;
        var maxDonationsRecieved = 5;
        var attacskWins = 3;
        var versusAttacksWins = 3;
        var maxCapitalLootLength = 6;


        str.AppendLine(StylingHelper.MakeItStyled("Сезонная статистика игроков клана", UiTextStyle.Header));
        str.AppendLine(StylingHelper.MakeItStyled(seasonStatisticsUistring.First().ClanName + " - " + seasonStatisticsUistring.First().ClanTag, UiTextStyle.Name));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Сбор статистики начат: " + seasonStatisticsUistring.First().InitializedOn.FormateToUiDateTime(), UiTextStyle.Subtitle));
        str.AppendLine(seasonStatisticsUistring.First().UpdatedOn.GetUpdatedOnString());
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Пояснение таблицы:", UiTextStyle.TableAnnotation));
        str.AppendLine(StylingHelper.MakeItStyled("Зв - боевые звезды, заработанные за сезон.", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("Дон.о - отправлено войск за сезон.", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("Дон.п - получено войск за сезон.", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("Атх - успешных атак в глобале за сезон.", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("Адс - успешных атак на ДС за сезон.", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("З.с. - золото столицы, вложенное за сезон.", UiTextStyle.Default));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Сбор статистики начинается заново каждый рейтинговый сезон или по усмотрению администратора.", UiTextStyle.Default));
        str.AppendLine();

        str.AppendLine($"``` " +
                  $"|{StylingHelper.GetCenteredString("Игрок", maxNameLength)}" +
                  $"|{StylingHelper.GetCenteredString("Зв", maxStarsLength)}" +
                  $"|{StylingHelper.GetCenteredString("Дон.о", maxDonationsLength)}" +
                  $"|{StylingHelper.GetCenteredString("Дон.п", maxDonationsRecieved)}" +
                  $"|{StylingHelper.GetCenteredString("Атх", attacskWins)}" +
                  $"|{StylingHelper.GetCenteredString("Адс", versusAttacksWins)}" +
                  $"|{StylingHelper.GetCenteredString("З.с.", maxCapitalLootLength)}|");

        str.AppendLine($" " +
            $"|{new string('-', maxNameLength)}" +
            $"|{new string('-', maxStarsLength)}" +
            $"|{new string('-', maxDonationsLength)}" +
            $"|{new string('-', maxDonationsRecieved)}" +
            $"|{new string('-', attacskWins)}" +
            $"|{new string('-', versusAttacksWins)}" +
            $"|{new string('-', maxCapitalLootLength)}|");

        foreach (var member in seasonStatisticsUistring)
        {
            var properName = StylingHelper.GetProperName(member.Name, maxNameLength);

            str.Append($" |{StylingHelper.GetCenteredString(properName, maxNameLength)}|");

            str.Append($"{StylingHelper.GetCenteredString((member.WarStarsEarned).ToString(), maxStarsLength)}|");

            str.Append($"{StylingHelper.GetCenteredString((member.DonationsSend).ToString(), maxDonationsLength)}|");

            str.Append($"{StylingHelper.GetCenteredString((member.DonationRecieved).ToString(), maxDonationsRecieved)}|");

            str.Append($"{StylingHelper.GetCenteredString((member.AttackWins).ToString(), attacskWins)}|");

            str.Append($"{StylingHelper.GetCenteredString((member.VersusBattleWins).ToString(), versusAttacksWins)}|");

            str.AppendLine($"{StylingHelper.GetCenteredString((member.CapitalContributions).ToString(), maxCapitalLootLength)}|");
        }

        str.Append("```\n");

        return str.ToString();
    }

    public static string GetRaidsHistory(List<RaidUi> raidsUi, int recordsCount, string messageSplitToken)
    {
        var str = new StringBuilder();

        var maxNameLength = 12;
        var maxDistrictLength = 9;
        var maxAttackLenght = 6;

        str.AppendLine(StylingHelper.MakeItStyled("Результаты рейдов клана", UiTextStyle.Header));
        str.AppendLine(StylingHelper.MakeItStyled(raidsUi.First().ClanName + " - " + raidsUi.First().ClanTag, UiTextStyle.Name));
        str.AppendLine();

        var counter = 0;

        foreach (var raid in raidsUi.OrderByDescending(x => x.StartedOn))
        {
            if (counter < recordsCount)
            {
                str.AppendLine(CurrentStatisticsFunctions.GetRaidsMainInfoHat(raid));

                str.AppendLine($@"{messageSplitToken}");

                str.AppendLine(StylingHelper.MakeItStyled("Показатели атак:", UiTextStyle.Subtitle));
                str.AppendLine();

                var defeatedClansCounter = 0;

                foreach (var defeatedClan in raid.DefeatedClans)
                {
                    str.AppendLine($"``` " +
                   $"|{StylingHelper.GetCenteredString("Игрок", maxNameLength)}" +
                   $"|{StylingHelper.GetCenteredString("Район", maxDistrictLength)}" +
                   $"|{StylingHelper.GetCenteredString("От-До%", maxAttackLenght)}|");

                    str.AppendLine($" " +
                        $"|{new string('-', maxNameLength)}" +
                        $"|{new string('-', maxDistrictLength)}" +
                        $"|{new string('-', maxAttackLenght)}|");

                    var tableWidth = maxNameLength + maxDistrictLength + maxAttackLenght + 2;

                    str.AppendLine($" {StylingHelper.GetCenteredString(" ", tableWidth + 2)}");

                    str.AppendLine($" |{StylingHelper.GetCenteredStringDash(StylingHelper.GetProperName(defeatedClan.Name, maxNameLength), tableWidth)}|");

                    foreach (var district in defeatedClan.DefeatedEmemyDistricts.SortAsOnMap())
                    {
                        foreach (var attack in district.Attacks)
                        {
                            var properName = StylingHelper.GetProperName(attack.AttackerName, maxNameLength);

                            str.Append($" |{StylingHelper.GetCenteredString(properName, maxNameLength)}|");

                            str.Append($"{StylingHelper.GetCenteredString(StylingHelper.GetFirstWord(district.Name), maxDistrictLength)}|");

                            str.AppendLine($"{StylingHelper.GetCenteredString(attack.DestructionPercentFrom + "-" + attack.DestructionPercentTo, maxAttackLenght)}|");
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

    public static string GetClanWarHistory(List<CwCwlUi> clanWarsUi, int recordsCount, string messageSplitToken)
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

        str.AppendLine(StylingHelper.MakeItStyled(clanWarsUi.First().ClanName + " - " + clanWarsUi.First().ClanTag, UiTextStyle.Name));
        str.AppendLine();

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
                str.AppendLine(StylingHelper.MakeItStyled("Атаки - Уровень ТХ / Проценты / Звезды", UiTextStyle.Default));
                str.AppendLine();

                str.AppendLine($"``` " +
                    $"|{StylingHelper.GetCenteredString("Игрок", maxNameLength)}" +
                    $"|{StylingHelper.GetCenteredString("Атака 1", maxAttackLenght)}" +
                    $"|{StylingHelper.GetCenteredString("Атака 2", maxAttackLenght)}|");

                str.AppendLine($" " +
                    $"|{new string('-', maxNameLength)}" +
                    $"|{new string('-', maxAttackLenght)}" +
                    $"|{new string('-', maxAttackLenght)}|");

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

    public static string GetMembersAverageRaidsPerfomance(List<AverageRaidsPerfomanceUi> playersPerfomances)
    {
        var maxNameLength = 12;
        var maxDestructionPercentLength = 5;
        var maxCapitalLootLength = 6;
        var maxNumberOfRaidsCount = 3;

        var str = new StringBuilder();

        str.AppendLine(StylingHelper.MakeItStyled("Средние показатели рейдеров клана:", UiTextStyle.Header));
        str.AppendLine(StylingHelper.MakeItStyled(playersPerfomances.First().ClanName + " - " + playersPerfomances.First().ClanTag, UiTextStyle.Name));
        str.AppendLine();
        str.AppendLine(playersPerfomances.First().UpdatedOn.GetUpdatedOnString());
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Пояснение таблицы:", UiTextStyle.TableAnnotation));
        str.AppendLine(StylingHelper.MakeItStyled("N - учтенное количество участий в рейдах.", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("Ср.% - средний процент разрушений за атаку.", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("Золото - среднее количество золота, добываемого за рейд.", UiTextStyle.Default));

        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Таблица отсортирована по убыванию среднего процента разрушений", UiTextStyle.Default));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Средние показатели рейдеров:", UiTextStyle.Subtitle));
        str.AppendLine();

        str.AppendLine($"``` " +
                  $"|{StylingHelper.GetCenteredString("Игрок", maxNameLength)}" +
                  $"|{StylingHelper.GetCenteredString("N", maxNumberOfRaidsCount)}" +
                  $"|{StylingHelper.GetCenteredString("Ср.%", maxDestructionPercentLength)}" +
                  $"|{StylingHelper.GetCenteredString("Золото", maxCapitalLootLength)}|");

        str.AppendLine($" " +
            $"|{new string('-', maxNameLength)}" +
            $"|{new string('-', maxNumberOfRaidsCount)}" +
            $"|{new string('-', maxDestructionPercentLength)}" +
            $"|{new string('-', maxCapitalLootLength)}|");


        foreach (var perfomance in playersPerfomances.OrderByDescending(x => x.AverageDestructionPercent))
        {
            var properName = StylingHelper.GetProperName(perfomance.Name, maxNameLength);

            str.Append($" |{StylingHelper.GetCenteredString(properName, maxNameLength)}|");

            str.Append($"{StylingHelper.GetCenteredString(perfomance.RaidMembershipsCount.ToString(), maxNumberOfRaidsCount)}|");

            str.Append($"{StylingHelper.GetCenteredString(perfomance.AverageDestructionPercent.ToString(), maxDestructionPercentLength)}|");

            str.AppendLine($"{StylingHelper.GetCenteredString(perfomance.AverageCapitalLoot.GetDividedString(), maxCapitalLootLength)}|");
        }

        str.Append("```\n");

        return str.ToString();
    }

    private static Dictionary<string, string> GetAllMachineLevels(ArmyUi memberArmy, List<string> allMachinesInGame)
    {
        var machineLevels = new Dictionary<string, string>();

        foreach (var machine in allMachinesInGame)
        {
            if (memberArmy.SiegeMachines.Any(x => x.Name == machine))
            {
                machineLevels.Add(machine, memberArmy.SiegeMachines.First(x => x.Name == machine).Lvl.ToString());
            }
            else
            {
                machineLevels.Add(machine, "0");
            }
        }

        return machineLevels;
    }
}