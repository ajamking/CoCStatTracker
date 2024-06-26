﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storage.Configurations.ClanWars;

public class WarAttackConfiguration : IEntityTypeConfiguration<WarAttack>
{
    public void Configure(EntityTypeBuilder<WarAttack> builder)
    {
        builder.ToTable("WarAttacks");
        builder.Property(p => p.AttackOrder).IsRequired();
        builder.Property(p => p.Stars).IsRequired();
        builder.Property(p => p.DestructionPercent).IsRequired();
        builder.Property(p => p.Duration).IsRequired();

        builder
         .HasOne<EnemyWarMember>(x => x.EnemyWarMember)
         .WithMany()
         .OnDelete(DeleteBehavior.Cascade);

        builder
         .HasOne<WarMember>(x => x.WarMember)
         .WithMany(x => x.WarAttacks)
         .OnDelete(DeleteBehavior.Cascade);
    }
}