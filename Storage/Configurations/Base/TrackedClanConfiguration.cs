using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storage.Configurations.Base;

public class TrackedClanConfiguration : IEntityTypeConfiguration<TrackedClan>
{
    public void Configure(EntityTypeBuilder<TrackedClan> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable("TrackedClans");
        builder.Property(p => p.UpdatedOn).IsRequired();
        builder.Property(p => p.Tag).IsRequired();
        builder.Property(p => p.Name).IsRequired();
    }
}