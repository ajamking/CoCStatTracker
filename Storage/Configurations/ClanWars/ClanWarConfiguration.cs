using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storage.Configurations.ClanWars;

public class ClanWarConfiguration : IEntityTypeConfiguration<ClanWar>
{
    public void Configure(EntityTypeBuilder<ClanWar> builder)
    {
        builder.ToTable("ClanWars");
        builder.Property(p => p.IsCWL).IsRequired();
        builder.Property(p => p.PreparationStartTime).IsRequired();
        builder.Property(p => p.StartedOn).IsRequired();
        builder.Property(p => p.EndedOn).IsRequired();
        builder.Property(p => p.OpponentsTag).IsRequired();
        builder.Property(p => p.OpponentsName).IsRequired();
        builder.Property(p => p.OpponentsLevel).IsRequired();

        builder
      .HasOne<TrackedClan>(x => x.TrackedClan)
      .WithMany(x => x.ClanWars)
      .HasForeignKey(t => t.TrackedClanId)
      .OnDelete(DeleteBehavior.Cascade);
    }
}