using System.Collections.Generic;

namespace Domain.Entities;

public class OpponentDistrict : Entity
{
    public string Name { get; set; }
    public int Level { get; set; }

    public int? DefeatedCLanId { get; set; }
    public virtual DefeatedClan DefeatedClan { get; set; }

    public virtual ICollection<RaidAttack> Attacks { get; set; }

    public OpponentDistrict()
    {
        Attacks = new HashSet<RaidAttack>();
    }
}
