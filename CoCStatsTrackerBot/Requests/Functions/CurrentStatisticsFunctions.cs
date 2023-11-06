using CoCStatsTracker.UIEntities;
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
        str.AppendLine(StylingHelper.MakeItStyled("Противник: ", UiTextStyle.Subtitle));
        str.AppendLine(StylingHelper.MakeItStyled(currentClanWarUi.OpponentName + " - " + currentClanWarUi.OpponentTag, UiTextStyle.Name));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Даты войны: ", UiTextStyle.Subtitle));
        str.AppendLine(StylingHelper.MakeItStyled(currentClanWarUi.StartedOn + " - ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(currentClanWarUi.EndedOn.ToString(), UiTextStyle.Default));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Состояние: ", UiTextStyle.Subtitle));
        str.AppendLine(StylingHelper.MakeItStyled(currentClanWarUi.Result, UiTextStyle.Default));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Доступно атак участникам - " + currentClanWarUi.AttackPerMember, UiTextStyle.Default));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Суммарное число атак: ", UiTextStyle.Subtitle));
        str.AppendLine(StylingHelper.MakeItStyled(currentClanWarUi.AttacksCount + " : " + currentClanWarUi.OpponentAttacksCount, UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("Суммарное количество звезд: ", UiTextStyle.Subtitle));
        str.AppendLine(StylingHelper.MakeItStyled(currentClanWarUi.TotalStarsEarned + " : " + currentClanWarUi.OpponentStarsCount, UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("Суммарный процент разрушений: ", UiTextStyle.Subtitle));
        str.AppendLine(StylingHelper.MakeItStyled(Math.Round(currentClanWarUi.DestructionPercentage, 1) + " : " + Math.Round(currentClanWarUi.OpponentDestructionPercentage, 1), UiTextStyle.Default));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Игроки, проведшие не все атаки:", UiTextStyle.Subtitle));

        foreach (var nonAttacker in currentClanWarUi.NonAttackersCw)
        {
            str.AppendLine(StylingHelper.MakeItStyled($"{nonAttacker.Key} Атак: {nonAttacker.Value}", UiTextStyle.Name));
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
        str.AppendLine(StylingHelper.MakeItStyled("Даты войны::", UiTextStyle.Subtitle));
        str.AppendLine(StylingHelper.MakeItStyled(warMapUi.PreparationStartTime + " - ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(warMapUi.StartedOn + " - ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(warMapUi.EndedOn.ToString(), UiTextStyle.Default));
        str.AppendLine();
        str.AppendLine($@"``` ");

        for (int i = 0; i < warMembers.Count; i++)
        {
            var mate = warMembers[i];

            var opponent = enemyWarMembers[i];

            var properMateName = StylingHelper.GetProperString(mate.Name, maxNameLength);

            var properOpponentName = StylingHelper.GetProperString(opponent.Name, maxNameLength);

            var position = StylingHelper.GetCenteredString((i + 1).ToString(), 2);

            if (properMateName.Length >= maxNameLength)
            {
                properMateName = properMateName.Substring(0, maxNameLength);
            }

            properMateName = StylingHelper.GetCenteredString(properMateName, maxNameLength);

            if (properOpponentName.Length >= maxNameLength)
            {
                properOpponentName = properOpponentName.Substring(0, maxNameLength);
            }

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

            properOpponentName = StylingHelper.GetCenteredString(properOpponentName, maxNameLength);

            var mapStr = $@"{properMateName} {membersThLevel} |{position}| {opponent.TownHallLevel} {properOpponentName}";

            str.AppendLine($@"{StylingHelper.Ecranize(mapStr)}");
        }

        str.AppendLine($@"```");

        return str.ToString();
    }

    public static string GetCurrentRaidShortInfo(RaidUi raidsUi)
    {
        var str = new StringBuilder();

        var totalAttacksCount = 0;

        var offensiveReward = raidsUi.OffensiveReward;

        var totalReward = offensiveReward + raidsUi.DefensiveReward;

        foreach (var defeatedClan in raidsUi.DefeatedClans)
        {
            totalAttacksCount += defeatedClan.TotalAttacksCount;
        }

        str.AppendLine(StylingHelper.MakeItStyled("Общая информация о последнем рейде клана", UiTextStyle.Header));
        str.AppendLine(StylingHelper.MakeItStyled(raidsUi.ClanName + " - " + raidsUi.ClanTag, UiTextStyle.Name));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Даты дней рейдов:", UiTextStyle.Subtitle));
        str.AppendLine(StylingHelper.MakeItStyled(raidsUi.StartedOn + " - ", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled(raidsUi.EndedOn.ToString(), UiTextStyle.Default));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Проведено атак: " + totalAttacksCount, UiTextStyle.Subtitle));
        str.AppendLine(StylingHelper.MakeItStyled("Повержено кланов: " + raidsUi.RaidsCompleted, UiTextStyle.Subtitle));
        str.AppendLine(StylingHelper.MakeItStyled("Разрушено районов: " + raidsUi.DefeatedDistrictsCount, UiTextStyle.Subtitle));
        str.AppendLine(StylingHelper.MakeItStyled("Награблено золота: " + raidsUi.TotalCapitalLoot, UiTextStyle.Subtitle));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Медали рейдов: ", UiTextStyle.Subtitle));
        str.AppendLine(StylingHelper.MakeItStyled("За атаку: " + offensiveReward, UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("За защиту: " + raidsUi.DefensiveReward, UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("Суммарно: " + totalReward, UiTextStyle.Default));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Общие показатели обороны: ", UiTextStyle.Subtitle));

        var averageDefenses = 0.0;

        foreach (var defense in raidsUi.Defenses)
        {
            str.AppendLine(StylingHelper.MakeItStyled("[" + defense.AttackersTag + "] " + "[" + defense.AttackersName + "] " +
                "[" + defense.TotalAttacksCount + "]", UiTextStyle.Name));

            averageDefenses += defense.TotalAttacksCount;
        }

        str.AppendLine(StylingHelper.MakeItStyled("Выдержано атак в среднем: " + Math.Round(averageDefenses / raidsUi.Defenses.Count, 2), UiTextStyle.Subtitle));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Общие показатели нападения: ", UiTextStyle.Subtitle));

        var averageAttacks = 0.0;

        foreach (var defeatedClan in raidsUi.DefeatedClans)
        {
            str.AppendLine(StylingHelper.MakeItStyled("[" + defeatedClan.ClanTag + "] " + "[" + defeatedClan.ClanName + "] " +
                "[" + defeatedClan.TotalAttacksCount + "]", UiTextStyle.Name));

            averageAttacks += defeatedClan.TotalAttacksCount;
        }

        str.AppendLine(StylingHelper.MakeItStyled("Потрачено атак в среднем: " + Math.Round(averageAttacks / raidsUi.DefeatedClans.Count, 2), UiTextStyle.Subtitle));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Игроки, проведшие не все атаки:", UiTextStyle.Subtitle));

        foreach (var nonAttacker in raidsUi.NonAttackersRaids)
        {
            str.AppendLine(StylingHelper.MakeItStyled($"{nonAttacker.Key} Атак: {nonAttacker.Value}", UiTextStyle.Name));
        }

        str.AppendLine();

        return str.ToString();
    }

    public static string GetDistrictStatistics(RaidUi raidUi, ADistrictType districtType)
    {
        var maxNameLength = 18;
        var max2ColumnLength = 3;
        var max3ColumnLength = 3;

        var districtMapper = new Dictionary<ADistrictType, string>()
        {
            {ADistrictType.Capital_Peak, "Capital Peak" },
            {ADistrictType.Barbarian_Camp, "Barbarian Camp" },
            {ADistrictType.Wizard_Valley, "Wizard Valley" },
            {ADistrictType.Balloon_Lagoon, "Balloon Lagoon" },
            {ADistrictType.Builders_Workshop, "Builder's Workshop" },
            {ADistrictType.Dragon_Cliffs, "Dragon Cliffs" },
            {ADistrictType.Golem_Quarry, "Golem Quarry" },
            {ADistrictType.Skeleton_Park, "Skeleton Park" },
            {ADistrictType.Goblin_Mines, "Goblin Mines" },

        };

        var chosenDistrictName = districtMapper.First(x => x.Key == districtType).Value;

        var avgPercent = 0.0;
        var counter = 0;

        foreach (var clan in raidUi.DefeatedClans)
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

        if (counter is not 0)
        {
            avgPercent = Math.Round(avgPercent / counter, 2);
        }

        var str = new StringBuilder();

        str.AppendLine(StylingHelper.MakeItStyled("Показатели игроков клана", UiTextStyle.Header));
        str.AppendLine(StylingHelper.MakeItStyled(raidUi.ClanName + " - " + raidUi.ClanTag, UiTextStyle.Name));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("В атаках на район", UiTextStyle.Subtitle));
        str.AppendLine(StylingHelper.MakeItStyled(chosenDistrictName, UiTextStyle.Name));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Разрушений за атаку в среднем: " + avgPercent + "%", UiTextStyle.Default));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Показатели атак", UiTextStyle.Subtitle));
        str.AppendLine();

        str.AppendLine($"``` " +
                           $"|{StylingHelper.GetCenteredString("Игрок", maxNameLength)}" +
                           $"|{StylingHelper.GetCenteredString("%От", max2ColumnLength)}" +
                           $"|{StylingHelper.GetCenteredString("%До", max3ColumnLength)}|");

        str.AppendLine($" " +
            $"|{new string('-', maxNameLength)}" +
            $"|{new string('-', max2ColumnLength)}" +
            $"|{new string('-', max3ColumnLength)}|");

        foreach (var clan in raidUi.DefeatedClans)
        {
            foreach (var district in clan.AttackedDistricts.Where(x => x.DistrictName == chosenDistrictName))
            {
                foreach (var attack in district.Attacks)
                {
                    var properName = StylingHelper.GetProperString(attack.PlayerName, maxNameLength);

                    str.Append($" |{StylingHelper.GetCenteredString(properName, maxNameLength)}|");

                    str.Append($"{StylingHelper.GetCenteredString(attack.DestructionPercentFrom.ToString(), max2ColumnLength)}|");

                    str.AppendLine($"{StylingHelper.GetCenteredString(attack.DestructionPercentTo.ToString(), max3ColumnLength)}|");
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
}