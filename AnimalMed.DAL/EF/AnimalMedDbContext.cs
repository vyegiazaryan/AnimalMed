using AnimalMed.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalMed.DAL.EF
{
    public class AnimalMedDbContext : DbContext
    {
        public AnimalMedDbContext(DbContextOptions<AnimalMedDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Animal>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<MedicalRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }

        public virtual DbSet<Animal> Animals { get; set; }
        public virtual DbSet<MedicalRecord> MedicalRecords { get; set; }

    }
}
