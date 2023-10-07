namespace Domain.Entities
{
    public class EnemyWarMember : Entity
    {
        public string Tag { get; set; }
        public string Name { get; set; }
        public int THLevel { get; set; }
        public int MapPosition { get; set; }

        public int? ClanWarId { get; set; }
        public virtual ClanWar ClanWar { get; set; }
    }
}
