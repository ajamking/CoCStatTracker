namespace CoCStatsTracker.UIEntities;

public class MedianRaidPerfomanse : UiEntity
{
    public int RaidMembershipsCount { get; set; }
    public string ClanName { get; set; }
    public string ClanTag { get; set; }
    public string Name { get; set; }
    public string Tag { get; set; }
    public double MedianDestructionPersent { get; set; }
    public double MedianLoot { get; set; }
}