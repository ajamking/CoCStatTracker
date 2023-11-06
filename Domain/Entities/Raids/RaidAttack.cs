namespace Domain.Entities
{
    public class RaidAttack : Entity
    {
        public string MemberTag { get; set; }
        public string MemberName { get; set; }

        public string OpponentClanTag { get; set; }
        public string OpponentClanName { get; set; }
        public int OpponentClanLevel { get; set; }
        public string OpponentDistrictName { get; set; }
        public int OpponentDistrictLevel { get; set; }

        public int DestructionPercentFrom { get; set; }
        public int DestructionPercentTo { get; set; }

        public int? RaidMemberId { get; set; }
        public virtual RaidMember RaidMember { get; set; }
    }
}