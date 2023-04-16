using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storage.Configurations.ClanWars
{
    public class CustomActivitiesConfiguration : IEntityTypeConfiguration<CustomActivity>
    {
        public void Configure(EntityTypeBuilder<CustomActivity> builder)
        {
            builder.ToTable("CustomActivities");
            builder.Property(p => p.UpdatedOn).IsRequired();
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.EarnedPoints).IsRequired();
            builder.Property(p => p.CarmaId).IsRequired();
        }
    }
}
