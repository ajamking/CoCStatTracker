using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storage.Configurations.Raids;

public class RaidDefenseConfiguration : IEntityTypeConfiguration<RaidDefense>
{
    public void Configure(EntityTypeBuilder<RaidDefense> builder)
    {
        builder.ToTable("RaidDefense");

        builder
            .HasOne<CapitalRaid>(x => x.CapitalRaid)
            .WithMany(x => x.RaidDefenses)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

