using System.Collections.Generic;

namespace Domain.Entities
{
    public class WarMember
    {
        public int Id { get; set; }
        public int MapPosition { get; set; }
        public int BestOpponentStars { get; set; }
        public int BestOpponentTime { get; set; }
        public int BestOpponentPercent { get; set; }

        public int? ClanWarId { get; set; }
        public ClanWar ClanWar { get; set; }
        public int? ClanMemberId { get; set; }
        public ClanMember ClanMember { get; set; }
        public ICollection<WarAttack> WarAttacks { get; set; }

        public WarMember()
        {
            WarAttacks = new HashSet<WarAttack>();

        }
    }
}
