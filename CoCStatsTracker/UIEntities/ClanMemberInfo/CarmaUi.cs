using System.Collections.Generic;

namespace CoCApiDealer.UIEntities;

public class CarmaUi
{
    public string PlayersName { get; set; }
    public string PlayersTag { get; set; }
    public string CurrentCarma { get; set; }
    public ICollection<ActivityUi> Activities { get; set; }
}

public class ActivityUi
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string EarnedPoints { get; set; }
    public string UpdatedOn { get; set; }
}