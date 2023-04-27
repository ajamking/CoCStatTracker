using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storage.Configurations.ClanWars
{
    public class CapitalRaidConfigurartion : IEntityTypeConfiguration<CapitalRaid>
    {
        public void Configure(EntityTypeBuilder<CapitalRaid> builder)
        {
            builder.ToTable("CapitalRaids");
            builder.Property(p => p.StartedOn).IsRequired();
            builder.Property(p => p.EndedOn).IsRequired();
        }
    }
}
