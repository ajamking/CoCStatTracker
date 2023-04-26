﻿using CoCStatsTracker.ApiEntities;
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

    public void SetWarMember(WarMember warMember)
    {
        WarAttack.WarMember = warMember;
    }

    public void SetEnemyWarMember(EnemyWarMember enemyWarMember)
    {
        WarAttack.EnemyWarMember = enemyWarMember;
    }
}
