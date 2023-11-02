using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class CapitalRaid : Entity
    {
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

        public virtual ICollection<RaidMember> RaidMembers { get; set; }
        public virtual ICollection<RaidDefense> RaidDefenses { get; set; }

        public CapitalRaid()
        {
            RaidMembers = new HashSet<RaidMember>();
            RaidDefenses = new HashSet<RaidDefense>();
        }
    }
}
