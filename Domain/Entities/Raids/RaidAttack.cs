using Domain.Entities;
using System.Collections;

namespace Domain.Entities
{
    public class RaidAttack : Entity
    {
        public int DestructionPercentFrom { get; set; }
        public int DestructionPercentTo { get; set; }

        public int? DefeatedEmemyDistrictId { get; set; }
        public virtual DefeatedEmemyDistrict DefeatedEmemyDistrict { get; set; }

        public int? RaidMemberId { get; set; }
        public virtual RaidMember RaidMember { get; set; }

        public int? AttackedClanId { get; set; }
        public virtual AttackedClanOnRaid AttackedClan { get; set; }
    }
}