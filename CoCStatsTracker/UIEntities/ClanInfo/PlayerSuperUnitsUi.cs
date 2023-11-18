using System.Collections.Generic;

namespace CoCStatsTracker.UIEntities;
public class PlayerSuperUnitsUi : UiEntity
{
    public string PlayerName { get; set; }
    public ICollection<SuperUnitUi> ActivatedSuperUnits { get; set; }
}

public class SuperUnitUi : UiEntity
{
    public string Name { get; set; }
    public int Level { get; set; }
}