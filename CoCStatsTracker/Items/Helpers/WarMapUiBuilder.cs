using CoCStatsTracker.UIEntities;
using Domain.Entities;
using System.Collections.Generic;

namespace CoCStatsTracker.Items.Helpers;

public static class WarMapUiBuilder
{
    public static WarMapUi Build(ClanWar clanWar)
    {
        var warMembersUi = new List<WarMemberOnMapUi>();

        foreach (var warMember in clanWar.WarMembers)
        {
            warMembersUi.Add(new WarMemberOnMapUi()
            {
                MapPosition = warMember.MapPosition,
                Name = warMember.Name,
                Tag = warMember.Tag,
                TownHallLevel = warMember.TownHallLevel,

            });
        }

        var enemyWarMembersUi = new List<WarMemberOnMapUi>();

        foreach (var warMember in clanWar.EnemyWarMembers)
        {
            enemyWarMembersUi.Add(new WarMemberOnMapUi()
            {
                MapPosition = warMember.MapPosition,
                Name = warMember.Name,
                Tag = warMember.Tag,
                TownHallLevel = warMember.TownHallLevel,
            });
        }

        return new WarMapUi()
        {
            ClanName = clanWar.TrackedClan.Name,
            ClanTag = clanWar.TrackedClan.Tag,
            OpponentClanName = clanWar.OpponentClanName,
            OpponentClanTag = clanWar.OpponentClanTag,
            PreparationStartTime = clanWar.PreparationStartTime.ToString(),
            StartedOn = clanWar.StartedOn.ToString(),
            EndedOn = clanWar.EndedOn.ToString(),
            WarMembers = warMembersUi,
            EnemyWarMembers = enemyWarMembersUi,
        };
    }
}