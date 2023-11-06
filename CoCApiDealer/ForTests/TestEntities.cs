namespace CoCApiDealer.ForTests;

public class TrackedClanTest
{
    public int Id { get; set; }
    public string Tag { get; set; }
    public string Name { get; set; }

    public virtual ICollection<CapitalRaidTest> CapitalRaids { get; set; }

    public virtual ICollection<ClanMemberTest> ClanMembers { get; set; }

    public TrackedClanTest()
    {
        ClanMembers = new HashSet<ClanMemberTest>();
        CapitalRaids = new HashSet<CapitalRaidTest>();
    }
}

public class CapitalRaidTest
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int? TrackedClanTestId { get; set; }
    public virtual TrackedClanTest Clan { get; set; }

    public virtual ICollection<RaidMemberTest> RaidMembers { get; set; }

    public CapitalRaidTest()
    {
        RaidMembers = new HashSet<RaidMemberTest>();
    }
}

public class ClanMemberTest
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int? TrackedClanTestId { get; set; }
    public virtual TrackedClanTest Clan { get; set; }

    public virtual ICollection<RaidMemberTest> RaidMemberships { get; set; }

    public ClanMemberTest()
    {
        RaidMemberships = new HashSet<RaidMemberTest>();
    }
}

public class RaidMemberTest
{
    public int Id { get; set; }

    public int? ClanMemberTestId { get; set; }
    public virtual ClanMemberTest ClanMember { get; set; }
    public int? CapitalRaidTestId { get; set; }
    public virtual CapitalRaidTest Raid { get; set; }

    public virtual ICollection<RaidAttackTest> Attacks { get; set; }

    public RaidMemberTest()
    {
        Attacks = new HashSet<RaidAttackTest>();
    }
}

public class RaidAttackTest
{
    public int Id { get; set; }
    public int DestructionPercentFrom { get; set; }
    public int DestructionPercentTo { get; set; }

    public int? RaidMemberTestId { get; set; }
    public virtual RaidMemberTest RaidMember { get; set; }
}
