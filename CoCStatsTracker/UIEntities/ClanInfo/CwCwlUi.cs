using System;
using System.Collections.Generic;

namespace CoCStatsTracker.UIEntities;

public class CwCwlUi
{
    public DateTime StartedOn { get; set; }
    public DateTime EndedOn { get; set; }
    public int WarMembersCount { get; set; }
    public string ClanTag { get; set; }
    public string ClanName { get; set; }
    public int TotalStarsEarned { get; set; }
    public int AverageDestructionPercent { get; set; }
    public string EnemyName { get; set; }
    public string EnemyTag { get; set; }
    public int EnemyTotalStarsEarned { get; set; }
    public int EnemyAverageDestructionPercent { get; set; }
    public string Result { get; set; }

    public ICollection<ClanWarAttackUi> WarAttacks { get; set; } // Для графика по всему клану
}

public class ClanWarAttackUi
{
    public string PlayerName { get; set; }
    public int FirstAttackDestructionPercent { get; set; }
    public int SecondAttackDestructionPercent { get; set; }
}