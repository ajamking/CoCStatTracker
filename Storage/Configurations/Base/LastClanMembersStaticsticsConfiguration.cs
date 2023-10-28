using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storage.Configurations.Base
{
    public class LastClanMembersStaticsticsConfiguration : IEntityTypeConfiguration<LastClanMemberStatistics>
    {
        public void Configure(EntityTypeBuilder<LastClanMemberStatistics> builder)
        {
            builder.ToTable("LastClanMembersStaticstics");
            builder.Property(p => p.UpdatedOn).IsRequired();

            builder
           .HasOne<TrackedClan>(x => x.Clan)
           .WithOne(x => x.LastClanMembersStaticstics)
           .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
