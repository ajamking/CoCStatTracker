using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storage.Configurations.ClanWars
{
    public class RaidMemberConfiguration : IEntityTypeConfiguration<RaidMember>
    {
        public void Configure(EntityTypeBuilder<RaidMember> builder)
        {
            builder.ToTable("RaidMembers");

            builder.Property(p => p.CapitalRaidId).IsRequired();

            builder
           .HasOne<ClanMember>(x => x.ClanMember)
           .WithMany(x => x.RaidMemberships)
           .OnDelete(DeleteBehavior.Cascade);

            builder
           .HasOne<CapitalRaid>(x => x.Raid)
           .WithMany(x => x.RaidMembers)
           .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
