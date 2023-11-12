using System;

namespace Domain.Entities
{
    public class PreviousClanMember : Entity
    {
        public DateTime UpdatedOn { get; set; }

        public int? TrackedClanId { get; set; }
        public virtual TrackedClan Clan { get; set; }

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
    }
}