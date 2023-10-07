namespace Domain.Entities
{
    public class RaidAttack : Entity
    {
        public int DestructionPercentFrom { get; set; }
        public int DestructionPercentTo { get; set; }

        public string MemberTag { get; set; }
        public string MemberName { get; set; }

        public int? CapitalRaidId { get; set; }
        public virtual CapitalRaid Raid { get; set; }
        public int? RaidMemberId { get; set; }
        public virtual RaidMember RaidMember { get; set; }
        public int? OpponentDistrictId { get; set; }
        public virtual OpponentDistrict OpponentDistrict { get; set; }

    }
}
