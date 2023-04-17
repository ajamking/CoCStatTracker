using System;
using System.Collections.Generic;

namespace CoCApiDealer.UIEntities;

public class ShortPrizeDrawUi
{
    public string ClanTag { get; set; }
    public string ClanName { get; set; }
    public DateTime StartOn { get; set; }
    public DateTime EndedOn { get; set; }
    public string Description { get; set; }

    public Dictionary<string, int> Participants { get; set; }
}
