using System.Collections.Generic;

namespace CoCStatsTracker.UIEntities;

public class WarMapUi : UiEntity
{
    public string ClanTag { get; set; }
    public string ClanName { get; set; }
    public string OpponentClanName { get; set; }
    public string OpponentClanTag { get; set; }
    public string PreparationStartTime { get; set; }
    public string StartedOn { get; set; }
    public string EndedOn { get; set; }

    public List<WarMemberOnMapUi> WarMembers { get; set; }
    public List<WarMemberOnMapUi> EnemyWarMembers { get; set; }
}

public class WarMemberOnMapUi : UiEntity
{
    public string Name { get; set; }
    public string Tag { get; set; }
    public int MapPosition { get; set; }
    public int TownHallLevel { get; set; }
}