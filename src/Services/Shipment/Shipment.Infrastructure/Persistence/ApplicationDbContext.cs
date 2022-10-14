using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Shipment.Application.Common.Interfaces;

namespace Shipment.Infrastructure.Persistence
{
    public partial class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Domain.Entities.Cargo> Cargos { get; set; } = null!;
        public virtual DbSet<ShipmentType> ShipmentTypes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-CHB8USB;Database=CargoShipment;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Entities.Cargo>(entity =>
            {
                entity.ToTable("Cargo");

                entity.Property(e => e.CargoId).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DebitId).HasMaxLength(50);

                entity.Property(e => e.ShipmentTypeId).HasMaxLength(50);

                entity.HasOne(d => d.ShipmentType)
                    .WithMany(p => p.Cargos)
                    .HasForeignKey(d => d.ShipmentTypeId)
                    .HasConstraintName("FK_Cargo_ShipmentType");
            });

            modelBuilder.Entity<ShipmentType>(entity =>
            {
                entity.ToTable("ShipmentType");

                entity.Property(e => e.ShipmentTypeId).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
