using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AD_Entities.Models
{
    public partial class Alaska_DemoContext : DbContext
    {
        public Alaska_DemoContext()
        {
        }

        public Alaska_DemoContext(DbContextOptions<Alaska_DemoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Travel> Travel { get; set; }
        public virtual DbSet<TravelType> TravelType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //optionsBuilder.UseSqlServer("Server=DESKTOP-R75ENUQ;Database=Alaska_Demo;Trusted_Connection=True;");
                optionsBuilder.UseSqlServer("");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.EmployeeCode).HasMaxLength(50);
            });

            modelBuilder.Entity<Travel>(entity =>
            {
                entity.Property(e => e.TravelCompletetionDate).HasColumnType("datetime");

                entity.Property(e => e.TravelInititionDate).HasColumnType("datetime");

                entity.Property(e => e.TravelStatusCode).HasMaxLength(50);
            });

            modelBuilder.Entity<TravelType>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TypeCode)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
