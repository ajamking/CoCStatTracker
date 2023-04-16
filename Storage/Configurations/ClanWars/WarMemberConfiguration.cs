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
            builder.Property(p => p.ClanWarId).IsRequired();
            builder.Property(p => p.ClanMemberId).IsRequired();
        }
    }
}
