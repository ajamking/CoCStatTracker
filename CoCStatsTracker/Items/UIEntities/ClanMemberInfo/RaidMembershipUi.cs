using System.Collections.Generic;

namespace CoCStatsTracker.UIEntities;

public class RaidMembershipUi : UiEntity
{
    public string Tag { get; set; }
    public string Name { get; set; }
    public string ClanTag { get; set; }
    public string ClanName { get; set; }
    public string StartedOn { get; set; }
    public string EndedOn { get; set; }
    public string TotalLoot { get; set; }
    public ICollection<RaidAttackUi> Attacks { get; set; }
}

public class RaidAttackUi : UiEntity
{
    public string DefendersTag { get; set; }
    public string DefendersName { get; set; }
    public string DistrictName { get; set; }
    public string DistrictLevel { get; set; }
    public string DestructionPercentFrom { get; set; }
    public string DestructionPercentTo { get; set; }
}