using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Storage.Configurations.Base;
using Storage.Configurations.ClanWars;
using Storage.Configurations.Raids;
using System.Reflection.Emit;

namespace Storage
{
    public class AppDbContext : DbContext, ICoCDbContext
    {
        public string ConnectionString { get; }
      
        public DbSet<TrackedClan> TrackedClans { get; set; }
        public DbSet<ClanMember> ClanMembers { get; set; }
        public DbSet<InitialClanMembersStaticstics> InitialClanMembersStaticstics { get; set; }
        public DbSet<Troop> Units { get; set; }
        
        public DbSet<ClanWar> ClanWars { get; set; }
        public DbSet<EnemyWarMember> EnemyWarMembers { get; set; }
        public DbSet<WarAttack> WarAttacks { get; set; }
        public DbSet<WarMember> WarMembers { get; set; }

        public DbSet<CapitalRaid> CapitalRaids { get; set; }
        public DbSet<DefeatedClan> DefeatedClans { get; set; }
        public DbSet<OpponentDistrict> OpponentDistricts { get; set; }
        public DbSet<RaidAttack> RaidAttacks { get; set; }
        public DbSet<RaidMember> RaidMembers { get; set; }
        public DbSet<RaidDefense> RaidDefenses { get; set; }
        public DbSet<DestroyedFriendlyDistrict> DestroyedFriendlyDistricts { get; set; }

        public AppDbContext(string connectionString, bool firstStrat = false)
        {
            ConnectionString = connectionString;

            if (firstStrat)
            {
                Database.EnsureDeleted();
            }

            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlite(ConnectionString);

            //optionsBuilder.LogTo(Console.WriteLine); //удалить и сделать нормальный

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClanMemberConfiguration());
            modelBuilder.ApplyConfiguration(new TrackedClanConfiguration());
            modelBuilder.ApplyConfiguration(new UnitConfiguration());
            modelBuilder.ApplyConfiguration(new InitialClanMembersStaticsticsConfiguration());
            
            modelBuilder.ApplyConfiguration(new ClanWarConfiguration());
            modelBuilder.ApplyConfiguration(new EnemyWarMemberConfiguration());
            modelBuilder.ApplyConfiguration(new WarAttackConfiguration());
            modelBuilder.ApplyConfiguration(new WarMemberConfiguration());

            modelBuilder.ApplyConfiguration(new CapitalRaidConfigurartion());
            modelBuilder.ApplyConfiguration(new DefeatedClanConfiguration());
            modelBuilder.ApplyConfiguration(new EnemyDistrictConfiguration());
            modelBuilder.ApplyConfiguration(new RaidAttackConfiguration());
            modelBuilder.ApplyConfiguration(new RaidMemberConfiguration());
            modelBuilder.ApplyConfiguration(new RaidDefenseConfiguration());
            modelBuilder.ApplyConfiguration(new DestroyedFriendlyDistrictConfiguration());
        }
    }
}
