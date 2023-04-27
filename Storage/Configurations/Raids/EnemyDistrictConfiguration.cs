using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storage.Configurations.ClanWars
{
    public class EnemyDistrictConfiguration : IEntityTypeConfiguration<OpponentDistrict>
    {
        public void Configure(EntityTypeBuilder<OpponentDistrict> builder)
        {
            builder.ToTable("EnemyDistricts");
            builder.Property(p => p.Name).IsRequired();
        }
    }
}
