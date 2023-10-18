using System;
using System.Collections.Generic;

namespace CoCStatsTracker.UIEntities;

public class RaidsUi : UiEntity
{
    public string State { get; set; }
    public DateTime StartedOn { get; set; }
    public DateTime EndedOn { get; set; }
    public string ClanTag { get; set; }
    public string ClanName { get; set; }
    public int TotalCapitalLoot { get; set; }
    public int DefeatedDistrictsCount { get; set; }
    public int DefensiveReward { get; set; }
    public int OffensiveReward { get; set; }
    public int RaidsCompleted { get; set; }
    public ICollection<RaidDefenseUi> Defenses { get; set; }

    public ICollection<DefeatedClanUi> DefeatedClans { get; set; }
}

public class RaidDefenseUi : UiEntity
{
    public string AttackersTag { get; set; }
    public string AttackersName { get; set; }
    public int TotalAttacksCount { get; set; }
}

public class DefeatedClanUi : UiEntity
{
    public ICollection<DistrictUi> AttackedDistricts { get; set; }
    public string ClanName { get; set; }
    public string ClanTag { get; set; }
    public int TotalAttacksCount { get; set; }
}

public class DistrictUi : UiEntity
{
    public string DistrictName { get; set; }
    public int DistrictLevel { get; set; }
    public ICollection<AttackOnDistrictUi> Attacks { get; set; }
}

public class AttackOnDistrictUi : UiEntity
{
    public string PlayerName { get; set; }
    public string PlayerTag { get; set; }
    public int DestructionPercentFrom { get; set; }
    public int DestructionPercentTo { get; set; }
}