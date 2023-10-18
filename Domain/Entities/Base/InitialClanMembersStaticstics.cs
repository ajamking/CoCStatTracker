using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class InitialClanMembersStaticstics : Entity
{
    public DateTime UpdatedOn { get; set; }

    public int? TrackedClanId { get; set; }
    public virtual TrackedClan Clan { get; set; }

    public virtual ICollection<ClanMember> InitialClanMembers { get; set; }

    public InitialClanMembersStaticstics()
    {
        InitialClanMembers = new HashSet<ClanMember>();
    }
}
