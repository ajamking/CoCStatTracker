using Domain.Entities;

namespace Domain.Entities;

public class DefeatedEmemyDistrict : Entity
{
    public string Name { get; set; }

    public int Level { get; set; }

    public int TotalDistrictLoot { get; set; }

    public int? AttackedClanOnRaidId { get; set; }
    public virtual AttackedClanOnRaid AttackedClan { get; set; }
}