using Microsoft.EntityFrameworkCore;
using NanatsuDle.Models;

namespace NanatsuDle.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Character> Characters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Character>().ToTable("Characters");
            modelBuilder.Entity<Gender>().ToTable("Genders");
            modelBuilder.Entity<Race>().ToTable("Races");
            modelBuilder.Entity<Arc>().ToTable("Arcs");
            modelBuilder.Entity<Affiliation>().ToTable("Affiliations");
            modelBuilder.Entity<HairColor>().ToTable("HairColors");
            modelBuilder.Entity<TypeOfSkill>().ToTable("TypesOfSkills");
        }
    }
}
