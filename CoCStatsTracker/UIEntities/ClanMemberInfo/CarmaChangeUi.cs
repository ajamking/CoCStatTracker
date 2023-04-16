using System;
using System.Collections.Generic;

namespace CoCApiDealer.UIEntities;
public class CarmaChangeUi
{
    public string PlayersName { get; set; }
    public string PlayersTag { get; set; }
    public int TotalCarma { get; set; }
    public ICollection<ActivityUi> Activities { get; set; }
}

public class ActivityUi
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int EarnedPoints { get; set; }
    public DateTime UpdatedOn { get; set; }
}