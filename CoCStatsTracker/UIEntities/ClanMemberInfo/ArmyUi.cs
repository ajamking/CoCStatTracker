using System.Collections.Generic;

namespace CoCStatsTracker.UIEntities;

public class ArmyUi
{
    public Dictionary<string, int> SuperUnits { get; set; } = new Dictionary<string, int>() { { null, 0 } };
    public Dictionary<string, int> SiegeMachines { get; set; } = new Dictionary<string, int>() { { null, 0 } };
    public Dictionary<string, int> Heroes { get; set; } = new Dictionary<string, int>() { { null, 0 } };
    public Dictionary<string, int> Pets { get; set; } = new Dictionary<string, int>() { { null, 0 } };
    public Dictionary<string, int> Units { get; set; } = new Dictionary<string, int>() { { null, 0 } };
}
