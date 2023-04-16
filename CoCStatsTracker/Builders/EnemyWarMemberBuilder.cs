using CoCStatsTracker.ApiEntities;
using Domain.Entities;

namespace CoCStatsTracker.Builders;
public class EnemyWarMemberBuilder
{
    public EnemyWarMember EnemyWarMember { get; } = new EnemyWarMember();

    public EnemyWarMemberBuilder(EnemyWarMember enemyWarMember = null)
    {
        if (enemyWarMember != null)
        {
            EnemyWarMember = enemyWarMember;

        }
    }

    public void SetBaseProperties(WarMemberApi enemyWarMember)
    {
        EnemyWarMember.MapPosition = enemyWarMember.MapPosition;
        EnemyWarMember.Tag = enemyWarMember.Tag;
        EnemyWarMember.Name = enemyWarMember.Name;
        EnemyWarMember.THLevel = enemyWarMember.TownhallLevel;
        EnemyWarMember.MapPosition = enemyWarMember.MapPosition;
    }

    public void SetClanWar()
    {

    }
}
