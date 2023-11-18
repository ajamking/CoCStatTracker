using System.Collections.Generic;

namespace Domain.Entities;

public class AttackedClanOnRaid : Entity
{
    public string Tag { get; set; }
    public string Name { get; set; }
    public int Level { get; set; }

    public int? CapitalRaidId { get; set; }
    public virtual CapitalRaid CapitalRaid { get; set; }

    public virtual ICollection<RaidAttack> RaidAttacks { get; set; }
    public virtual ICollection<DefeatedEmemyDistrict> DefeatedEmemyDistricts { get; set; }

    public AttackedClanOnRaid()
    {
        DefeatedEmemyDistricts = new HashSet<DefeatedEmemyDistrict>();
        RaidAttacks = new HashSet<RaidAttack>();
    }
}