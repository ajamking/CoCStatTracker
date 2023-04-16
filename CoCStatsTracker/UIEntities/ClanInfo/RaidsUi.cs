using System;
using System.Collections.Generic;

namespace CoCStatsTracker.UIEntities;

public class RaidsUi
{
    public DateTime StartedOn { get; set; }
    public DateTime EndedOn { get; set; }
    public string ClanTag { get; set; }
    public string ClanName { get; set; }
    public int TotalCapitalLoot { get; set; }
    public int DefeatedDistrictsCount { get; set; }
    public int DefensiveReward { get; set; }
    public int OffensiveReward { get; set; }
    public int RaidsCompleted { get; set; }
    public ICollection<District> AttackedDistricts { get; set; } // Для графика по всему клану
}

public class District
{
    public string DistrictName { get; set; }
    public ICollection<AttackOnDistrict> Attacks { get; set; }
}

public class AttackOnDistrict
{
    public string PlayerName { get; set; }
    public int DestructionPercentFrom { get; set; }
    public int DestructionPercentTo { get; set; }
}