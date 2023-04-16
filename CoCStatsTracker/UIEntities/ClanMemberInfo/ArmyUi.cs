using System.Collections.Generic;

namespace CoCStatsTracker.UIEntities;

public class ArmyUi
{
    public Dictionary<string, int>[] SuperUnits { get; set; } = null;
    public Dictionary<string, int>[] SiegeMachines { get; set; } = null;
    public Dictionary<string, int>[] Heroes { get; set; } = null;
}
