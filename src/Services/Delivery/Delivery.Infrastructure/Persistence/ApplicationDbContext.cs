﻿using Delivery.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Infrastructure.Persistence
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
        public virtual DbSet<CargoItem> CargoItems { get; set; } = null!;
        public virtual DbSet<Domain.Entities.Delivery> Deliveries { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-CHB8USB;Database=CargoDelivery;Trusted_Connection=True;");
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Entities.Cargo>(entity =>
            {
                entity.ToTable("Cargo");

                entity.Property(e => e.CargoId).HasMaxLength(50);

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.DebitId).HasMaxLength(50);

                entity.Property(e => e.Route).HasMaxLength(4000);
            });

            modelBuilder.Entity<CargoItem>(entity =>
            {
                entity.ToTable("CargoItem");

                entity.Property(e => e.CargoItemId).HasMaxLength(50);

                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.Barcode).HasMaxLength(50);

                entity.Property(e => e.CargoId).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Desi).HasMaxLength(50);

                entity.Property(e => e.Kg).HasMaxLength(50);

                entity.Property(e => e.WaybillNumber).HasMaxLength(50);

                entity.HasOne(d => d.Cargo)
                    .WithMany(p => p.CargoItems)
                    .HasForeignKey(d => d.CargoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CargoItem_Cargo");
            });

            modelBuilder.Entity<Domain.Entities.Delivery>(entity =>
            {
                entity.ToTable("Delivery");

                entity.Property(e => e.DeliveryId).HasMaxLength(50);

                entity.Property(e => e.CargoId).HasMaxLength(50);

                entity.Property(e => e.CorrelationId).HasMaxLength(50);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Cargo)
                    .WithMany(p => p.Deliveries)
                    .HasForeignKey(d => d.CargoId)
                    .HasConstraintName("FK_Delivery_Cargo");
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
