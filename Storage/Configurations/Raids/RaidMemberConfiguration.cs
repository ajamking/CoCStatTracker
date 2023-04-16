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
            builder.Property(p => p.ClanMemberId).IsRequired();
            builder.Property(p => p.CapitalRaidId).IsRequired();
        }
    }
}
