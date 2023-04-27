using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storage.Configurations.ClanWars
{
    public class DestroyedFriendlyDistrictConfiguration : IEntityTypeConfiguration<DestroyedFriendlyDistrict>
    {
        public void Configure(EntityTypeBuilder<DestroyedFriendlyDistrict> builder)
        {
            builder.ToTable("DestroyedFriendlyDistrict");
        }
    }
}
