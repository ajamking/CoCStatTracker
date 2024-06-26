﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Storage;

public interface ICoCDbContext
{
    public DbSet<TrackedClan> TrackedClans { get; set; }
    public DbSet<ClanMember> ClanMembers { get; set; }
    public DbSet<Troop> Units { get; set; }
    public DbSet<PreviousClanMember> PreviousClanMembers { get; set; }

    public DbSet<ClanWar> ClanWars { get; set; }
    public DbSet<EnemyWarMember> EnemyWarMembers { get; set; }
    public DbSet<WarAttack> WarAttacks { get; set; }
    public DbSet<WarMember> WarMembers { get; set; }

    public DbSet<CapitalRaid> CapitalRaids { get; set; }
    public DbSet<RaidMember> RaidAttacks { get; set; }
    public DbSet<RaidMember> RaidMembers { get; set; }
    public DbSet<AttackedClanOnRaid> AttackedClanOnRaid { get; set; }
    public DbSet<RaidDefense> RaidDefenses { get; set; }
    public DbSet<DestroyedFriendlyDistrict> DestroyedFriendlyDistricts { get; set; }
}