using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storage.Configurations.Base
{
    public class InitialClanMembersStaticsticsConfiguration : IEntityTypeConfiguration<InitialClanMembersStaticstics>
    {
        public void Configure(EntityTypeBuilder<InitialClanMembersStaticstics> builder)
        {
            builder.ToTable("InitialClanMembersStaticstics");
            builder.Property(p => p.UpdatedOn).IsRequired();

            builder
           .HasOne<TrackedClan>(x => x.Clan)
           .WithOne(x => x.InitialClanMembersStaticstics)
           .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
