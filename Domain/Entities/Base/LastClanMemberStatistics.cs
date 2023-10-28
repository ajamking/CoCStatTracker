using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class LastClanMemberStatistics : Entity
{
    public DateTime UpdatedOn { get; set; }

    public int? TrackedClanId { get; set; }
    public virtual TrackedClan Clan { get; set; }

    public virtual ICollection<ClanMember> ObsoleteClanMembers { get; set; }

    public LastClanMemberStatistics()
    {
        ObsoleteClanMembers = new HashSet<ClanMember>();
    }
}
