using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ApiAeropuertos.Models
{
    public partial class sistem21_avionesafContext : DbContext
    {
        public sistem21_avionesafContext()
        {
        }

        public sistem21_avionesafContext(DbContextOptions<sistem21_avionesafContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Partidas> Partidas { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
          }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8_general_ci")
                .HasCharSet("utf8");

            modelBuilder.Entity<Partidas>(entity =>
            {
                entity.ToTable("partidas");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Destino).HasMaxLength(100);

                entity.Property(e => e.Puerta)
                    .HasMaxLength(3)
                    .IsFixedLength();

                entity.Property(e => e.Status).HasMaxLength(100);

                entity.Property(e => e.Tiempo).HasColumnType("datetime(1)");

                entity.Property(e => e.Vuelo).HasMaxLength(60);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
