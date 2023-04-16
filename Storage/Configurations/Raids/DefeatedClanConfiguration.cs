using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storage.Configurations.ClanWars
{
    public class DefeatedClanConfiguration : IEntityTypeConfiguration<DefeatedCapital>
    {
        public void Configure(EntityTypeBuilder<DefeatedCapital> builder)
        {
            builder.ToTable("DefeatedClans");
            builder.Property(p => p.DefendersTag).IsRequired();
            builder.Property(p => p.DefendersName).IsRequired();
            builder.Property(p => p.CapitalRaidId).IsRequired();
        }
    }
}
