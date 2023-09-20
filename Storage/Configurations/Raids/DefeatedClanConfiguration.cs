using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storage.Configurations.ClanWars
{
    public class DefeatedClanConfiguration : IEntityTypeConfiguration<DefeatedClan>
    {
        public void Configure(EntityTypeBuilder<DefeatedClan> builder)
        {
            builder.ToTable("DefeatedClans");
            builder.Property(p => p.DefendersTag).IsRequired();
            builder.Property(p => p.DefendersName).IsRequired();

            builder
               .HasOne<CapitalRaid>(x => x.CapitalRaid)
               .WithMany(x => x.DefeatedClans)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
