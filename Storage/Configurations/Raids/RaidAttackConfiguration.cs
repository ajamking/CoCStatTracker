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
            // builder.Property(p => p.DestructionPercentFrom).IsRequired(); // Отказались пока
            builder.Property(p => p.DestructionPercentTo).IsRequired();
            // builder.Property(p => p.RaidMemberId).IsRequired();
            builder.Property(p => p.OpponentDistrictId).IsRequired();
        }
    }
}
