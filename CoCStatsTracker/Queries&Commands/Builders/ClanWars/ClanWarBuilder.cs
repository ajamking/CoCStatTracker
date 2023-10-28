using CoCStatsTracker.ApiEntities;
using CoCStatsTracker;
using Domain.Entities;
using System.Collections.Generic;
using CoCStatsTracker.Items.Helpers;

namespace CoCStatsTracker.Builders;

public class ClanWarBuilder
{
    public ClanWar ClanWar { get; }

    public ClanWarBuilder(ClanWar clanWar = null)
    {
        ClanWar = clanWar ?? new ClanWar();
    }

    public void SetBaseProperties(ClanWarApi clanWarApi, bool isCwl = false, string state = "War still goes on")
    {
        ClanWar.IsCWL = isCwl;
        ClanWar.Result = state;

        ClanWar.State = clanWarApi.State;
        ClanWar.TeamSize = clanWarApi.TeamSize;
        ClanWar.AttackPerMember = clanWarApi.AttacksPerMember;

        ClanWar.PreparationStartTime = DateTimeParser.Parse(clanWarApi.PreparationStartTime).ToLocalTime();
        ClanWar.StartedOn = DateTimeParser.Parse(clanWarApi.StartTime).ToLocalTime();
        ClanWar.EndedOn = DateTimeParser.Parse(clanWarApi.EndTime).ToLocalTime();

        ClanWar.AttacksCount = clanWarApi.ClanResults.AttacksCount;
        ClanWar.StarsCount = clanWarApi.ClanResults.StarsCount;
        ClanWar.DestructionPercentage = clanWarApi.ClanResults.DestructionPercentage;

        ClanWar.OpponentClanTag = clanWarApi.OpponentResults.Tag;
        ClanWar.OpponentClanName = clanWarApi.OpponentResults.Name;
        ClanWar.OpponentClanLevel = clanWarApi.OpponentResults.ClanLevel;
        ClanWar.OpponentAttacksCount = clanWarApi.OpponentResults.AttacksCount;
        ClanWar.OpponentStarsCount = clanWarApi.OpponentResults.StarsCount;
        ClanWar.OpponentDestructionPercentage = clanWarApi.OpponentResults.DestructionPercentage;
    }

    public void SetTrackedClan(TrackedClan clan)
    {
        ClanWar.TrackedClan = clan;
    }

    public void SetWarMembers(ICollection<WarMember> warMembers)
    {
        ClanWar.WarMembers = warMembers;
    }

    public void SetEnemyWarMembers(ICollection<EnemyWarMember> warMembers)
    {
        ClanWar.EnemyWarMembers = warMembers;
    }
}
