namespace Domain.Entities;

public class DestroyedFriendlyDistrict : Entity
{
    public string Name { get; set; }
    public int Level { get; set; }
    public int AttacksSpent { get; set; }
    public int TotalDestructionPersent { get; set; }

    public int? RaidDefenseId { get; set; }
    public virtual RaidDefense RaidDefense { get; set; }
}