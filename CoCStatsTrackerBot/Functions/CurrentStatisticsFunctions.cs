using CoCStatsTracker;
using CoCStatsTracker.UIEntities;
using Domain.Entities;
using System.Text;

namespace CoCStatsTrackerBot;

public class CurrentStatisticsFunctions
{
    public static string GetCurrentWarShortInfo(string clanTag, ICollection<TrackedClan> trackedClans)
    {
        if (trackedClans.FirstOrDefault(x => x.Tag == clanTag) == null)
        {
            return UiHelper.Ecranize($"Клан с тегом {clanTag} не отслеживается. Введите корректный тег клана");
        }

        var trackedClan = trackedClans.First(x => x.Tag == clanTag && x.IsCurrent == true);

        if (trackedClan?.ClanWars.Count == 0)
        {
            return UiHelper.Ecranize($"Нет записей о войнах клана с тегом {clanTag} ");
        }

        var clanWarUi = Mapper.MapToUi(trackedClan.ClanWars.OrderByDescending(x => x.EndedOn).FirstOrDefault());

        var str = new StringBuilder();

        str.AppendLine(UiHelper.MakeItStyled("Общая информация о последней войне клана", UiTextStyle.Header));
        str.AppendLine(UiHelper.MakeItStyled(trackedClan.Name + " - " + trackedClan.Tag, UiTextStyle.Name));
        str.AppendLine();
        str.AppendLine(UiHelper.MakeItStyled("Противник: ", UiTextStyle.Subtitle));
        str.AppendLine(UiHelper.MakeItStyled(clanWarUi.OpponentName + " - " + clanWarUi.OpponentTag, UiTextStyle.Name));
        str.AppendLine();
        str.AppendLine(UiHelper.MakeItStyled("Даты войны: ", UiTextStyle.Subtitle));
        str.AppendLine(UiHelper.MakeItStyled(clanWarUi.StartedOn + " - ", UiTextStyle.Default));
        str.AppendLine(UiHelper.MakeItStyled(clanWarUi.EndedOn.ToString(), UiTextStyle.Default));
        str.AppendLine();
        str.AppendLine(UiHelper.MakeItStyled("Состояние: ", UiTextStyle.Subtitle));
        str.AppendLine(UiHelper.MakeItStyled(clanWarUi.Result, UiTextStyle.Default));
        str.AppendLine();
        str.AppendLine(UiHelper.MakeItStyled("Доступно атак участникам - " + clanWarUi.AttackPerMember, UiTextStyle.Default));
        str.AppendLine();
        str.AppendLine(UiHelper.MakeItStyled("Суммарное число атак: ", UiTextStyle.Subtitle));
        str.AppendLine(UiHelper.MakeItStyled(clanWarUi.AttacksCount + " : " + clanWarUi.OpponentAttacksCount, UiTextStyle.Default));
        str.AppendLine(UiHelper.MakeItStyled("Суммарное количество звезд: ", UiTextStyle.Subtitle));
        str.AppendLine(UiHelper.MakeItStyled(clanWarUi.TotalStarsEarned + " : " + clanWarUi.OpponentStarsCount, UiTextStyle.Default));
        str.AppendLine(UiHelper.MakeItStyled("Суммарный процент разрушений: ", UiTextStyle.Subtitle));
        str.AppendLine(UiHelper.MakeItStyled(Math.Round(clanWarUi.DestructionPercentage, 1) + " : " + Math.Round(clanWarUi.OpponentDestructionPercentage, 1), UiTextStyle.Default));
        str.AppendLine();
        str.AppendLine(GetNonAttackersCw(clanWarUi));

        return str.ToString();
    }

    public static string GetCurrentWarMap(string clanTag, ICollection<TrackedClan> trackedClans)
    {
        if (trackedClans.FirstOrDefault(x => x.Tag == clanTag) == null)
        {
            return UiHelper.Ecranize($"Клан с тегом {clanTag} не отслеживается. Введите корректный тег клана");
        }

        var trackedClan = trackedClans.First(x => x.Tag == clanTag && x.IsCurrent == true);

        if (trackedClan?.ClanWars.Count == 0)
        {
            return UiHelper.Ecranize($"Нет записей о войнах клана с тегом {clanTag} ");
        }

        var clanWar = trackedClan.ClanWars.OrderByDescending(x => x.StartedOn).FirstOrDefault();

        var maxNameLength = 8;

        var warMembers = clanWar.WarMembers.OrderBy(x => x.MapPosition).ToList();

        var enemyWarMembers = clanWar.EnemyWarMembers.OrderBy(x => x.MapPosition).ToList();

        var str = new StringBuilder();

        str.AppendLine(UiHelper.MakeItStyled("Карта текущей войны клана", UiTextStyle.Header));
        str.AppendLine(UiHelper.MakeItStyled(trackedClan.Name + " - " + trackedClan.Tag, UiTextStyle.Name));
        str.AppendLine();
        str.AppendLine(UiHelper.MakeItStyled("Противник:", UiTextStyle.Subtitle));
        str.AppendLine(UiHelper.MakeItStyled(clanWar.OpponentClanName + " - " + clanWar.OpponentClanTag, UiTextStyle.Name));
        str.AppendLine();
        str.AppendLine(UiHelper.MakeItStyled("Даты войны::", UiTextStyle.Subtitle));
        str.AppendLine(UiHelper.MakeItStyled(clanWar.StartedOn + " - ", UiTextStyle.Default));
        str.AppendLine(UiHelper.MakeItStyled(clanWar.EndedOn.ToString(), UiTextStyle.Default));
        str.AppendLine();

        for (int i = 0; i <= warMembers?.Count; i++)
        {
            try
            {
                var mate = warMembers[i];

                var opponent = enemyWarMembers[i];

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
        if (trackedClans.FirstOrDefault(x => x.Tag == clanTag) == null)
        {
            return UiHelper.Ecranize($"Клан с тегом {clanTag} не отслеживается. Введите корректный тег клана");
        }

        var trackedClan = trackedClans.First(x => x.Tag == clanTag && x.IsCurrent == true);

        if (trackedClan?.CapitalRaids.Count == 0)
        {
            return UiHelper.Ecranize($"Нет записей о рейдах клана с тегом {clanTag} ");
        }

        var raid = Mapper.MapToUi(trackedClan.CapitalRaids.OrderByDescending(x => x.StartedOn).FirstOrDefault());

        var str = new StringBuilder();

        var totalAttacksCount = 0;

        var offensiveReward = raid.OffensiveReward * 6;

        var totalReward = offensiveReward + raid.DefensiveReward;

        foreach (var defeatedClan in raid.DefeatedClans)
        {
            totalAttacksCount += defeatedClan.TotalAttacksCount;
        }

        str.AppendLine(UiHelper.MakeItStyled("Общая информация о последнем рейде клана", UiTextStyle.Header));
        str.AppendLine(UiHelper.MakeItStyled(trackedClan.Name + " - " + trackedClan.Tag, UiTextStyle.Name));
        str.AppendLine();
        str.AppendLine(UiHelper.MakeItStyled("Даты дней рейдов:", UiTextStyle.Subtitle));
        str.AppendLine(UiHelper.MakeItStyled(raid.StartedOn + " - ", UiTextStyle.Default));
        str.AppendLine(UiHelper.MakeItStyled(raid.EndedOn.ToString(), UiTextStyle.Default));
        str.AppendLine();
        str.AppendLine(UiHelper.MakeItStyled("Проведено атак: " + totalAttacksCount, UiTextStyle.Subtitle));
        str.AppendLine(UiHelper.MakeItStyled("Повержено кланов: " + raid.RaidsCompleted, UiTextStyle.Subtitle));
        str.AppendLine(UiHelper.MakeItStyled("Разрушено районов: " + raid.DefeatedDistrictsCount, UiTextStyle.Subtitle));
        str.AppendLine(UiHelper.MakeItStyled("Награблено золота: " + raid.TotalCapitalLoot, UiTextStyle.Subtitle));
        str.AppendLine();
        str.AppendLine(UiHelper.MakeItStyled("Медали рейдов: ", UiTextStyle.Subtitle));
        str.AppendLine(UiHelper.MakeItStyled("За атаку: " + offensiveReward, UiTextStyle.Default));
        str.AppendLine(UiHelper.MakeItStyled("За защиту: " + raid.DefensiveReward, UiTextStyle.Default));
        str.AppendLine(UiHelper.MakeItStyled("Суммарно: " + totalReward, UiTextStyle.Default));
        str.AppendLine();
        str.AppendLine(UiHelper.MakeItStyled("Общие показатели обороны: ", UiTextStyle.Subtitle));

        var averageDefenses = 0.0;

        foreach (var defense in raid.Defenses)
        {
            str.AppendLine(UiHelper.MakeItStyled("[" + defense.AttackersTag + "] " + "[" + defense.AttackersName + "] " +
                "[" + defense.TotalAttacksCount + "]", UiTextStyle.Name));

            averageDefenses += defense.TotalAttacksCount;
        }

        str.AppendLine(UiHelper.MakeItStyled("Выдержано атак в среднем: " + Math.Round(averageDefenses / raid.Defenses.Count, 2), UiTextStyle.Subtitle));
        str.AppendLine();
        str.AppendLine(UiHelper.MakeItStyled("Общие показатели нападения: ", UiTextStyle.Subtitle));

        var averageAttacks = 0.0;

        foreach (var defeatedClan in raid.DefeatedClans)
        {
            str.AppendLine(UiHelper.MakeItStyled("[" + defeatedClan.ClanTag + "] " + "[" + defeatedClan.ClanName + "] " +
                "[" + defeatedClan.TotalAttacksCount + "]", UiTextStyle.Name));

            averageAttacks += defeatedClan.TotalAttacksCount;
        }

        str.AppendLine(UiHelper.MakeItStyled("Потрачено атак в среднем: " + Math.Round(averageAttacks / raid.DefeatedClans.Count, 2), UiTextStyle.Subtitle));
        str.AppendLine();
        str.AppendLine(GetNonAttackersRaids(trackedClan));
        str.AppendLine();

        return str.ToString();
    }

    public static string GetCDistrictStatistics(string clanTag, ICollection<TrackedClan> trackedClans, DistrictType districtType)
    {
        if (trackedClans.FirstOrDefault(x => x.Tag == clanTag) == null)
        {
            return UiHelper.Ecranize($"Клан с тегом {clanTag} не отслеживается. Введите корректный тег клана");
        }

        var trackedClan = trackedClans.First(x => x.Tag == clanTag && x.IsCurrent == true);

        if (trackedClan?.CapitalRaids.Count == 0)
        {
            return UiHelper.Ecranize($"Нет записей о рейдах клана с тегом {clanTag} ");
        }

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
            Console.WriteLine(e.Message);
        }

        var str = new StringBuilder();

        str.AppendLine(UiHelper.MakeItStyled("Показатели игроков клана", UiTextStyle.Header));
        str.AppendLine(UiHelper.MakeItStyled(trackedClan.Name + " - " + trackedClan.Tag, UiTextStyle.Name));
        str.AppendLine();
        str.AppendLine(UiHelper.MakeItStyled("В атаках на район", UiTextStyle.Subtitle));
        str.AppendLine(UiHelper.MakeItStyled(chosenDistrictName, UiTextStyle.Name));
        str.AppendLine();
        str.AppendLine(UiHelper.MakeItStyled("Разрушений за атаку в среднем: " + avgPercent + "%", UiTextStyle.Default));
        str.AppendLine();
        str.AppendLine(UiHelper.MakeItStyled("Показатели атак", UiTextStyle.Subtitle));
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


    public static string GetNonAttackersCw(CwCwlUi clanWar)
    {
        try
        {
            var str = new StringBuilder();

            var count = 0;

            if (clanWar.MembersResults.Any(x => x.FirstDestructionPercent != 0))
            {
                str.AppendLine(UiHelper.MakeItStyled("Не провели атаки на КВ: ", UiTextStyle.Subtitle));

                foreach (var memberAttack in clanWar.MembersResults)
                {
                    if (memberAttack.FirstDestructionPercent == 0)
                    {
                        str.AppendLine(UiHelper.MakeItStyled(memberAttack.PlayerName, UiTextStyle.Name));

                        count++;
                    }
                }
            }
            else
            {
                str.Append(UiHelper.MakeItStyled("Никто еще не провел атак, вероятно идет день подготовки.", UiTextStyle.Default));
            }

            if (clanWar.MembersResults.Any(x => x.SecondDestructionpercent != 0))
            {
                str.AppendLine(UiHelper.MakeItStyled("Не провели вторую атаку КВ: ", UiTextStyle.Subtitle));

                foreach (var memberAttack in clanWar.MembersResults)
                {
                    if (memberAttack.SecondDestructionpercent == 0)
                    {
                        str.AppendLine(UiHelper.MakeItStyled(memberAttack.PlayerName, UiTextStyle.Name));

                        count++;
                    }
                }
            }

            if (count == 0)
            {
                return "Все атаки проведены";
            }

            return str.ToString();
        }
        catch (Exception e)
        {

            return e.Message;
        }
    }

    public static string GetNonAttackersRaids(TrackedClan clan)
    {
        try
        {
            var str = new StringBuilder();

            var count = 0;

            var raid = clan.CapitalRaids.OrderByDescending(x => x.StartedOn).FirstOrDefault();

            if (clan.ClanMembers.Any(x => x.RaidMemberships.FirstOrDefault(x => x.Raid.StartedOn == raid.StartedOn) == null))
            {
                str.AppendLine(UiHelper.MakeItStyled("Не участвовали в рейдах в этом клане:", UiTextStyle.Subtitle));

                foreach (var clanMember in clan.ClanMembers)
                {
                    if (raid.RaidMembers.FirstOrDefault(x => x.Tag == clanMember.Tag) == null)
                    {
                        str.AppendLine(UiHelper.MakeItStyled(clanMember.Name, UiTextStyle.Name));

                        count++;
                    }
                }
            }

            if (raid.RaidMembers.Any(x => x.Attacks.Count != 6))
            {
                str.AppendLine();
                str.AppendLine(UiHelper.MakeItStyled("Провели не все доступные атаки:", UiTextStyle.Subtitle));

                foreach (var raidMember in raid.RaidMembers)
                {
                    if (raidMember.Attacks.Count != 6)
                    {
                        str.AppendLine(UiHelper.MakeItStyled(raidMember.Name, UiTextStyle.Name));

                        count++;
                    }
                }
            }

            if (count == 0)
            {
                return "Все игроки провели атаки";
            }

            return str.ToString();
        }
        catch (Exception e)
        {
            return e.Message;
        }

    }
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