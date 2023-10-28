using CoCStatsTracker.ApiEntities;
using Domain.Entities;

namespace CoCStatsTracker.Builders;

public class EnemyWarMemberBuilder
{
    public EnemyWarMember EnemyWarMember { get; }

    public EnemyWarMemberBuilder(EnemyWarMember enemyWarMember = null)
    {
        EnemyWarMember = enemyWarMember ?? new EnemyWarMember();
    }

    public void SetBaseProperties(WarMemberApi enemyWarMember)
    {
        EnemyWarMember.Tag = enemyWarMember.Tag;
        EnemyWarMember.Name = enemyWarMember.Name;
        EnemyWarMember.TownHallLevel = enemyWarMember.TownhallLevel;
        EnemyWarMember.MapPosition = enemyWarMember.MapPosition;
    }

    public void SetClanWar(ClanWar clanWar)
    {
        EnemyWarMember.ClanWar = clanWar;
    }

}
