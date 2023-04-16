using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storage.Configurations.ClanWars
{
    public class EnemyWarMemberConfiguration : IEntityTypeConfiguration<EnemyWarMember>
    {
        public void Configure(EntityTypeBuilder<EnemyWarMember> builder)
        {
            builder.ToTable("EnenemyWarMembers");
            builder.Property(p => p.Tag).IsRequired();
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.ClanWarId).IsRequired();
        }
    }
}
