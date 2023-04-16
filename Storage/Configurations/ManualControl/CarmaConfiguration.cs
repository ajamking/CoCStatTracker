using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storage.Configurations.ClanWars
{
    public class CarmaConfiguration : IEntityTypeConfiguration<Carma>
    {
        public void Configure(EntityTypeBuilder<Carma> builder)
        {
            builder.ToTable("Carmas");
            builder.Property(p => p.UpdatedOn).IsRequired();
        }
    }
}
