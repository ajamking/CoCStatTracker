using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class PrizeDraw
    {
        public int Id { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime EndedOn { get; set; }
        public string Description { get; set; }
        public int TrackedClanId { get; set; }
        public TrackedClan TrackedClan { get; set; }
        public virtual ICollection<DrawMember> Participants { get; set; }

        public PrizeDraw()
        {
            Participants = new HashSet<DrawMember>();
        }
    }
}
