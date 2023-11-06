using System.Collections.Generic;

namespace CoCStatsTracker.UIEntities;

public class ArmyUi : UiEntity
{
    public string PlayerName { get; set; }
    public string PlayerTag { get; set; }
    public int TownHallLevel { get; set; }
    public string ClanName { get; set; }
    public string ClanTag { get; set; }

    public List<TroopUi> SuperUnits { get; set; } = new List<TroopUi>();
    public List<TroopUi> SiegeMachines { get; set; } = new List<TroopUi>();
    public List<TroopUi> Heroes { get; set; } = new List<TroopUi>();
    public List<TroopUi> Pets { get; set; } = new List<TroopUi>();
    public List<TroopUi> Units { get; set; } = new List<TroopUi>();
}

public class TroopUi : UiEntity
{
    public string Name { get; set; }
    public int Lvl { get; set; }
    public bool? SuperTroopIsActivated { get; set; }
}