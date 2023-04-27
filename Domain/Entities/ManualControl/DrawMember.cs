namespace Domain.Entities
{
    public class DrawMember
    {
        public int Id { get; set; }
        public int TotalPointsEarned { get; set; }

        public int? ClanMemberId { get; set; }
        public virtual ClanMember ClanMember { get; set; }
        public int PrizeDrawId { get; set; }
        public virtual PrizeDraw PrizeDraw { get; set; }
    }
}
