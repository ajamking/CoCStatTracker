using CoCApiDealer.ForTests;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection.Emit;

namespace Storage
{
    public class TestDbContext : DbContext
    {
        public string ConnectionString { get; }

        public DbSet<TrackedClanTest> TrackedClans { get; set; }
        public DbSet<CapitalRaidTest> CapitalRaids { get; set; }
        public DbSet<ClanMemberTest> ClanMembers { get; set; }
        public DbSet<RaidMemberTest> RaidMembers { get; set; }
        public DbSet<RaidAttackTest> RaidAttacks { get; set; }


        public TestDbContext(string connectionString, bool firstStrat = false)
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
            modelBuilder.Entity<RaidAttackTest>()
                .HasOne<RaidMemberTest>(x => x.RaidMember)
                .WithMany(x => x.Attacks)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RaidMemberTest>()
               .HasOne<CapitalRaidTest>(x => x.Raid)
               .WithMany(x => x.RaidMembers)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RaidMemberTest>()
              .HasOne<ClanMemberTest>(x => x.ClanMember)
              .WithMany(x => x.RaidMemberships)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CapitalRaidTest>()
             .HasOne<TrackedClanTest>(x => x.Clan)
             .WithMany(x => x.CapitalRaids)
             .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClanMemberTest>()
            .HasOne<TrackedClanTest>(x => x.Clan)
            .WithMany(x => x.ClanMembers)
            .OnDelete(DeleteBehavior.Cascade);
        }

        public int Complete()
        {
            return SaveChanges();
        }
    }
}
