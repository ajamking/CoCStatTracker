using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Carma
    {
        public int Id { get; set; }
        public int TotalCarma { get; set; }
        public DateTime UpdatedOn { get; set; }

        public int ClanMemberId { get; set; }
        public ClanMember ClanMember { get; set; }
        public virtual ICollection<CustomActivity> PlayerActivities { get; set; }

        public Carma()
        {
            PlayerActivities = new HashSet<CustomActivity>();
        }
    }
}
