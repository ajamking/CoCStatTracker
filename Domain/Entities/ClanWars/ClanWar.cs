using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class ClanWar : Entity
    {
        public DateTime UpdatedOn { get; set; }

        public bool IsCWL { get; set; }
        public string Result { get; set; }

        public string State { get; set; }
        public int TeamSize { get; set; }
        public int AttackPerMember { get; set; }

        public DateTime PreparationStartTime { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime EndedOn { get; set; }

        public int AttacksCount { get; set; }
        public int StarsCount { get; set; }
        public double DestructionPercentage { get; set; }

        public string OpponentClanTag { get; set; }
        public string OpponentClanName { get; set; }
        public int OpponentClanLevel { get; set; }
        public int OpponentAttacksCount { get; set; }
        public int OpponentStarsCount { get; set; }
        public double OpponentDestructionPercentage { get; set; }

        public int? TrackedClanId { get; set; }
        public virtual TrackedClan TrackedClan { get; set; }

        public virtual ICollection<WarMember> WarMembers { get; set; }
        public virtual ICollection<EnemyWarMember> EnemyWarMembers { get; set; }

        public ClanWar()
        {
            WarMembers = new HashSet<WarMember>();
            EnemyWarMembers = new HashSet<EnemyWarMember>();
        }
    }
}
