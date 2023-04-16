using CoCStatsTracker.UIEntities;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;


namespace CoCStatsTracker;

public static class Mapper
{
    public static ClanMemberUi MapToUi(ClanMember clanMember)
    {
        return new ClanMemberUi
        {
            Tag = clanMember.Tag,
            Name = clanMember.Name,
            RoleInClan = clanMember.Role,
            TownHallLevel = clanMember.TownHallLevel,
            DonationsSent = clanMember.DonationsSent,
            WarStars = clanMember.WarStars,
            CapitalContributions = clanMember.TotalCapitalContributions
        };
    }

    public static ClanUi MapToUi(TrackedClan clan)
    {
        var warLogType = "";

        if (clan.IsWarLogPublic == false)
            warLogType = "Закрытая";
        else
            warLogType = "Общедоступная";

        return new ClanUi
        {
            Tag = clan.Tag,
            Name = clan.Name,
            Type = clan.Type,
            Description = clan.Description,
            ClanLevel = clan.ClanLevel,
            ClanMembersCount = clan.ClanMembers.Count(),
            ClanPoints = clan.ClanPoints,
            ClanVersusPoints = clan.ClanVersusPoints,
            ClanCapitalPoints = clan.ClanCapitalPoints,
            CapitalLeague = clan.CapitalLeague,
            IsWarLogPublic = warLogType,
            WarLeague = clan.WarLeague,
            WarWinStreak = clan.WarWinStreak,
            WarWins = clan.WarWins,
            WarTies = clan.WarTies,
            WarLoses = clan.WarLoses,
            CapitalHallLevel = clan.CapitalHallLevel
        };
    }

    public static CwCwlUi MapToUi(ClanWar clanWar)
    {
        return null;
    }











    public static PlayerSuperUnitsUi MapUnitsToUi(ClanMember member)
    {
        var activatedUnits = new List<SuperUnitUi>();

        foreach (var unit in member.Units)
        {
            if (unit.SuperTroopIsActivated == true)
            {
                activatedUnits.Add(new SuperUnitUi { Name = unit.Name, Level = unit.Level });
            }
        }

        return new PlayerSuperUnitsUi
        {
            PlayerName = member.Name,
            ActivatedSuperUnits = activatedUnits
        };
    }





}
