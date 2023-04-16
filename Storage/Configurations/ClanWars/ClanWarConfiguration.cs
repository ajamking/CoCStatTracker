﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storage.Configurations.ClanWars
{
    public class ClanWarConfiguration : IEntityTypeConfiguration<ClanWar>
    {
        public void Configure(EntityTypeBuilder<ClanWar> builder)
        {
            builder.ToTable("ClanWars");
            builder.Property(p => p.IsCWL).IsRequired();
            builder.Property(p => p.PreparationStartTime).IsRequired();
            builder.Property(p => p.StartTime).IsRequired();
            builder.Property(p => p.EndTime).IsRequired();
            builder.Property(p => p.OpponentClanTag).IsRequired();
            builder.Property(p => p.OpponentClanName).IsRequired();
            builder.Property(p => p.OpponentClanLevel).IsRequired();
        }
    }
}
