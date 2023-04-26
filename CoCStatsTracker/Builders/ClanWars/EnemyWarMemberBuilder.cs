using CoCStatsTracker.ApiEntities;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        EnemyWarMember.Tag = enemyWarMember.Tag;
        EnemyWarMember.Name = enemyWarMember.Name;
        EnemyWarMember.THLevel = enemyWarMember.TownhallLevel;
        EnemyWarMember.MapPosition = enemyWarMember.MapPosition;
    }

    public void SetClanWar(ClanWar clanWar)
    {
        EnemyWarMember.ClanWar = clanWar;
    }

}
