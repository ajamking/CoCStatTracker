using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storage.Configurations.ClanWars
{
    public class WarMemberConfiguration : IEntityTypeConfiguration<WarMember>
    {
        public void Configure(EntityTypeBuilder<WarMember> builder)
        {
            builder.ToTable("WarMembers");

            builder
           .HasOne<ClanWar>(x => x.ClanWar)
           .WithMany(x => x.WarMembers)
           .OnDelete(DeleteBehavior.Cascade);

            builder
           .HasOne<ClanMember>(x => x.ClanMember)
           .WithMany(x => x.WarMemberships)
           .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
