using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class ClanMember : Entity
    {
        public string TelegramUserName { get; set; }
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
        public int TotalCapitalGoldContributed { get; set; }
        public int TotalCapitalGoldLooted { get; set; }

        public string League { get; set; }

        public int? TrackedClanId { get; set; }
        public virtual TrackedClan TrackedClan { get; set; }

        public virtual ICollection<Troop> Units { get; set; }
        public virtual ICollection<WarMember> WarMemberships { get; set; }
        public virtual ICollection<RaidMember> RaidMemberships { get; set; }

        public ClanMember()
        {
            Units = new HashSet<Troop>();
            WarMemberships = new HashSet<WarMember>();
            RaidMemberships = new HashSet<RaidMember>();
        }
    }
}