using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Resource;
using CoCApiDealer.UIEntities;
using CoCStatsTracker;
using CoCStatsTracker.ApiEntities;
using CoCStatsTracker.UIEntities;
using Domain.Entities;
using Telegram.Bot.Requests;

namespace CoCStatsTrackerBot;

public static class ClanFunctions
{
    public static string GetClanShortInfo(string clanTag, ICollection<TrackedClan> trackedClans)
    {
        var clanUi = GetActiveClanUi(clanTag, trackedClans);

        if (clanUi == null)
        {
            return "Клан с таким тегом не отслеживается, введите тег заново";
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

        str.AppendLine($@"```  Краткая информация о клане```");
        str.AppendLine($@"     *{UiHelper.Ecranize(clanUi.Name + " - " + clanUi.Tag)}*");
        str.AppendLine();
        str.AppendLine($@"```  Описание:```");
        str.AppendLine($@"{UiHelper.Ecranize(clanUi.Description)}");
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

    public static string GetClanMembers(string clanTag, ICollection<TrackedClan> trackedClans)
    {
        var clanUi = GetActiveClanUi(clanTag, trackedClans);

        if (clanUi == null)
        {
            return "Клан с таким тегом не отслеживается, введите тег заново";
        }

        var str = new StringBuilder();

        str.AppendLine($@"``` Список членов клана```");
        str.AppendLine($@"*{UiHelper.Ecranize(clanUi.Name + " - " + clanUi.Tag)}*");
        str.AppendLine();

        var maxNameLength = 0;

        foreach (var member in clanUi.ClanMembers)
        {
            if (member.Name.Length >= maxNameLength)
            {
                maxNameLength = member.Name.Length;
            }
        }

        var counter = 0;

        foreach (var member in clanUi.ClanMembers.OrderByDescending(x => x.TownHallLevel))
        {
            counter++;

            str.AppendLine($@"{counter} *{UiHelper.Ecranize(member.Name)}* {UiHelper.Ecranize("[" + member.Tag + "]")}");
        }

        return str.ToString();
    }

    public static string GetWarHistory(string clanTag, ICollection<TrackedClan> trackedClans, int recordsCount, string messageSplitToken)
    {
        if (trackedClans.FirstOrDefault(x => x.Tag == clanTag) == null || trackedClans.First(x => x.Tag == clanTag)?.ClanWars == null)
        {
            return "Клан с таким тегом не отслеживается или еще не принимал участия в войнах.";
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

        str.AppendLine($@"```  Результаты войн клана```");
        str.AppendLine($@"     *{UiHelper.Ecranize(trackedClan.Name + " - " + trackedClan.Tag)}*");
        str.AppendLine();

        var counter = 0;

        foreach (var cw in clanWarsUi.OrderByDescending(x => x.StartedOn))
        {
            if (counter < recordsCount)
            {
                str.AppendLine($@"{messageSplitToken}");
                str.AppendLine($@"```  В войне против```");
                str.AppendLine($@"     *{UiHelper.Ecranize(cw.OpponentName + " - " + cw.OpponentTag)}*");
                str.AppendLine();
                str.AppendLine($@"```  Даты войны:```");
                str.AppendLine($@"     *{UiHelper.Ecranize(cw.StartedOn + " - ")}*");
                str.AppendLine($@"     *{UiHelper.Ecranize(cw.EndedOn.ToString())}*");
                str.AppendLine();
                str.AppendLine($@"```  Результат:```");
                str.AppendLine($@"     *{UiHelper.Ecranize(cw.Result)}*");
                str.AppendLine();
                str.AppendLine($@"```  Суммарное количество звезд:```");
                str.AppendLine($@"     *{UiHelper.Ecranize(cw.TotalStarsEarned + " : " + cw.OpponentStarsCount)}*");
                str.AppendLine($@"```  Суммарный процент разрушений:```");
                str.AppendLine($@"     *{UiHelper.Ecranize(Math.Round(cw.DestructionPercentage, 1) + " : " + Math.Round(cw.OpponentDestructionPercentage, 1))}*");
                str.AppendLine();
                str.AppendLine($@"```  Показатели атак:```");
                str.AppendLine($@"     {UiHelper.Ecranize("Атаки - ТХ/Проценты/Звезды")}");
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
                    var properName = UiHelper.ChangeInvalidSymbols(attack.PlayerName);

                    if (properName.Length > properNameLevgth)
                    {
                        properName = properName.Substring(0, properNameLevgth);
                    }

                    str.Append($" |{attack.ThLevel} {UiHelper.GetCenteredString(properName, maxNameLength - 3)}|");

                    str.Append($"{UiHelper.GetCenteredString(attack.FirstEnemyThLevel + "/" + attack.FirstDestructionPercent + "/" + attack.FirstStarsCount, maxAttackLenght)}|");

                    str.AppendLine($"{UiHelper.GetCenteredString(attack.SecondEnemyThLevel + "/" + attack.SecondDestructionpercent + "/" + attack.SecondStarsCount, maxAttackLenght)}|");
                }

                str.Append("```\n");

                counter++;
            }
        }

        return str.ToString();
    }

    public static string GetRaidsHistory(string clanTag, ICollection<TrackedClan> trackedClans, int recordsCount, string messageSplitToken)
    {
        if (trackedClans.FirstOrDefault(x => x.Tag == clanTag) == null || trackedClans.First(x => x.Tag == clanTag)?.CapitalRaids == null)
        {
            return "Клан с таким тегом не отслеживается или еще не принимал участия в рейдах.";
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


        str.AppendLine($@"```  Результаты рейдов клана```");
        str.AppendLine($@"     *{UiHelper.Ecranize(trackedClan.Name + " - " + trackedClan.Tag)}*");
        str.AppendLine();

        var counter = 0;

        foreach (var raid in raidUi.OrderByDescending(x => x.StartedOn))
        {
            var offensiveReward = raid.OffensiveReward * 6;
            var totalReward = offensiveReward + raid.DefensiveReward;

            if (counter < recordsCount)
            {
                str.AppendLine($@"{messageSplitToken}");
                str.AppendLine($@"```  Даты дней рейдов:```");
                str.AppendLine($@"     *{UiHelper.Ecranize(raid.StartedOn + " - ")}*");
                str.AppendLine($@"     *{UiHelper.Ecranize(raid.EndedOn.ToString())}*");
                str.AppendLine();
                str.AppendLine($@"     Повержено кланов: *{raid.RaidsCompleted}*");
                str.AppendLine($@"     Разрушено районов: *{raid.DefeatedDistrictsCount}*");
                str.AppendLine($@"     Награблено золота: *{raid.TotalCapitalLoot}*");
                str.AppendLine();
                str.AppendLine($@"```  Медали рейдов:```");
                str.AppendLine($@"     За атаку: {offensiveReward}");
                str.AppendLine($@"     За защиту: {raid.DefensiveReward}");
                str.AppendLine($@"     Суммарно: {totalReward}");
                str.AppendLine();

                str.AppendLine($@"```  Общие показатели обороны:```");

                var averageDefenses = 0.0;

                foreach (var defense in raid.Defenses)
                {
                    str.AppendLine($@"     {UiHelper.Ecranize("[" + defense.AttackersTag + "] " + "[" + defense.AttackersName + "] " + "[" + defense.TotalAttacksCount + "]")}");

                    averageDefenses += defense.TotalAttacksCount;
                }

                str.AppendLine($@"     Выдержано атак в среднем: {Math.Round(averageDefenses / raid.Defenses.Count, 2)}");
                str.AppendLine();

                str.AppendLine($@"```  Общие показатели нападения:```");

                var averageAttacks = 0.0;

                foreach (var defeatedClan in raid.DefeatedClans)
                {
                    str.AppendLine($@"     {UiHelper.Ecranize("[" + defeatedClan.ClanTag + "] " + "[" + defeatedClan.ClanName + "] " + "[" + defeatedClan.TotalAttacksCount + "]")}");

                    averageAttacks += defeatedClan.TotalAttacksCount;
                }

                str.AppendLine($@"     Потрачено атак в среднем: {Math.Round(averageAttacks / raid.DefeatedClans.Count, 2)}");

                str.AppendLine($@"{messageSplitToken}");
                str.AppendLine($@"```  Показатели атак:```");
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

                            str.Append($"{UiHelper.GetCenteredString(UiHelper.GetTrimmedString(district.DistrictName), maxDistrictLength)}|");

                            str.AppendLine($"{UiHelper.GetCenteredString(attack.DestructionPercentFrom + "-" + attack.DestructionPercentTo, maxAttackLenght)}|");
                        }
                    }

                    str.AppendLine($" " +
                    $"|{new string('-', maxNameLength)}" +
                    $"|{new string('-', maxDistrictLength)}" +
                    $"|{new string('-', maxAttackLenght)}|");

                    str.Append("```\n");

                    clanCounter++;

                    if (clanCounter > 0 && clanCounter % 2 == 0)
                    {
                        str.AppendLine($@"{messageSplitToken}");
                    }
                }

                counter++;
            }
        }

        return str.ToString();
    }

    public static string GetMembersAverageRaidsPerfomance(string clanTag, ICollection<TrackedClan> trackedClans, string messageSplitToken)
    {
        if (trackedClans.FirstOrDefault(x => x.Tag == clanTag) == null || trackedClans.First(x => x.Tag == clanTag)?.CapitalRaids == null)
        {
            return "Клан с таким тегом не отслеживается или еще не принимал участия в рейдах.";
        }

        var maxNameLength = 15;
        var maxDestructionPercentLength = 5;
        var maxCapitalLootLength = 5;

        var trackedClan = trackedClans.First(x => x.Tag == clanTag && x.IsCurrent == true);

        var str = new StringBuilder();

        str.AppendLine($@"*Средние показатели рейдеров клана:*");
        str.AppendLine($@"*{UiHelper.Ecranize(trackedClan.Name + " - " + trackedClan.Tag)}*");
        str.AppendLine();
        str.AppendLine($@"*{UiHelper.Ecranize("Avg % - средний процент разрушений за атаку.")}*");
        str.AppendLine($@"*{UiHelper.Ecranize("Loot - среднее количество золота, добываемого за рейд.")}*");
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

            str.AppendLine($"{UiHelper.GetCenteredString(perfomance.AverageCapitalLoot.ToString(), maxCapitalLootLength)}|");
        }

        str.Append("```\n");

        return str.ToString();
    }

    public static string GetClanSiegeMachines(string clanTag, ICollection<TrackedClan> trackedClans)
    {
        if (trackedClans.FirstOrDefault(x => x.Tag == clanTag) == null)
        {
            return "Клан с таким тегом не отслеживается.";
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

        str.AppendLine($@"```  Осадные машины игроков клана```");
        str.AppendLine($@"     *{UiHelper.Ecranize(trackedClan.Name + " - " + trackedClan.Tag)}*");
        str.AppendLine();
        str.AppendLine($@"```  Пояснение таблицы:```");

        str.AppendLine($@"     *{UiHelper.Ecranize("1 - " + machineMapper.ElementAt(0).Value)}*");
        str.AppendLine($@"     *{UiHelper.Ecranize("2 - " + machineMapper.ElementAt(1).Value)}*");
        str.AppendLine($@"     *{UiHelper.Ecranize("3 - " + machineMapper.ElementAt(2).Value)}*");
        str.AppendLine($@"     *{UiHelper.Ecranize("4 - " + machineMapper.ElementAt(3).Value)}*");
        str.AppendLine($@"     *{UiHelper.Ecranize("5 - " + machineMapper.ElementAt(4).Value)}*");
        str.AppendLine($@"     *{UiHelper.Ecranize("6 - " + machineMapper.ElementAt(5).Value)}*");
        str.AppendLine($@"     *{UiHelper.Ecranize("7 - " + machineMapper.ElementAt(6).Value)}*");
        str.AppendLine();
        str.AppendLine($@"     *{UiHelper.Ecranize("Цифры в столбцах - уровни осадных машин.")}*");
        str.AppendLine();

        str.AppendLine($@"```  Список доступных игрокам машин:```");
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

    public static string GetClanActiveeSuperUnits(string clanTag, ICollection<TrackedClan> trackedClans)
    {
        if (trackedClans.FirstOrDefault(x => x.Tag == clanTag) == null)
        {
            return "Клан с таким тегом не отслеживается.";
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

        str.AppendLine($@"``` Активные супер юниты клана```");
        str.AppendLine($@"   *{UiHelper.Ecranize(trackedClan.Name + " - " + trackedClan.Tag)}*");
        str.AppendLine();

        foreach (var superUnit in superUnitsMapper)
        {
            if (trackedClan.ClanMembers.Any(x => x.Units.FirstOrDefault(x => x.Name == superUnit.Key)?.SuperTroopIsActivated == true))
            {
                str.AppendLine($@"``` {UiHelper.Ecranize(superUnit.Value + ":")}```");

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

                str.AppendLine($@"{UiHelper.Ecranize(members)}");
                str.AppendLine();
            }


        }

        return str.ToString();
    }

    public static string GetMonthStatistcs(string clanTag, ICollection<TrackedClan> trackedClans)
    {
        if (trackedClans.FirstOrDefault(x => x.Tag == clanTag) == null)
        {
            return "Клан с таким тегом не отслеживается.";
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

        str.AppendLine($@"``` Сезонная статистика игроков клана```");
        str.AppendLine($@"   *{UiHelper.Ecranize(trackedClan.Name + " - " + trackedClan.Tag)}*");
        str.AppendLine();
        str.AppendLine($@"``` Пояснение таблицы:```");
        str.AppendLine($@"*{UiHelper.Ecranize("Зв. - боевые звезды, заработанные за сезон.")}*");
        str.AppendLine($@"*{UiHelper.Ecranize("Золото - столичное золото, вложенное за сезон.")}*");
        str.AppendLine($@"*{UiHelper.Ecranize("Донат - пожертвовано войск за сезон.")}*");
        str.AppendLine();
        str.AppendLine($@"*{UiHelper.Ecranize("Все показатели сбрасываются каждый рейтинговый сезон.")}*");
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

    private static ClanUi GetActiveClanUi(string clanTag, ICollection<TrackedClan> trackedClans)
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
