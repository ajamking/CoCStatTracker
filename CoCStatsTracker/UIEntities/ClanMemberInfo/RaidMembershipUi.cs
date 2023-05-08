using System;
using System.Collections.Generic;

namespace CoCApiDealer.UIEntities;

public class RaidMembershipUi
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

public class RaidAttackUi
{
    public string DefendersTag { get; set; }
    public string DefendersName { get; set; }
    public string DistrictName { get; set; }
    public string DistrictLevel { get; set; }
    public string DestructionPercentFrom { get; set; }
    public string DestructionPercentTo { get; set; }
}