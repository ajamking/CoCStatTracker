using System;
using System.Collections.Generic;

namespace CoCStatsTracker.UIEntities;
public class CurrentPrizeDrawUi
{
    public string ClanTag { get; set; }
    public string ClanName { get; set; }
    public DateTime StartOn { get; set; }
    public DateTime EndedOn { get; set; }
    public string Description { get; set; }

    public ICollection<ParticipantsUi> Participants { get; set; }
}

public class ParticipantsUi
{
    public string Tag { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public int WarStarsCount { get; set; }
    public int DonationsSentCount { get; set; }
    public int CapitalContributionsCount { get; set; }
    public int CarmaScore { get; set; }
    public int TotalDrawScore { get; set; }
}
