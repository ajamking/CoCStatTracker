namespace Domain.Entities
{
    public class WarAttack : Entity
    {
        public int AttackOrder { get; set; }
        public int Stars { get; set; }
        public int DestructionPercent { get; set; }
        public int Duration { get; set; }

        public int? EnemyWarMemberId { get; set; }
        public virtual EnemyWarMember EnemyWarMember { get; set; }
        public int? WarMemberId { get; set; }
        public virtual WarMember WarMember { get; set; }
    }
}
