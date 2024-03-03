using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class CapitalRaid : Entity
{
    public DateTime UpdatedOn { get; set; }
    public string State { get; set; }
    public DateTime StartedOn { get; set; }
    public DateTime EndedOn { get; set; }
    public int TotalLoot { get; set; }
    public int TotalAttacks { get; set; }
    public int EnemyDistrictsDestoyed { get; set; }
    public int OffensiveReward { get; set; }
    public int DefenSiveReward { get; set; }

    public int? TrackedClanId { get; set; }
    public virtual TrackedClan TrackedClan { get; set; }

    public virtual ICollection<RaidDefense> RaidDefenses { get; set; }
    public virtual ICollection<AttackedClanOnRaid> AttackedClans { get; set; }
    public virtual ICollection<RaidMember> RaidMembers { get; set; }


    public CapitalRaid()
    {
        RaidDefenses = new HashSet<RaidDefense>();
        AttackedClans = new HashSet<AttackedClanOnRaid>();
        RaidMembers = new HashSet<RaidMember>();
    }
}