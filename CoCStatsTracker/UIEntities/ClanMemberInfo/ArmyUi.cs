using System.Collections.Generic;

namespace CoCStatsTracker.UIEntities;

public class ArmyUi
{

    public List<TroopUi> SuperUnits { get; set; } = new List<TroopUi>();
    public List<TroopUi> SiegeMachines { get; set; } = new List<TroopUi>();
    public List<TroopUi> Heroes { get; set; } = new List<TroopUi>();
    public List<TroopUi> Pets { get; set; } = new List<TroopUi>();
    public List<TroopUi> Units { get; set; } = new List<TroopUi>();
}

public class TroopUi
{
    public string Name { get; set; }
    public string Lvl { get; set; }
    public string SuperTroopIsActivated { get; set; }
}
