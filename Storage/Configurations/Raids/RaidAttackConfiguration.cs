﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storage.Configurations.ClanWars
{
    public class RaidAttackConfiguration : IEntityTypeConfiguration<RaidAttack>
    {
        public void Configure(EntityTypeBuilder<RaidAttack> builder)
        {
            builder.ToTable("RaidAttacks");
            builder.Property(p => p.DestructionPercentFrom).IsRequired();
            builder.Property(p => p.DestructionPercentTo).IsRequired();

            builder
               .HasOne<CapitalRaid>(x => x.Raid)
               .WithMany(x => x.RaidAttacks)
               .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne<RaidMember>(x => x.RaidMember)
                .WithMany(x => x.Attacks)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne<OpponentDistrict>(x => x.OpponentDistrict)
                .WithMany(x => x.Attacks)
                .OnDelete(DeleteBehavior.Cascade);

            
        }
    }
}
