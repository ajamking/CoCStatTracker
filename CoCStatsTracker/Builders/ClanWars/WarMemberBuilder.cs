using CoCStatsTracker.ApiEntities;
using Domain.Entities;
using System.Collections.Generic;

namespace CoCStatsTracker;
public class WarMemberBuilder
{
    public WarMember WarMember { get; } = new WarMember();

    public WarMemberBuilder(WarMember warMember = null)
    {
        if (warMember != null)
        {
            WarMember = warMember;
        }
    }

    public void SetBaseProperties(WarMemberApi warMemberApi)
    {
        WarMember.Tag = warMemberApi.Tag;
        WarMember.Name = warMemberApi.Name;
        WarMember.TownHallLevel = warMemberApi.TownhallLevel;
        WarMember.MapPosition = warMemberApi.MapPosition;
        if (warMemberApi.BestOpponentAttack != null)
        {
            WarMember.BestOpponentStars = warMemberApi.BestOpponentAttack.Stars;
            WarMember.BestOpponentTime = warMemberApi.BestOpponentAttack.Duration;
            WarMember.BestOpponentPercent = warMemberApi.BestOpponentAttack.DestructionPercent;
        }
    }

    public void SetWarAttacks(ICollection<WarAttack> attacks)
    {
        WarMember.WarAttacks = attacks;
    }

    public void SetClanWar(ClanWar clanWar)
    {
        WarMember.ClanWar = clanWar;
    }

    public void SetClanMember(ClanMember clanMember)
    {
        WarMember.ClanMember = clanMember;
    }
}
