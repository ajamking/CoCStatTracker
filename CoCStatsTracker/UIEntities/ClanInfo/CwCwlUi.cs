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
    public double DestructionPercentage { get; set; }
    public string OpponentName { get; set; }
    public string OpponentTag { get; set; }
    public int OpponentStarsCount { get; set; }
    public double OpponentDestructionPercentage { get; set; }
    public string Result { get; set; }

    public ICollection<ClanWarAttackUi> WarAttacks { get; set; } // Для графика по всему клану
}

public class ClanWarAttackUi
{
    public string PlayerName { get; set; }
    public int FirstStarsCount { get; set; }
    public int FirstDestructionPercent { get; set; }
    public int SecondStarsCount { get; set; }
    public int SecondDestructionpercent { get; set; }
}