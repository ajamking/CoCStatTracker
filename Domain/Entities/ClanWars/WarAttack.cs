namespace Domain.Entities
{
    public class WarAttack
    {
        public int Id { get; set; }
        public int AttackOrder { get; set; }
        public int Stars { get; set; }
        public int DestructionPercent { get; set; }
        public int Duration { get; set; }

        public int? EnemyWarMemberId { get; set; }
        public EnemyWarMember EnemyWarMember { get; set; }

        public int? WarMemberId { get; set; }
        public WarMember WarMember { get; set; }
    }
}
