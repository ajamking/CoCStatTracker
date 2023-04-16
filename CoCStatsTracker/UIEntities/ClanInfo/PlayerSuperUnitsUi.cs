using System.Collections.Generic;

namespace CoCStatsTracker.UIEntities;
public class PlayerSuperUnitsUi
{
    public string PlayerName { get; set; }
    public ICollection<SuperUnitUi> ActivatedSuperUnits { get; set; }
}

public class SuperUnitUi
{
    public string Name { get; set; }
    public int Level { get; set; }
}
