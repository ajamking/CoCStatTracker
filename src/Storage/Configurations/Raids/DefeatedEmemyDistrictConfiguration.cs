using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storage.Configurations.ClanWars;

public class DefeatedEmemyDistrictConfiguration : IEntityTypeConfiguration<DefeatedEmemyDistrict>
{
    public void Configure(EntityTypeBuilder<DefeatedEmemyDistrict> builder)
    {
        builder.ToTable("DefeatedEmemyDistricts");

        builder
          .HasOne<AttackedClanOnRaid>(x => x.AttackedClan)
          .WithMany(x => x.DefeatedEmemyDistricts)
          .HasForeignKey(x=>x.AttackedClanOnRaidId)
          .OnDelete(DeleteBehavior.Cascade);
    }
}