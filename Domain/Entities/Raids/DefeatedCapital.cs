using System.Collections.Generic;

namespace Domain.Entities
{
    public class DefeatedCapital
    {
        public int Id { get; set; }
        public string DefendersTag { get; set; }
        public string DefendersName { get; set; }
        public int DefendersLevel { get; set; }
        public int DistrictsCount { get; set; }
        public int DistrictsDestroyedCount { get; set; }
        public int AttacksSpentCount { get; set; }

        public int? CapitalRaidId { get; set; }
        public CapitalRaid CapitalRaid { get; set; }
        public virtual ICollection<OpponentDistrict> DefeatedDistricts { get; set; }

        public DefeatedCapital()
        {
            DefeatedDistricts = new HashSet<OpponentDistrict>();
        }
    }
}
