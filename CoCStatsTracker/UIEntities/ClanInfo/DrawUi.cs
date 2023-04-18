using System;

namespace CoCStatsTracker.UIEntities;

public class DrawUi
{
    public DateTime StartedOn { get; set; }
    public DateTime EndedOn { get; set; }
    public string Winner { get; set; }
    public int WinnersTotalScore { get; set; }
}
