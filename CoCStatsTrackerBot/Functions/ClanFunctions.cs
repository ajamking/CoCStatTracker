using CoCApiDealer.ApiRequests;
using CoCStatsTracker;
using CoCStatsTracker.UIEntities;
using Domain.Entities;
using System.Text;

namespace CoCStatsTrackerBot;

public static class ClanFunctions
{
    public static string GetClanShortInfo(string clanTag, ICollection<TrackedClan> trackedClans)
    {
        try
        {
            var clanUi = UiHelper.GetActiveClanUi(clanTag, trackedClans);

            if (clanUi == null)
            {
                return UiHelper.Ecranize($"Клан с тегом {clanTag} не отслеживается. Введите корректный тег клана");
            }

            var newCWLeagueString = "";
            var newCapitalLeagueString = "";
            newCWLeagueString = clanUi.WarLeague.Replace("League ", "");
            newCapitalLeagueString = clanUi.CapitalLeague.Replace("League ", "");

            var dic = new Dictionary<string, string>()
        {
            { "Уровень", $"{clanUi.ClanLevel}" },
            { "Участники", $"{clanUi.ClanMembersCount}" },
            { "Очки клана", $"{clanUi.ClanPoints}" },
            { "Очки клана ДС", $"{clanUi.ClanVersusPoints}" },
            { "История КВ", $"{clanUi.IsWarLogPublic}" },
            { "Лига ЛВК", $"{newCWLeagueString}" },
            { "Винстрик КВ", $"{clanUi.WarWinStreak}" },
            { "Побед КВ", $"{clanUi.WarWins}" },
            { "Ничьих КВ", $"{clanUi.WarTies}" },
            { "Поражений КВ", $"{clanUi.WarLoses}" },
            { "Уровень столицы", $"{clanUi.CapitalHallLevel}" },
            { "Лига столицы", $"{newCapitalLeagueString}" },
            { "Очки столицы", $"{clanUi.ClanCapitalPoints}" },
        };

            var str = new StringBuilder();

            var firstColumnName = "Параметр";

            var secondColumnName = "Значение";

            var tableSize = UiHelper.DefineTableMaxSize(dic, firstColumnName, secondColumnName);

            str.AppendLine(UiHelper.MakeItStyled("Краткая информация о клане", UiTextStyle.Header));
            str.AppendLine(UiHelper.MakeItStyled(clanUi.Name + " - " + clanUi.Tag, UiTextStyle.Name));
            str.AppendLine();
            str.AppendLine(UiHelper.MakeItStyled("Шапка клана:", UiTextStyle.Subtitle));
            str.AppendLine(UiHelper.MakeItStyled(clanUi.Description, UiTextStyle.Default));
            str.AppendLine();
            str.AppendLine(UiHelper.MakeItStyled("Основные показатели:", UiTextStyle.Subtitle));
            str.AppendLine();
            str.AppendLine($"``` |{firstColumnName.PadRight(tableSize.KeyMaxLength)}|{UiHelper.GetCenteredString(secondColumnName, tableSize.ValueMaxLength)}|");
            str.AppendLine($" |{new string('-', tableSize.KeyMaxLength)}|{new string('-', tableSize.ValueMaxLength)}|");

            foreach (var item in dic)
            {
                str.Append($" |{item.Key.PadRight(tableSize.KeyMaxLength)}|");

                str.AppendLine($"{UiHelper.GetCenteredString(item.Value.ToString(), tableSize.ValueMaxLength)}|");
            }

            str.Append("```");

            return str.ToString();
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    public static string GetClanMembers(string clanTag, ICollection<TrackedClan> trackedClans)
    {
        try
        {
            var clanUi = UiHelper.GetActiveClanUi(clanTag, trackedClans);

            if (clanUi == null)
            {
                return UiHelper.Ecranize($"Клан с тегом {clanTag} не отслеживается. Введите корректный тег клана");
            }
            if (clanUi.ClanMembers.Count == 0)
            {
                return UiHelper.Ecranize($"В клане {clanTag} нет участников");
            }

            var str = new StringBuilder();

            str.AppendLine(UiHelper.MakeItStyled("Список членов клана", UiTextStyle.Header));
            str.AppendLine(UiHelper.MakeItStyled(clanUi.Name + " - " + clanUi.Tag, UiTextStyle.Name));
            str.AppendLine();

            var counter = 0;

            foreach (var member in clanUi.ClanMembers.OrderByDescending(x => x.TownHallLevel))
            {
                counter++;

                str.AppendLine(UiHelper.MakeItStyled(counter + member.Name + "[" + member.Tag + "]", UiTextStyle.Name));
            }

            return str.ToString();
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    public static string GetWarHistory(string clanTag, ICollection<TrackedClan> trackedClans, int recordsCount, string messageSplitToken)
    {
        try
        {
            if (trackedClans.FirstOrDefault(x => x.Tag == clanTag) == null || trackedClans.First(x => x.Tag == clanTag)?.ClanWars == null)
            {
                return UiHelper.Ecranize($"Клан с тегом {clanTag} не отслеживается или не принимал участия в войнах. Введите корректный тег клана");
            }

            var trackedClan = trackedClans.First(x => x.Tag == clanTag && x.IsCurrent == true);

            var clanWarsUi = new List<CwCwlUi>();

            foreach (var cw in trackedClan.ClanWars)
            {
                var members = cw.WarMembers;

                clanWarsUi.Add(Mapper.MapToUi(cw));
            }

            var str = new StringBuilder();

            var maxAttackLenght = 8;
            var maxNameLength = 12;

            str.AppendLine(UiHelper.MakeItStyled("Результаты войн клана", UiTextStyle.Header));
            str.AppendLine(UiHelper.MakeItStyled(trackedClan.Name + " - " + trackedClan.Tag, UiTextStyle.Name));
            str.AppendLine();

            var counter = 0;

            foreach (var cw in clanWarsUi.OrderByDescending(x => x.StartedOn))
            {
                if (counter < recordsCount)
                {
                    str.AppendLine($@"{messageSplitToken}");
                    str.AppendLine(UiHelper.MakeItStyled("В войне против:", UiTextStyle.Subtitle));
                    str.AppendLine(UiHelper.MakeItStyled(cw.OpponentName + " - " + cw.OpponentTag, UiTextStyle.Name));
                    str.AppendLine();
                    str.AppendLine(UiHelper.MakeItStyled("Даты войны:", UiTextStyle.Subtitle));
                    str.AppendLine(UiHelper.MakeItStyled(cw.StartedOn.ToString() + "-", UiTextStyle.Default));
                    str.AppendLine(UiHelper.MakeItStyled(cw.EndedOn.ToString(), UiTextStyle.Default));
                    str.AppendLine();
                    str.AppendLine(UiHelper.MakeItStyled("Результат:", UiTextStyle.Subtitle));
                    str.AppendLine(UiHelper.MakeItStyled(cw.Result, UiTextStyle.Default));
                    str.AppendLine();
                    str.AppendLine(UiHelper.MakeItStyled("Суммарное количество звезд:", UiTextStyle.Subtitle));
                    str.AppendLine(UiHelper.MakeItStyled(cw.TotalStarsEarned + " : " + cw.OpponentStarsCount, UiTextStyle.Default));
                    str.AppendLine(UiHelper.MakeItStyled("Суммарный процент разрушений:", UiTextStyle.Subtitle));
                    str.AppendLine(UiHelper.MakeItStyled(Math.Round(cw.DestructionPercentage, 1) + " : " +
                                                         Math.Round(cw.OpponentDestructionPercentage, 1), UiTextStyle.Default));
                    str.AppendLine();
                    str.AppendLine(UiHelper.MakeItStyled("Пояснение таблицы:", UiTextStyle.TableAnnotation));
                    str.AppendLine(UiHelper.MakeItStyled("Атаки - ТХ/Проценты/Звезды", UiTextStyle.Default));
                    str.AppendLine();
                    
                    str.AppendLine(UiHelper.MakeItStyled("Показатели атак:", UiTextStyle.Subtitle));
                    str.AppendLine();

                    str.AppendLine($"``` " +
                        $"|{UiHelper.GetCenteredString("Игрок", maxNameLength)}" +
                        $"|{UiHelper.GetCenteredString("Атака№1", maxAttackLenght)}" +
                        $"|{UiHelper.GetCenteredString("Атака№2", maxAttackLenght)}|");

                    str.AppendLine($" " +
                        $"|{new string('-', maxNameLength)}" +
                        $"|{new string('-', maxAttackLenght)}" +
                        $"|{new string('-', maxAttackLenght)}|");

                    foreach (var attack in cw.MembersResults.OrderByDescending(x => x.ThLevel))
                    {
                        var properNameLevgth = 8;
                        var shorterCoefficient = 3;
                        var properName = UiHelper.ChangeInvalidSymbols(attack.PlayerName);

                        if (properName.Length > properNameLevgth)
                        {
                            properName = properName.Substring(0, properNameLevgth);
                        }

                        str.Append($" |{attack.ThLevel} {UiHelper.GetCenteredString(properName, maxNameLength - shorterCoefficient)}|");

                        str.Append($"{UiHelper.GetCenteredString(attack.FirstEnemyThLevel + "/" + attack.FirstDestructionPercent + "/" + attack.FirstStarsCount, maxAttackLenght)}|");

                        str.AppendLine($"{UiHelper.GetCenteredString(attack.SecondEnemyThLevel + "/" + attack.SecondDestructionpercent + "/" + attack.SecondStarsCount, maxAttackLenght)}|");
                    }

                    str.Append("```\n");

                    counter++;
                }
            }

            return str.ToString();
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    public static string GetRaidsHistory(string clanTag, ICollection<TrackedClan> trackedClans, int recordsCount, string messageSplitToken)
    {
        try
        {
            if (trackedClans.FirstOrDefault(x => x.Tag == clanTag) == null || trackedClans.First(x => x.Tag == clanTag)?.CapitalRaids == null)
            {
                return UiHelper.Ecranize($"Клан с тегом {clanTag} не отслеживается или не принимал участия в рейдах. Введите корректный тег клана");
            }

            var trackedClan = trackedClans.First(x => x.Tag == clanTag && x.IsCurrent == true);

            var raidUi = new List<RaidsUi>();

            foreach (var raid in trackedClan.CapitalRaids)
            {
                raidUi.Add(Mapper.MapToUi(raid));
            }

            var str = new StringBuilder();

            var maxNameLength = 12;
            var maxDistrictLength = 9;
            var maxAttackLenght = 6;

            str.AppendLine(UiHelper.MakeItStyled("Результаты рейдов клана", UiTextStyle.Header));
            str.AppendLine(UiHelper.MakeItStyled(trackedClan.Name + " - " + trackedClan.Tag, UiTextStyle.Name));
            str.AppendLine();

            var counter = 0;

            foreach (var raid in raidUi.OrderByDescending(x => x.StartedOn))
            {
                var offensiveReward = raid.OffensiveReward * 6;
                var totalReward = offensiveReward + raid.DefensiveReward;

                if (counter < recordsCount)
                {
                    str.AppendLine($@"{messageSplitToken}");
                    str.AppendLine(UiHelper.MakeItStyled("Даты дней рейдов:", UiTextStyle.Subtitle));
                    str.AppendLine(UiHelper.MakeItStyled(raid.StartedOn + " - ", UiTextStyle.Default));
                    str.AppendLine(UiHelper.MakeItStyled(raid.EndedOn.ToString(), UiTextStyle.Default));
                    str.AppendLine();
                    str.AppendLine(UiHelper.MakeItStyled("Рейдов завершено: " + raid.RaidsCompleted, UiTextStyle.Subtitle));
                    str.AppendLine(UiHelper.MakeItStyled("Разрушено районов: " + raid.DefeatedDistrictsCount, UiTextStyle.Subtitle));
                    str.AppendLine(UiHelper.MakeItStyled("Награблено золота: " + raid.TotalCapitalLoot, UiTextStyle.Subtitle));
                    str.AppendLine();
                    str.AppendLine(UiHelper.MakeItStyled("Медали рейдов:", UiTextStyle.Subtitle));
                    str.AppendLine(UiHelper.MakeItStyled("За атаку: " + offensiveReward, UiTextStyle.Default));
                    str.AppendLine(UiHelper.MakeItStyled("За защиту: " + raid.DefensiveReward, UiTextStyle.Default));
                    str.AppendLine(UiHelper.MakeItStyled("Суммарно: " + totalReward, UiTextStyle.Default));
                    str.AppendLine();
                    str.AppendLine(UiHelper.MakeItStyled("Общие показатели обороны:", UiTextStyle.Subtitle));

                    var averageDefenses = 0.0;

                    foreach (var defense in raid.Defenses)
                    {
                        str.AppendLine(UiHelper.MakeItStyled("[" + defense.AttackersTag + "] " + "[" + defense.AttackersName + "] "
                            + "[" + defense.TotalAttacksCount + "]", UiTextStyle.Name));

                        averageDefenses += defense.TotalAttacksCount;
                    }

                    str.AppendLine(UiHelper.MakeItStyled("Выдержано атак в среднем: " + Math.Round(averageDefenses / raid.Defenses.Count, 2), UiTextStyle.Subtitle));
                    str.AppendLine();

                    str.AppendLine(UiHelper.MakeItStyled("Общие показатели нападения:", UiTextStyle.Subtitle));

                    var averageAttacks = 0.0;

                    foreach (var defeatedClan in raid.DefeatedClans)
                    {
                        str.AppendLine(UiHelper.MakeItStyled("[" + defeatedClan.ClanTag + "] " + "[" + defeatedClan.ClanName + "] " +
                            "[" + defeatedClan.TotalAttacksCount + "]", UiTextStyle.Name));

                        averageAttacks += defeatedClan.TotalAttacksCount;
                    }

                    str.AppendLine(UiHelper.MakeItStyled("Потрачено атак в среднем: " + Math.Round(averageAttacks / raid.DefeatedClans.Count, 2), UiTextStyle.Subtitle));

                    str.AppendLine($@"{messageSplitToken}");

                    str.AppendLine(UiHelper.MakeItStyled("Показатели атак:", UiTextStyle.Subtitle));
                    str.AppendLine();

                    var clanCounter = 0;

                    foreach (var defeatedClan in raid.DefeatedClans)
                    {
                        str.AppendLine($"``` " +
                       $"|{UiHelper.GetCenteredString("Игрок", maxNameLength)}" +
                       $"|{UiHelper.GetCenteredString("Район", maxDistrictLength)}" +
                       $"|{UiHelper.GetCenteredString("От-До%", maxAttackLenght)}|");

                        str.AppendLine($" " +
                            $"|{new string('-', maxNameLength)}" +
                            $"|{new string('-', maxDistrictLength)}" +
                            $"|{new string('-', maxAttackLenght)}|");

                        foreach (var district in defeatedClan.AttackedDistricts)
                        {
                            foreach (var attack in district.Attacks)
                            {
                                var properName = UiHelper.ChangeInvalidSymbols(attack.PlayerName);

                                if (properName.Length > maxNameLength)
                                {
                                    properName = properName.Substring(0, maxNameLength);
                                }

                                str.Append($" |{UiHelper.GetCenteredString(properName, maxNameLength)}|");

                                str.Append($"{UiHelper.GetCenteredString(UiHelper.GetFirstWord(district.DistrictName), maxDistrictLength)}|");

                                str.AppendLine($"{UiHelper.GetCenteredString(attack.DestructionPercentFrom + "-" + attack.DestructionPercentTo, maxAttackLenght)}|");
                            }
                        }

                        str.AppendLine($" " +
                        $"|{new string('-', maxNameLength)}" +
                        $"|{new string('-', maxDistrictLength)}" +
                        $"|{new string('-', maxAttackLenght)}|");

                        str.Append("```\n");

                        clanCounter++;

                        var optimalRecordsCount = 2;

                        if (clanCounter > 0 && clanCounter % optimalRecordsCount == 0)
                        {
                            str.AppendLine($@"{messageSplitToken}");
                        }
                    }

                    counter++;
                }
            }

            return str.ToString();
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    public static string GetMembersAverageRaidsPerfomance(string clanTag, ICollection<TrackedClan> trackedClans)
    {
        try
        {
            if (trackedClans.FirstOrDefault(x => x.Tag == clanTag) == null || trackedClans.First(x => x.Tag == clanTag)?.CapitalRaids == null)
            {
                return UiHelper.Ecranize($"Клан с тегом {clanTag} не отслеживается или не принимал участия в рейдах. Введите корректный тег клана");
            }

            var maxNameLength = 15;
            var maxDestructionPercentLength = 5;
            var maxCapitalLootLength = 5;

            var trackedClan = trackedClans.First(x => x.Tag == clanTag && x.IsCurrent == true);

            var str = new StringBuilder();

            str.AppendLine(UiHelper.MakeItStyled("Средние показатели рейдеров клана:", UiTextStyle.Header));
            str.AppendLine(UiHelper.MakeItStyled(trackedClan.Name + " - " + trackedClan.Tag, UiTextStyle.Name));
            str.AppendLine();
            str.AppendLine(UiHelper.MakeItStyled("Пояснение таблицы:", UiTextStyle.TableAnnotation));
            str.AppendLine(UiHelper.MakeItStyled("Avg % - средний процент разрушений за атаку.", UiTextStyle.Default));
            str.AppendLine(UiHelper.MakeItStyled("Loot - среднее количество золота, добываемого за рейд.", UiTextStyle.Default));
            str.AppendLine();
            str.AppendLine(UiHelper.MakeItStyled("Таблица отсортирована по убыванию среднего процента разрушений", UiTextStyle.Default));
            str.AppendLine();
            str.AppendLine(UiHelper.MakeItStyled("Средние показатели рейдеров:", UiTextStyle.Subtitle));
            str.AppendLine();

            var playersPerfomances = new List<AverageRaidsPerfomance>();

            foreach (var member in trackedClan.ClanMembers)
            {
                if (member.RaidMemberships != null && member.RaidMemberships.Count != 0)
                {
                    playersPerfomances.Add(Mapper.MapToAverageRaidsPerfomance(member.RaidMemberships));
                }
            }

            str.AppendLine($"``` " +
                      $"|{UiHelper.GetCenteredString("Игрок", maxNameLength)}" +
                      $"|{UiHelper.GetCenteredString("Avg %", maxDestructionPercentLength)}" +
                      $"|{UiHelper.GetCenteredString("Loot", maxCapitalLootLength)}|");

            str.AppendLine($" " +
                $"|{new string('-', maxNameLength)}" +
                $"|{new string('-', maxDestructionPercentLength)}" +
                $"|{new string('-', maxCapitalLootLength)}|");

            var properNameLength = maxNameLength - 3;

            foreach (var perfomance in playersPerfomances.OrderByDescending(x => x.AverageDestructionPercent))
            {
                var properName = UiHelper.ChangeInvalidSymbols(perfomance.Name);

                if (properName.Length > properNameLength)
                {
                    properName = properName.Substring(0, properNameLength);
                }

                str.Append($" |{UiHelper.GetCenteredString(properName, maxNameLength)}|");

                str.Append($"{UiHelper.GetCenteredString(perfomance.AverageDestructionPercent.ToString(), maxDestructionPercentLength)}|");

                str.AppendLine($"{UiHelper.GetCenteredString(Math.Round(perfomance.AverageCapitalLoot).ToString(), maxCapitalLootLength)}|");
            }

            str.Append("```\n");

            return str.ToString();
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    public static string GetClanSiegeMachines(string clanTag, ICollection<TrackedClan> trackedClans)
    {
        try
        {
            if (trackedClans.FirstOrDefault(x => x.Tag == clanTag) == null)
            {
                return UiHelper.Ecranize($"Клан с тегом {clanTag} не отслеживается. Введите корректный тег клана");
            }

            var trackedClan = trackedClans.First(x => x.Tag == clanTag && x.IsCurrent == true);

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

            str.AppendLine(UiHelper.MakeItStyled("Осадные машины игроков клана", UiTextStyle.Header));
            str.AppendLine(UiHelper.MakeItStyled(trackedClan.Name + " - " + trackedClan.Tag, UiTextStyle.Name));
            str.AppendLine();

            str.AppendLine(UiHelper.MakeItStyled("Пояснение таблицы", UiTextStyle.TableAnnotation));
            str.AppendLine(UiHelper.MakeItStyled(("1 - " + machineMapper.ElementAt(0).Value), UiTextStyle.Default));
            str.AppendLine(UiHelper.MakeItStyled(("2 - " + machineMapper.ElementAt(1).Value), UiTextStyle.Default));
            str.AppendLine(UiHelper.MakeItStyled(("3 - " + machineMapper.ElementAt(2).Value), UiTextStyle.Default));
            str.AppendLine(UiHelper.MakeItStyled(("4 - " + machineMapper.ElementAt(3).Value), UiTextStyle.Default));
            str.AppendLine(UiHelper.MakeItStyled(("5 - " + machineMapper.ElementAt(4).Value), UiTextStyle.Default));
            str.AppendLine(UiHelper.MakeItStyled(("6 - " + machineMapper.ElementAt(5).Value), UiTextStyle.Default));
            str.AppendLine(UiHelper.MakeItStyled(("7 - " + machineMapper.ElementAt(6).Value), UiTextStyle.Default));
            str.AppendLine();
            str.AppendLine(UiHelper.MakeItStyled("Цифры в столбцах - уровни осадных машин.", UiTextStyle.Default));
            str.AppendLine();
            str.AppendLine(UiHelper.MakeItStyled("Перечень доступных игрокам машин:", UiTextStyle.Subtitle));
            str.AppendLine();

            str.AppendLine($"``` " +
                      $"|{UiHelper.GetCenteredString("Игрок", maxNameLength)}" +
                      $"|{UiHelper.GetCenteredString("1", maxMachineLevelLength)}" +
                      $"|{UiHelper.GetCenteredString("2", maxMachineLevelLength)}" +
                      $"|{UiHelper.GetCenteredString("3", maxMachineLevelLength)}" +
                      $"|{UiHelper.GetCenteredString("4", maxMachineLevelLength)}" +
                      $"|{UiHelper.GetCenteredString("5", maxMachineLevelLength)}" +
                      $"|{UiHelper.GetCenteredString("6", maxMachineLevelLength)}" +
                      $"|{UiHelper.GetCenteredString("7", maxMachineLevelLength)}|");

            str.AppendLine($" " +
                $"|{new string('-', maxNameLength)}" +
                $"|{new string('-', maxMachineLevelLength)}" +
                $"|{new string('-', maxMachineLevelLength)}" +
                $"|{new string('-', maxMachineLevelLength)}" +
                $"|{new string('-', maxMachineLevelLength)}" +
                $"|{new string('-', maxMachineLevelLength)}" +
                $"|{new string('-', maxMachineLevelLength)}" +
                $"|{new string('-', maxMachineLevelLength)}|");

            foreach (var member in trackedClan.ClanMembers.OrderByDescending(x => x.TownHallLevel))
            {
                var memberMachines = GetAllMachineLevels(member, machineMapper.Keys.ToList());

                var properName = UiHelper.ChangeInvalidSymbols(member.Name);

                if (properName.Length > maxNameLength)
                {
                    properName = properName.Substring(0, maxNameLength);
                }

                if (memberMachines.All(x => x.Value == "0"))
                {
                    break;
                }

                str.Append($" |{UiHelper.GetCenteredString(properName, maxNameLength)}|");
                str.Append($"{UiHelper.GetCenteredString(memberMachines.First(x => x.Key == allMachines[0]).Value, maxMachineLevelLength)}|");
                str.Append($"{UiHelper.GetCenteredString(memberMachines.First(x => x.Key == allMachines[1]).Value, maxMachineLevelLength)}|");
                str.Append($"{UiHelper.GetCenteredString(memberMachines.First(x => x.Key == allMachines[2]).Value, maxMachineLevelLength)}|");
                str.Append($"{UiHelper.GetCenteredString(memberMachines.First(x => x.Key == allMachines[3]).Value, maxMachineLevelLength)}|");
                str.Append($"{UiHelper.GetCenteredString(memberMachines.First(x => x.Key == allMachines[4]).Value, maxMachineLevelLength)}|");
                str.Append($"{UiHelper.GetCenteredString(memberMachines.First(x => x.Key == allMachines[5]).Value, maxMachineLevelLength)}|");
                str.AppendLine($"{UiHelper.GetCenteredString(memberMachines.First(x => x.Key == allMachines[6]).Value, maxMachineLevelLength)}|");
            }

            str.Append("```\n");

            return str.ToString();
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    public static string GetClanActiveeSuperUnits(string clanTag, ICollection<TrackedClan> trackedClans)
    {
        try
        {
            if (trackedClans.FirstOrDefault(x => x.Tag == clanTag) == null)
            {
                return UiHelper.Ecranize($"Клан с тегом {clanTag} не отслеживается. Введите корректный тег клана");
            }

            var trackedClan = trackedClans.First(x => x.Tag == clanTag && x.IsCurrent == true);

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
                { "Super Hog Rider", "Боевой бур"},};

            str.AppendLine(UiHelper.MakeItStyled("Активные супер юниты клана", UiTextStyle.Header));
            str.AppendLine(UiHelper.MakeItStyled(trackedClan.Name + " - " + trackedClan.Tag, UiTextStyle.Name));
            str.AppendLine();

            foreach (var superUnit in superUnitsMapper)
            {
                if (trackedClan.ClanMembers.Any(x => x.Units.FirstOrDefault(x => x.Name == superUnit.Key)?.SuperTroopIsActivated == true))
                {


                    str.AppendLine(UiHelper.MakeItStyled(superUnit.Value + ":", UiTextStyle.Subtitle));

                    var members = "";

                    foreach (var member in trackedClan.ClanMembers)
                    {
                        if (member.Units.FirstOrDefault(x => x.Name == superUnit.Key)?.SuperTroopIsActivated == true)
                        {
                            var properName = member.Name;

                            if (properName.Length > maxNameLength)
                            {
                                properName = properName.Substring(0, maxNameLength);
                            }

                            members += properName + "; ";
                        }
                    }

                    str.AppendLine(UiHelper.MakeItStyled(members, UiTextStyle.Name));
                    str.AppendLine();
                }
            }

            return str.ToString();
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    public static string GetMonthStatistcs(string clanTag, ICollection<TrackedClan> trackedClans)
    {
        try
        {
            if (trackedClans.FirstOrDefault(x => x.Tag == clanTag) == null)
            {
                return UiHelper.Ecranize($"Клан с тегом {clanTag} не отслеживается. Введите корректный тег клана");
            }

            var trackedClan = trackedClans.First(x => x.Tag == clanTag && x.IsCurrent == true);
            var obsoleteClan = trackedClans.First(x => x.Tag == clanTag && x.IsCurrent == false);

            var str = new StringBuilder();

            var maxNameLength = 12;
            var maxStarsLength = 3;
            var maxCapitalLootLength = 6;
            var maxDonationsLength = 6;

            var properMembers = new Dictionary<ClanMemberUi, ClanMemberUi>();

            foreach (var clanMember in trackedClan.ClanMembers)
            {
                var currentMember = new ClanMemberUi();
                var obsoleteMember = new ClanMemberUi();

                if (obsoleteClan.ClanMembers.Any(x => x.Tag == clanMember.Tag))
                {
                    currentMember = Mapper.MapToUi(trackedClan.ClanMembers.First(x => x.Tag == clanMember.Tag));
                    obsoleteMember = Mapper.MapToUi(obsoleteClan.ClanMembers.First(x => x.Tag == clanMember.Tag));

                    properMembers.Add(currentMember, obsoleteMember);
                }
            }

            str.AppendLine(UiHelper.MakeItStyled("Сезонная статистика игроков клана", UiTextStyle.Header));
            str.AppendLine(UiHelper.MakeItStyled(trackedClan.Name + " - " + trackedClan.Tag, UiTextStyle.Name));
            str.AppendLine();
            str.AppendLine(UiHelper.MakeItStyled("Пояснение таблицы:", UiTextStyle.TableAnnotation));
            str.AppendLine(UiHelper.MakeItStyled("Зв. - боевые звезды, заработанные за сезон.", UiTextStyle.Default));
            str.AppendLine(UiHelper.MakeItStyled("Золото - столичное золото, вложенное за сезон.", UiTextStyle.Default));
            str.AppendLine(UiHelper.MakeItStyled("Донат - пожертвовано войск за сезон.", UiTextStyle.Default));
            str.AppendLine();
            str.AppendLine(UiHelper.MakeItStyled("Все показатели сбрасываются каждый рейтинговый сезон.", UiTextStyle.Default));
            str.AppendLine();

            str.AppendLine($"``` " +
                      $"|{UiHelper.GetCenteredString("Игрок", maxNameLength)}" +
                      $"|{UiHelper.GetCenteredString("Зв.", maxStarsLength)}" +
                      $"|{UiHelper.GetCenteredString("Золото", maxCapitalLootLength)}" +
                      $"|{UiHelper.GetCenteredString("Донат", maxDonationsLength)}|");

            str.AppendLine($" " +
                $"|{new string('-', maxNameLength)}" +
                $"|{new string('-', maxStarsLength)}" +
                $"|{new string('-', maxCapitalLootLength)}" +
                $"|{new string('-', maxDonationsLength)}|");

            foreach (var member in properMembers)
            {
                var properName = UiHelper.ChangeInvalidSymbols(member.Key.Name);

                if (properName.Length > maxNameLength)
                {
                    properName = properName.Substring(0, maxNameLength);
                }

                str.Append($" |{UiHelper.GetCenteredString(properName, maxNameLength)}|");

                str.Append($"{UiHelper.GetCenteredString((member.Key.WarStars - member.Value.WarStars).ToString(), maxStarsLength)}|");

                str.Append($"{UiHelper.GetCenteredString((member.Key.CapitalContributions - member.Value.CapitalContributions).ToString(), maxCapitalLootLength)}|");

                str.AppendLine($"{UiHelper.GetCenteredString((member.Key.DonationsSent - member.Value.DonationsSent).ToString(), maxDonationsLength)}|");
            }

            str.Append("```\n");

            return str.ToString();
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    private static Dictionary<string, string> GetAllMachineLevels(ClanMember member, List<string> allMachines)
    {
        var machineLevels = new Dictionary<string, string>();

        foreach (var machine in allMachines)
        {
            if (member.Units.Any(x => x.Name == machine))
            {
                machineLevels.Add(machine, member.Units.First(x => x.Name == machine).Level.ToString());
            }
            else
            {
                machineLevels.Add(machine, "0");
            }
        }

        return machineLevels;
    }
}
