using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storage.Configurations.Base
{
    public class ClanMemberConfiguration : IEntityTypeConfiguration<ClanMember>
    {
        public void Configure(EntityTypeBuilder<ClanMember> builder)
        {
            builder.ToTable("ClanMembers");
            builder.Property(p => p.UpdatedOn).IsRequired();
            builder.Property(p => p.Tag).IsRequired();
            builder.Property(p => p.Name).IsRequired();

            builder
           .HasOne<TrackedClan>(x => x.Clan)
           .WithMany(x => x.ClanMembers)
           .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
