using System.Collections.Generic;

namespace CoCStatsTracker.UIEntities;

public class TrackedClanUi : UiEntity
{
    public bool NewsLetterOn { get; set; }
    public string ClanChatId { get; set; }
    public string AdminsKey { get; set; }
    public bool IsInBlackList { get; set; }
    public string Tag { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public int ClanLevel { get; set; }
    public int ClanMembersCount { get; set; }
    public int ClanPoints { get; set; }
    public int ClanVersusPoints { get; set; }
    public int ClanCapitalPoints { get; set; }
    public string CapitalLeague { get; set; }
    public string IsWarLogPublic { get; set; }
    public string WarLeague { get; set; }
    public int WarWinStreak { get; set; }
    public int WarWins { get; set; }
    public int WarDraws { get; set; }
    public int WarLoses { get; set; }
    public int CapitalHallLevel { get; set; }
    public List<ClanMemberUi> ClanMembers { get; set; }
}