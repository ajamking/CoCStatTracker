namespace Domain.Entities
{
    public class RaidAttack
    {
        public int Id { get; set; }
        public int DestructionPercentFrom { get; set; }
        public int DestructionPercentTo { get; set; }
        public int? RaidMemberId { get; set; }
        public RaidMember RaidMember { get; set; }
        public int? EnemyDistricrId { get; set; }
        public OpponentDistrict EnemyDistrict { get; set; }

    }
}
