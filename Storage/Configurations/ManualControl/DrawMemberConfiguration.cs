using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storage.Configurations.ClanWars
{
    public class DrawMemberConfiguration : IEntityTypeConfiguration<DrawMember>
    {
        public void Configure(EntityTypeBuilder<DrawMember> builder)
        {
            builder.ToTable("DrawMembers");
            builder.Property(p => p.ClanMemberId).IsRequired();
        }
    }
}
