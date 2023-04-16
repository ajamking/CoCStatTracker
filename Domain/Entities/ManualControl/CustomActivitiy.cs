using System;

namespace Domain.Entities
{
    public class CustomActivity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int EarnedPoints { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int? CarmaId { get; set; }
        public Carma Carma { get; set; }
    }
}
