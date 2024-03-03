using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storage.Configurations.ClanWars;

public class AttackedClanOnRaidConfiguration : IEntityTypeConfiguration<AttackedClanOnRaid>
{
    public void Configure(EntityTypeBuilder<AttackedClanOnRaid> builder)
    {
        builder.ToTable("AttackedClansOnRaid");

        builder.Property(p => p.CapitalRaidId).IsRequired();

        builder
       .HasOne<CapitalRaid>(x => x.CapitalRaid)
       .WithMany(x => x.AttackedClans)
       .HasForeignKey(x => x.CapitalRaidId)
       .OnDelete(DeleteBehavior.Cascade);

        builder
       .HasMany(x => x.DefeatedEmemyDistricts)
       .WithOne()
       .OnDelete(DeleteBehavior.Cascade);
    }
}