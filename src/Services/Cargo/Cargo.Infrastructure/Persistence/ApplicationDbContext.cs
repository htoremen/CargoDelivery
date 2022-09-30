using System;
using System.Collections.Generic;
using Cargo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Cargo.Infrastructure.Persistence
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

        public virtual DbSet<Cargo.Domain.Entities.Cargo> Cargos { get; set; }
        public virtual DbSet<CargoItem> CargoItems { get; set; }
        public virtual DbSet<Debit> Debits { get; set; }
        public virtual DbSet<DebitHistory> DebitHistories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-CHB8USB;Database=Cargo;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cargo.Domain.Entities.Cargo>(entity =>
            {
                entity.ToTable("Cargo");

                entity.Property(e => e.CargoId).HasMaxLength(50);

                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.DebitId).HasMaxLength(50);

                entity.HasOne(d => d.Debit)
                    .WithMany(p => p.Cargos)
                    .HasForeignKey(d => d.DebitId)
                    .HasConstraintName("FK_Cargo_Debit");
            });

            modelBuilder.Entity<CargoItem>(entity =>
            {
                entity.ToTable("CargoItem");

                entity.Property(e => e.CargoItemId).HasMaxLength(50);

                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.Barcode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CargoId)
                    .IsRequired()
                    .HasMaxLength(50);

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

            modelBuilder.Entity<Debit>(entity =>
            {
                entity.ToTable("Debit");

                entity.HasIndex(e => e.CorrelationId, "IX_Debit")
                    .IsUnique();

                entity.HasIndex(e => e.CourierId, "IX_Debit_1");

                entity.Property(e => e.DebitId).HasMaxLength(50);

                entity.Property(e => e.ApprovingId).HasMaxLength(50);

                entity.Property(e => e.ClosingDate).HasColumnType("datetime");

                entity.Property(e => e.CorrelationId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CourierId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CurrentState).HasMaxLength(50);

                entity.Property(e => e.DistributionDate).HasColumnType("date");

                entity.Property(e => e.Selfie).HasColumnType("ntext");

                entity.Property(e => e.StartingDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<DebitHistory>(entity =>
            {
                entity.ToTable("DebitHistory");

                entity.Property(e => e.DebitHistoryId).HasMaxLength(50);

                entity.Property(e => e.CargoId).HasMaxLength(50);

                entity.Property(e => e.CargoItemId).HasMaxLength(50);

                entity.Property(e => e.CommandName).HasMaxLength(150);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DebitId).HasMaxLength(50);

                entity.Property(e => e.Request).HasMaxLength(4000);

                entity.Property(e => e.Response).HasMaxLength(4000);

                entity.Property(e => e.UserId).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
