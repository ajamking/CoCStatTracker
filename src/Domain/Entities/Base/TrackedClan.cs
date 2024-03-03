using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class TrackedClan : Entity
{
    public string ClansTelegramChatId { get; set; }
    public bool RegularNewsLetterOn { get; set; }
    public int WarTimeToMessageBeforeEnd { get; set; }
    public bool WarStartMessageOn { get; set; }
    public bool WarEndMessageOn { get; set; }
    public int RaidTimeToMessageBeforeEnd { get; set; }
    public bool RaidStartMessageOn { get; set; }
    public bool RaidEndMessageOn { get; set; }

    public string AdminsKey { get; set; }
    public bool IsInBlackList { get; set; }

    public DateTime UpdatedOn { get; set; }
    public string Tag { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public int ClanLevel { get; set; }
    public int ClanPoints { get; set; }
    public int ClanVersusPoints { get; set; }
    public int ClanCapitalPoints { get; set; }
    public string CapitalLeague { get; set; }
    public bool IsWarLogPublic { get; set; }
    public string WarLeague { get; set; }
    public int WarWinStreak { get; set; }
    public int WarWins { get; set; }
    public int WarTies { get; set; }
    public int WarLoses { get; set; }
    public int CapitalHallLevel { get; set; }

    public virtual ICollection<PreviousClanMember> PreviousClanMembersStaticstics { get; set; }
    public virtual ICollection<ClanMember> ClanMembers { get; set; }
    public virtual ICollection<ClanWar> ClanWars { get; set; }
    public virtual ICollection<CapitalRaid> CapitalRaids { get; set; }

    public TrackedClan()
    {
        ClanMembers = new HashSet<ClanMember>();
        ClanWars = new HashSet<ClanWar>();
        CapitalRaids = new HashSet<CapitalRaid>();
        PreviousClanMembersStaticstics = new HashSet<PreviousClanMember>();
    }
}