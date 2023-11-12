namespace CoCStatsTracker.UIEntities;

public class AverageRaidsPerfomanceUi : UiEntity
{
    public int RaidMembershipsCount { get; set; }
    public string ClanName { get; set; }
    public string ClanTag { get; set; }
    public string Name { get; set; }
    public string Tag { get; set; }
    public double AverageDestructionPercent { get; set; }
    public double AverageCapitalLoot { get; set; }
}