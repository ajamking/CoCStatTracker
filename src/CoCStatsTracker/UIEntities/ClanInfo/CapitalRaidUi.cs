using CoCStatsTracker.UIEntities.ClanInfo;
using System;
using System.Collections.Generic;

namespace CoCStatsTracker.UIEntities;

public class CapitalRaidUi : UiEntity
{
    public string State { get; set; }
    public DateTime StartedOn { get; set; }
    public DateTime EndedOn { get; set; }
    public string ClanTag { get; set; }
    public string ClanName { get; set; }
    public int TotalAttacksCount { get; set; }
    public int TotalCapitalLoot { get; set; }
    public int DefeatedDistrictsCount { get; set; }
    public int DefensiveReward { get; set; }
    public int OffensiveReward { get; set; }

    public ICollection<RaidDefenseUi> Defenses { get; set; }

    public ICollection<AttackedClanUi> DefeatedClans { get; set; }

    public ICollection<NonAttacker> NonAttackersRaids { get; set; }
}

public class RaidDefenseUi : UiEntity
{
    public string EnemyClanTag { get; set; }
    public string EnemyClanName { get; set; }
    public int TotalAttacksCount { get; set; }
    public int TotalEnemyLoot { get; set; }
    public int DestroyedFriendlyDistrictsCount { get; set; }

    public ICollection<DistrictUi> DestroyedFriendlyDistricts { get; set; }
}

public class AttackedClanUi : UiEntity
{
    public string Name { get; set; }
    public string Tag { get; set; }
    public int TotalAttacksCount { get; set; }
    public int TotalLoot { get; set; }

    public ICollection<DistrictUi> DefeatedEmemyDistricts { get; set; }
}

public class DistrictUi : UiEntity
{
    public string Name { get; set; }
    public int Level { get; set; }
    public int Loot { get; set; }
    public int AttacksCount { get; set; } //Только для оборонных
    public int TotalDestructionPercent { get; set; } //Только для оборонных

    public ICollection<AttackOnDistrictUi> Attacks { get; set; }
}

public class AttackOnDistrictUi : UiEntity
{
    public string AttackerTag { get; set; }
    public string AttackerName { get; set; }
    public int DestructionPercentFrom { get; set; }
    public int DestructionPercentTo { get; set; }
}