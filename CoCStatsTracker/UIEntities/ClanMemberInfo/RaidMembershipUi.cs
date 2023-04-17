using System;
using System.Collections.Generic;

namespace CoCApiDealer.UIEntities;

public class RaidMembershipUi
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

public class RaidAttackUi
{
    public string DefendersTag { get; set; }
    public string DefendersName { get; set; }
    public string DistrictName { get; set; }
    public int DistrictLevel { get; set; }
    public int DestructionPercentFrom { get; set; }
    public int DestructionPercentTo { get; set; }
}