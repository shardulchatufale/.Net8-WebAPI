// OurHeroDbContext.cs
using DotNet8WebAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace DotNet8WebAPI.Entity
{
    public class OurHeroDbContext : DbContext
    {
        public OurHeroDbContext(DbContextOptions<OurHeroDbContext> options) : base(options)
        {
        }
 
        public DbSet<OurHero> OurHeros { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<OurHero>().HasKey(x => x.Id);


            modelBuilder.Entity<OurHero>().HasData(
                new OurHero
                {
                    Id = 1,
                    FirstName = "System",
                    LastName = "",
                    isActive = true,
                }
            );
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName="System",
                    LastName="",
                    UserName="System",
                    Password="System",
                }
                );


        }
    }
}