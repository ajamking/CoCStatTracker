using CoCStatsTracker.ApiEntities;
using Domain.Entities;

namespace CoCStatsTracker.Builders;
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
        WarMember.TownHallLevel = warMemberApi.TownhallLevel;
        WarMember.MapPosition = warMemberApi.MapPosition;
        WarMember.Tag = warMemberApi.Tag;
        WarMember.Name = warMemberApi.Name;
        WarMember.BestOpponentStars = warMemberApi.BestOpponentAttack.Stars;
        WarMember.BestOpponentTime = warMemberApi.BestOpponentAttack.Duration;
        WarMember.BestOpponentPercent = warMemberApi.BestOpponentAttack.DestructionPercent;
    }

    public void SetClanWar()
    {

    }

    public void SetClanMember()
    {

    }

    public void SetWarAttacks()
    {

    }

}
