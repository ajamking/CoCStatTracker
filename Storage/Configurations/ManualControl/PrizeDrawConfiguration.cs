using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storage.Configurations.ClanWars
{
    public class PrizeDrawConfiguration : IEntityTypeConfiguration<PrizeDraw>
    {
        public void Configure(EntityTypeBuilder<PrizeDraw> builder)
        {
            builder.ToTable("PrizeDraws");
            builder.Property(p => p.StartedOn).IsRequired();
            builder.Property(p => p.EndedOn).IsRequired();
        }
    }
}
