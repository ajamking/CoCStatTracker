using CoCStatsTracker.UIEntities;
using CoCStatsTracker;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata.Ecma335;
using System.Collections;

namespace CoCStatsTrackerBot.Functions;

public class CurrentStatisticsFunctions
{
    public static string GetCurrentWarShortInfo(string clanTag, ICollection<TrackedClan> trackedClans)
    {
        if (trackedClans.FirstOrDefault(x => x.Tag == clanTag) == null || trackedClans.First(x => x.Tag == clanTag)?.ClanWars == null)
        {
            return "Клан с таким тегом не отслеживается или еще не принимал участия в войнах.";
        }

        var trackedClan = trackedClans.First(x => x.Tag == clanTag && x.IsCurrent == true);

        var clanWarUi = Mapper.MapToUi(trackedClan.ClanWars.OrderByDescending(x => x.EndTime).FirstOrDefault());

        var str = new StringBuilder();

        str.AppendLine($@"```  Общая информация о последней войне клана```");
        str.AppendLine($@"     *{UiHelper.Ecranize(trackedClan.Name + " - " + trackedClan.Tag)}*");
        str.AppendLine();

        str.AppendLine($@"```  Противник```");
        str.AppendLine($@"     *{UiHelper.Ecranize(clanWarUi.OpponentName + " - " + clanWarUi.OpponentTag)}*");
        str.AppendLine();
        str.AppendLine($@"```  Даты войны:```");
        str.AppendLine($@"     *{UiHelper.Ecranize(clanWarUi.StartedOn + " - ")}*");
        str.AppendLine($@"     *{UiHelper.Ecranize(clanWarUi.EndedOn.ToString())}*");
        str.AppendLine();
        str.AppendLine($@"```  Состояние:```");
        str.AppendLine($@"     *{UiHelper.Ecranize(clanWarUi.Result)}*");
        str.AppendLine();
        str.AppendLine($@"     Доступно атак участникам \- {clanWarUi.AttackPerMember}");
        str.AppendLine();
        str.AppendLine($@"```  Суммарное число атак:```");
        str.AppendLine($@"     *{UiHelper.Ecranize(clanWarUi.AttacksCount + " : " + clanWarUi.OpponentAttacksCount)}*");
        str.AppendLine($@"```  Суммарное количество звезд:```");
        str.AppendLine($@"     *{UiHelper.Ecranize(clanWarUi.TotalStarsEarned + " : " + clanWarUi.OpponentStarsCount)}*");
        str.AppendLine($@"```  Суммарный процент разрушений:```");
        str.AppendLine($@"     *{UiHelper.Ecranize(Math.Round(clanWarUi.DestructionPercentage, 1) + " : " + Math.Round(clanWarUi.OpponentDestructionPercentage, 1))}*");
        str.AppendLine();

        return str.ToString();
    }

    public static string GetCurrentWarMap(string clanTag, ICollection<TrackedClan> trackedClans)
    {
        if (trackedClans.FirstOrDefault(x => x.Tag == clanTag) == null || trackedClans.First(x => x.Tag == clanTag)?.ClanWars == null)
        {
            return "Клан с таким тегом не отслеживается или еще не принимал участия в войнах.";
        }

        var trackedClan = trackedClans.First(x => x.Tag == clanTag && x.IsCurrent == true);

        var clanWar = trackedClan.ClanWars.OrderByDescending(x => x.StartTime).FirstOrDefault();

        var maxNameLength = 8;

        var warMembers = clanWar?.WarMembers.OrderBy(x => x.MapPosition).ToList();

        var enemyWarMembers = clanWar?.EnemyWarMembers.OrderBy(x => x.MapPosition).ToList();

        var str = new StringBuilder();

        str.AppendLine($@"```  Карта текущей войны клана```");
        str.AppendLine($@"     *{UiHelper.Ecranize(trackedClan.Name + " - " + trackedClan.Tag)}*");
        str.AppendLine();
        str.AppendLine($@"```  Противник```");
        str.AppendLine($@"     *{UiHelper.Ecranize(clanWar.OpponentClanName + " - " + clanWar.OpponentClanTag)}*");
        str.AppendLine();
        str.AppendLine($@"```  Даты войны:```");
        str.AppendLine($@"     *{UiHelper.Ecranize(clanWar.StartTime + " - ")}*");
        str.AppendLine($@"     *{UiHelper.Ecranize(clanWar.EndTime.ToString())}*");
        str.AppendLine();

        for (int i = 0; i <= warMembers?.Count; i++)
        {
            try
            {
                var mate = warMembers[i];

                var opponent = enemyWarMembers?[i];

                var properMateName = UiHelper.ChangeInvalidSymbols(mate.Name);

                var properOpponentName = UiHelper.ChangeInvalidSymbols(opponent.Name);

                var position = UiHelper.GetCenteredString((i + 1).ToString(), 2);

                if (properMateName.Length >= maxNameLength)
                {
                    properMateName = properMateName.Substring(0, maxNameLength);
                }

                properMateName = UiHelper.GetCenteredString(properMateName, maxNameLength);

                if (properOpponentName.Length >= maxNameLength)
                {
                    properOpponentName = properOpponentName.Substring(0, maxNameLength);
                }

                properOpponentName = UiHelper.GetCenteredString(properOpponentName, maxNameLength);

                var mapStr = $@"{properMateName} {mate.TownHallLevel} |{position}| {opponent.THLevel} {properOpponentName}";

                str.AppendLine($@"``` {UiHelper.Ecranize(mapStr)}```");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        return str.ToString();
    }


    public static string GetCurrentRaidShortInfo(string clanTag, ICollection<TrackedClan> trackedClans)
    {
        if (trackedClans.FirstOrDefault(x => x.Tag == clanTag) == null || trackedClans.First(x => x.Tag == clanTag)?.CapitalRaids == null)
        {
            return "Клан с таким тегом не отслеживается или еще не принимал участия в рейдах.";
        }

        var trackedClan = trackedClans.First(x => x.Tag == clanTag && x.IsCurrent == true);

        var raid = Mapper.MapToUi(trackedClan.CapitalRaids.OrderByDescending(x => x.StartedOn).FirstOrDefault());

        var str = new StringBuilder();

        var totalAttacksCount = 0;

        var offensiveReward = raid.OffensiveReward * 6;

        var totalReward = offensiveReward + raid.DefensiveReward;

        foreach (var defeatedClan in raid.DefeatedClans)
        {
            totalAttacksCount += defeatedClan.TotalAttacksCount;
        }

        str.AppendLine($@"```  Общая информация о последнем рейде клана```");
        str.AppendLine($@"     *{UiHelper.Ecranize(trackedClan.Name + " - " + trackedClan.Tag)}*");
        str.AppendLine();

        str.AppendLine($@"```  Даты дней рейдов:```");
        str.AppendLine($@"     *{UiHelper.Ecranize(raid.StartedOn + " - ")}*");
        str.AppendLine($@"     *{UiHelper.Ecranize(raid.EndedOn.ToString())}*");
        str.AppendLine();
        str.AppendLine($@"     Проведено атак: *{totalAttacksCount}*");
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

        return str.ToString();
    }

    public static string GetCDistrictStatistics(string clanTag, ICollection<TrackedClan> trackedClans, DistrictType districtType)
    {
        if (trackedClans.FirstOrDefault(x => x.Tag == clanTag) == null || trackedClans.First(x => x.Tag == clanTag)?.CapitalRaids == null)
        {
            return "Клан с таким тегом не отслеживается или еще не принимал участия в рейдах.";
        }

        var trackedClan = trackedClans.First(x => x.Tag == clanTag && x.IsCurrent == true);

        var raid = Mapper.MapToUi(trackedClan.CapitalRaids.OrderByDescending(x => x.StartedOn).FirstOrDefault());


        var maxNameLength = 18;
        var max2ColumnLength = 3;
        var max3ColumnLength = 3;

        var districtMapper = new Dictionary<DistrictType, string>()
        {
            {DistrictType.Capital_Peak, "Capital Peak" },
            {DistrictType.Barbarian_Camp, "Barbarian Camp" },
            {DistrictType.Wizard_Valley, "Wizard Valley" },
            {DistrictType.Balloon_Lagoon, "Balloon Lagoon" },
            {DistrictType.Builders_Workshop, "Builder's Workshop" },
            {DistrictType.Dragon_Cliffs, "Dragon Cliffs" },
            {DistrictType.Golem_Quarry, "Golem Quarry" },
            {DistrictType.Skeleton_Park, "Skeleton Park" },
        };

        var chosenDistrictName = districtMapper.First(x => x.Key == districtType).Value;

        var avgPercent = 0.0;
        var counter = 0;

        foreach (var clan in raid.DefeatedClans)
        {
            foreach (var district in clan.AttackedDistricts.Where(x => x.DistrictName == chosenDistrictName))
            {
                foreach (var attack in district.Attacks)
                {
                    avgPercent += attack.DestructionPercentTo - attack.DestructionPercentFrom;
                    counter++;
                }

            }
        }

        try
        {
            avgPercent = Math.Round(avgPercent / counter, 2);
        }
        catch (Exception e)
        {

        }

        var str = new StringBuilder();

        str.AppendLine($@"```  Показатели игроков клана```");
        str.AppendLine($@"     *{UiHelper.Ecranize(trackedClan.Name + " - " + trackedClan.Tag)}*");
        str.AppendLine();
        str.AppendLine($@"```  В атаках на район```");
        str.AppendLine($@"     *{UiHelper.Ecranize(chosenDistrictName)}*");
        str.AppendLine($@"     *{UiHelper.Ecranize("Разрушений за атаку в среднем: " + avgPercent + "%")}*");
        str.AppendLine();
        str.AppendLine($"``` " +
                           $"|{UiHelper.GetCenteredString("Игрок", maxNameLength)}" +
                           $"|{UiHelper.GetCenteredString("%От", max2ColumnLength)}" +
                           $"|{UiHelper.GetCenteredString("%До", max3ColumnLength)}|");

        str.AppendLine($" " +
            $"|{new string('-', maxNameLength)}" +
            $"|{new string('-', max2ColumnLength)}" +
            $"|{new string('-', max3ColumnLength)}|");

        foreach (var clan in raid.DefeatedClans)
        {
            foreach (var district in clan.AttackedDistricts.Where(x => x.DistrictName == chosenDistrictName))
            {
                foreach (var attack in district.Attacks)
                {
                    var properName = UiHelper.ChangeInvalidSymbols(attack.PlayerName);

                    if (properName.Length >= maxNameLength)
                    {
                        properName = properName.Substring(0, maxNameLength);
                    }

                    str.Append($" |{UiHelper.GetCenteredString(properName, maxNameLength)}|");

                    str.Append($"{UiHelper.GetCenteredString(attack.DestructionPercentFrom.ToString(), max2ColumnLength)}|");

                    str.AppendLine($"{UiHelper.GetCenteredString(attack.DestructionPercentTo.ToString(), max3ColumnLength)}|");
                }
            }

            str.AppendLine($" " +
            $"|{new string('-', maxNameLength)}" +
            $"|{new string('-', max2ColumnLength)}" +
            $"|{new string('-', max3ColumnLength)}|");
        }

        str.Append("```\n");

        return str.ToString();
    }


    public enum DistrictType
    {
        Capital_Peak,
        Barbarian_Camp,
        Wizard_Valley,
        Balloon_Lagoon,
        Builders_Workshop,
        Dragon_Cliffs,
        Golem_Quarry,
        Skeleton_Park,
    }
}
