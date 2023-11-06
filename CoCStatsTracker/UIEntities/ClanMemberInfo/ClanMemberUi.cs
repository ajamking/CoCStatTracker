namespace CoCStatsTracker.UIEntities;

public class ClanMemberUi : UiEntity
{
    public string Tag { get; set; }
    public string Name { get; set; }
    public string ClanTag { get; set; }
    public string ClanName { get; set; }
    public string RoleInClan { get; set; }
    public int ExpLevel { get; set; }
    public int TownHallLevel { get; set; }
    public int TownHallWeaponLevel { get; set; }
    public int Trophies { get; set; }
    public int BestTrophies { get; set; }
    public string League { get; set; }
    public int VersusTrophies { get; set; }
    public int BestVersusTrophies { get; set; }
    public int AttackWins { get; set; }
    public int DefenseWins { get; set; }
    public string WarPreference { get; set; }
    public int DonationsSent { get; set; }
    public int DonationsRecieved { get; set; }
    public int WarStars { get; set; }
    public int TotalCapitalContributed { get; set; }
    public int TotalCapitalGoldLooted { get; set; }
    public int CwMedianDP { get; set; }
    public int CwMedianDPWithout14_15Th { get; set; }
    public int RaidsMedianDP { get; set; }
    public int RaidsMedianDPWithoutPeak { get; set; }
}