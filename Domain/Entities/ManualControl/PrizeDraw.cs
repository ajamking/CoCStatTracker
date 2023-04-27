using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class PrizeDraw
    {
        public int Id { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime EndedOn { get; set; }
        public string WinnerName { get; set; }
        public int WinnerTotalScore { get; set; }
        public string Description { get; set; }

        public int TrackedClanId { get; set; }
        public virtual TrackedClan TrackedClan { get; set; }

        public virtual ICollection<DrawMember> Members { get; set; }

        public PrizeDraw()
        {
            Members = new HashSet<DrawMember>();
        }
    }
}
