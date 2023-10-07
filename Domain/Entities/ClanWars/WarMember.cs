using System.Collections.Generic;

namespace Domain.Entities
{
    public class WarMember : Entity
    {
        public string Tag { get; set; }
        public string Name { get; set; }
        public int TownHallLevel { get; set; }
        public int MapPosition { get; set; }
        public int BestOpponentStars { get; set; }
        public int BestOpponentTime { get; set; }
        public int BestOpponentPercent { get; set; }

        public int? ClanWarId { get; set; }
        public virtual ClanWar ClanWar { get; set; }
        public int? ClanMemberId { get; set; }
        public virtual ClanMember ClanMember { get; set; }

        public virtual ICollection<WarAttack> WarAttacks { get; set; }

        public WarMember()
        {
            WarAttacks = new HashSet<WarAttack>();

        }
    }
}
