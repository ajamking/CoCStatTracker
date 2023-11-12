using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storage.Configurations.ClanWars
{
    public class DestroyedFriendlyDistrictConfiguration : IEntityTypeConfiguration<DestroyedFriendlyDistrict>
    {
        public void Configure(EntityTypeBuilder<DestroyedFriendlyDistrict> builder)
        {
            builder.ToTable("DestroyedFriendlyDistricts");

            builder
              .HasOne<RaidDefense>(x => x.RaidDefense)
              .WithMany(x => x.DestroyedFriendlyDistricts)
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
