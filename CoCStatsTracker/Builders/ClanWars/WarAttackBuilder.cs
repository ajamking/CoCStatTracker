using CoCStatsTracker.ApiEntities;
using Domain.Entities;

namespace CoCStatsTracker;

public class WarAttackBuilder
{
    public WarAttack WarAttack { get; } = new WarAttack();

    public WarAttackBuilder(WarAttack warAttack = null)
    {
        if (warAttack != null)
        {
            WarAttack = warAttack;
        }
    }

    public void SetBaseProperties(WarAttackApi warAttack)
    {
        WarAttack.AttackOrder = warAttack.Order;
        WarAttack.Stars = warAttack.Stars;
        WarAttack.DestructionPercent = warAttack.DestructionPercent;
        WarAttack.Duration = warAttack.Duration;
    }

    public void SetEnemyWarMember(WarMemberApi enemyWarMember)
    {
        WarAttack.EnemyWarMember.Tag = enemyWarMember.Tag;
        WarAttack.EnemyWarMember.Name = enemyWarMember.Name;
        WarAttack.EnemyWarMember.THLevel = enemyWarMember.TownhallLevel;
        WarAttack.EnemyWarMember.MapPosition = enemyWarMember.MapPosition;
    }
}
