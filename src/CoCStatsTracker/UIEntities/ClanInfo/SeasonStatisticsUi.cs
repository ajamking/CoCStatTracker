using System;

namespace CoCStatsTracker.UIEntities;

public class SeasonStatisticsUi : UiEntity
{
    public DateTime InitializedOn { get; set; }
    public string ClanTag { get; set; }
    public string ClanName { get; set; }
    public string Name { get; set; }
    public string Tag { get; set; }
    public int DonationsSend { get; set; }
    public int DonationRecieved { get; set; }
    public int CapitalContributions { get; set; }
    public int WarStarsEarned { get; set; }
    public int AttackWins { get; set; }
    public int VersusBattleWins { get; set; }
}