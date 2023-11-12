using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class RaidMember : Entity
    {
        public DateTime UpdatedOn { get; set; }
        public int TotalLoot { get; set; }
        public string Tag { get; set; }
        public string Name { get; set; }
        public int? ClanMemberId { get; set; }
        public virtual ClanMember ClanMember { get; set; }
        public int? CapitalRaidId { get; set; }
        public virtual CapitalRaid CapitalRaid { get; set; }

        public virtual ICollection<RaidAttack> Attacks { get; set; }

        public RaidMember()
        {
            Attacks = new HashSet<RaidAttack>();
        }
    }
}