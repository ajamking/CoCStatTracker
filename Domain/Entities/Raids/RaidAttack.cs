using System.Globalization;

namespace Domain.Entities
{
    public class RaidAttack
    {
        public int Id { get; set; }
        public int DestructionPercentFrom { get; set; }
        public int DestructionPercentTo { get; set; }

        public string MemberTag { get; set; }
        public string MemberName { get; set; }

        public int? RaidMemberId { get; set; }
        public virtual RaidMember RaidMember { get; set; }
        public int? OpponentDistrictId { get; set; }
        public virtual OpponentDistrict OpponentDistrict { get; set; }

    }
}
