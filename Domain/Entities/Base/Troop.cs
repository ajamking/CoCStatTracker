namespace Domain.Entities
{
    public class Troop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public string Village { get; set; }
        public bool? SuperTroopIsActivated { get; set; }
        public UnitType Type { get; set; }

        public int? ClanMemberId { get; set; }
        public virtual ClanMember ClanMember { get; set; }
    }

    public enum UnitType
    {
        Hero,
        Unit,
        SuperUnit,
        SiegeMachine,
        Pet
    }
}
