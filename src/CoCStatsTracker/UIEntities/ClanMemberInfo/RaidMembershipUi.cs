using System;
using System.Collections.Generic;

namespace CoCStatsTracker.UIEntities;

public class RaidMembershipUi : UiEntity
{
    public string Tag { get; set; }
    public string Name { get; set; }
    public string ClanTag { get; set; }
    public string ClanName { get; set; }
    public DateTime StartedOn { get; set; }
    public DateTime EndedOn { get; set; }
    public int TotalLoot { get; set; }
    public ICollection<RaidAttackUi> Attacks { get; set; }
}

public class RaidAttackUi : UiEntity
{
    public string DefendersTag { get; set; }
    public string DefendersName { get; set; }
    public string DistrictName { get; set; }
    public int DistrictLevel { get; set; }
    public int DestructionPercentFrom { get; set; }
    public int DestructionPercentTo { get; set; }
}