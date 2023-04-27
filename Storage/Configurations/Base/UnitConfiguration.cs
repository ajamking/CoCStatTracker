using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Storage.Configurations.Base
{
    public class UnitConfiguration : IEntityTypeConfiguration<Troop>
    {
        public void Configure(EntityTypeBuilder<Troop> builder)
        {
            builder.ToTable("Units");
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Type).IsRequired().HasConversion(new EnumToStringConverter<UnitType>());
            builder.Property(p => p.Village).IsRequired();
            builder.Property(p => p.Level).IsRequired();
        }
    }
}
