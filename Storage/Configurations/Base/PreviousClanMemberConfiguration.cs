using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storage.Configurations.Base
{
    public class PreviousClanMemberConfiguration : IEntityTypeConfiguration<PreviousClanMember>
    {
        public void Configure(EntityTypeBuilder<PreviousClanMember> builder)
        {
            builder.ToTable("PreviousClanMembers");
            builder.Property(p => p.UpdatedOn).IsRequired();
            builder.Property(p => p.Tag).IsRequired();
            builder.Property(p => p.Name).IsRequired();

            builder
           .HasOne<TrackedClan>(x => x.Clan)
           .WithMany(x => x.PreviousClanMembersStaticstics)
           .HasForeignKey(t => t.TrackedClanId)
           .OnDelete(DeleteBehavior.Cascade);
        }
    }
}