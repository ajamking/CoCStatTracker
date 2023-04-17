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

    public void SetBaseProperties(ClanWarApi clanWarApi)
    {
        ClanWar.IsCWL = false;
        ClanWar.Result = "InWar";

        ClanWar.State = clanWarApi.State;
        ClanWar.TeamSize = clanWarApi.TeamSize;
        ClanWar.AttackPerMember = clanWarApi.AttacksPerMember;

        ClanWar.PreparationStartTime = DateTimeParser.Parse(clanWarApi.PreparationStartTime);
        ClanWar.StartTime = DateTimeParser.Parse(clanWarApi.StartTime);
        ClanWar.EndTime = DateTimeParser.Parse(clanWarApi.EndTime);

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

    public void SetTrackedClan()
    {

    }

    public void SetWarMembers(ICollection<WarMember> warMembers)
    {
        foreach (var member in warMembers)
        {
            ClanWar.WarMembers.Add(member);
        }
    }

    public void SetWarAttacks()
    {

    }

    public void SetEnemyWarMembers()
    {

    }
}
