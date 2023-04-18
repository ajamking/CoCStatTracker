using CoCStatsTracker.ApiEntities;
using CoCStatsTracker.Helpers;
using Domain.Entities;
using System.Collections.Generic;

namespace CoCStatsTracker.Builders;

public class ClanWarBuilder
{
    public ClanWar ClanWar { get; } = new ClanWar();

    public ClanWarBuilder(ClanWar clanWar = null)
    {
        if (clanWar != null)
        {
            ClanWar = clanWar;
        }
    }

    public void SetBaseProperties(ClanWarApi clanWarApi, bool isCwl = false, string state = "War still goes on")
    {
        ClanWar.IsCWL = isCwl;
        ClanWar.Result = state;

        ClanWar.State = clanWarApi.State;
        ClanWar.TeamSize = clanWarApi.TeamSize;
        ClanWar.AttackPerMember = clanWarApi.AttacksPerMember;

        ClanWar.PreparationStartTime = DateTimeParser.Parse(clanWarApi.PreparationStartTime).ToLocalTime();
        ClanWar.StartTime = DateTimeParser.Parse(clanWarApi.StartTime).ToLocalTime();
        ClanWar.EndTime = DateTimeParser.Parse(clanWarApi.EndTime).ToLocalTime();

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

    public void SetWarMembers(ICollection<WarMember> warMembers)
    {
        ClanWar.WarMembers = warMembers;
    }
}
