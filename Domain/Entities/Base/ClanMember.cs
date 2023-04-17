using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class ClanMember
    {
        public int Id { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string Tag { get; set; }
        public string Name { get; set; }

        public int TownHallLevel { get; set; }
        public int TownHallWeaponLevel { get; set; }
        public int ExpLevel { get; set; }
        public int Trophies { get; set; }
        public int BestTrophies { get; set; }
        public int WarStars { get; set; }
        public int AttackWins { get; set; }
        public int DefenceWins { get; set; }

        public int BuilderHallLevel { get; set; }
        public int VersusTrophies { get; set; }
        public int BestVersusTrophies { get; set; }
        public int VersusBattleWins { get; set; }

        public string Role { get; set; }
        public string WarPreference { get; set; }

        public int DonationsSent { get; set; }
        public int DonationsRecieved { get; set; }
        public int TotalCapitalContributions { get; set; }

        public string League { get; set; }

        public int? TrackedClanId { get; set; }
        public TrackedClan Clan { get; set; }
        public int? CarmaId { get; set; }
        public Carma Carma { get; set; }
        public virtual ICollection<Troop> Units { get; set; }
        public virtual ICollection<WarMember> WarMembership { get; set; }
        public virtual ICollection<RaidMember> RaidMembership { get; set; }
        public virtual ICollection<DrawMember> DrawMembership { get; set; }

        public ClanMember()
        {
            Units = new HashSet<Troop>();
            WarMembership = new HashSet<WarMember>();
            RaidMembership = new HashSet<RaidMember>();
            DrawMembership = new HashSet<DrawMember>();
        }

    }
}
