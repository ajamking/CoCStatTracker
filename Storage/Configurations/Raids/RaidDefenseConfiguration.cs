using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storage.Configurations.Raids;

public class RaidDefenseConfiguration : IEntityTypeConfiguration<RaidDefense>
{
    public void Configure(EntityTypeBuilder<RaidDefense> builder)
    {
        builder.ToTable("RaidDefense");
        builder.Property(p => p.CapitalRaidId).IsRequired();
    }
}

