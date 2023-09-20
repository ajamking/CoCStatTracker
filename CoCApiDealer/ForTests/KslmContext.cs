using CoCApiDealer.ForTests;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection.Emit;

namespace Storage
{
    public class KslmContext : DbContext
    {
        public string ConnectionString { get; }

        public DbSet<Emploee> Staff { get; set; }
        public DbSet<Company> Companys { get; set; }


        public KslmContext(string connectionString, bool firstStrat = false)
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


        }

        public int Complete()
        {
            return SaveChanges();
        }
    }
}
