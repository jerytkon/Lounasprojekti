using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LounasprojektiLib.Models
{
    public partial class LounasDBContext : DbContext
    {
        public LounasDBContext()
        {
        }

        public LounasDBContext(DbContextOptions<LounasDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Arvio> Arvios { get; set; } = null!;
        public virtual DbSet<Käyttäjä> Käyttäjäs { get; set; } = null!;
        public virtual DbSet<Lounasseura> Lounasseuras { get; set; } = null!;
        public virtual DbSet<Lounastapahtuma> Lounastapahtumas { get; set; } = null!;
        public virtual DbSet<Ravintola> Ravintolas { get; set; } = null!;
        public virtual DbSet<VSyömäänRekisteröityneet> VSyömäänRekisteröityneets { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server = lunchapp.database.windows.net, 1433; Database = LounasDB; User ID = olenadmin; Password = Salasana123; Trusted_Connection = False; Encrypt = True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Arvio>(entity =>
            {
                entity.ToTable("Arvio");

                entity.Property(e => e.ArvioId).HasColumnName("ArvioID");

                entity.Property(e => e.Kommentti)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.KäyttäjäId).HasColumnName("KäyttäjäID");

                entity.Property(e => e.Päivämäärä).HasColumnType("datetime");

                entity.Property(e => e.RavintolaId).HasColumnName("RavintolaID");

                entity.HasOne(d => d.Käyttäjä)
                    .WithMany(p => p.Arvios)
                    .HasForeignKey(d => d.KäyttäjäId)
                    .HasConstraintName("FK__Arvio__KäyttäjäI__2A4B4B5E");

                entity.HasOne(d => d.Ravintola)
                    .WithMany(p => p.Arvios)
                    .HasForeignKey(d => d.RavintolaId)
                    .HasConstraintName("FK__Arvio__Ravintola__29572725");
            });

            modelBuilder.Entity<Käyttäjä>(entity =>
            {
                entity.ToTable("Käyttäjä");

                entity.Property(e => e.KäyttäjäId).HasColumnName("KäyttäjäID");

                entity.Property(e => e.Käyttäjänimi)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OnAdmin).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<Lounasseura>(entity =>
            {
                entity.HasKey(e => e.SeuraId)
                    .HasName("PK__Lounasse__2579DBD0623A8B06");

                entity.ToTable("Lounasseura");

                entity.Property(e => e.SeuraId).HasColumnName("SeuraID");

                entity.Property(e => e.KäyttäjäId).HasColumnName("KäyttäjäID");

                entity.Property(e => e.LounastapahtumaId).HasColumnName("LounastapahtumaID");

                entity.HasOne(d => d.Käyttäjä)
                    .WithMany(p => p.Lounasseuras)
                    .HasForeignKey(d => d.KäyttäjäId)
                    .HasConstraintName("FK__Lounasseu__Käytt__30F848ED");

                entity.HasOne(d => d.Lounastapahtuma)
                    .WithMany(p => p.Lounasseuras)
                    .HasForeignKey(d => d.LounastapahtumaId)
                    .HasConstraintName("FK__Lounasseu__Louna__300424B4");
            });

            modelBuilder.Entity<Lounastapahtuma>(entity =>
            {
                entity.ToTable("Lounastapahtuma");

                entity.Property(e => e.LounastapahtumaId).HasColumnName("LounastapahtumaID");

                entity.Property(e => e.Päivämäärä).HasColumnType("datetime");

                entity.Property(e => e.RavintolaId).HasColumnName("RavintolaID");

                entity.HasOne(d => d.Ravintola)
                    .WithMany(p => p.Lounastapahtumas)
                    .HasForeignKey(d => d.RavintolaId)
                    .HasConstraintName("FK__Lounastap__Ravin__2D27B809");
            });

            modelBuilder.Entity<Ravintola>(entity =>
            {
                entity.ToTable("Ravintola");

                entity.Property(e => e.RavintolaId).HasColumnName("RavintolaID");

                entity.Property(e => e.Kategoria)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.MenuUrl)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Osoite)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Postinumero)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("postinumero")
                    .IsFixedLength();

                entity.Property(e => e.Postitoimipaikka)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("postitoimipaikka");

                entity.Property(e => e.RavintolanNimi)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Verkkosivu)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VSyömäänRekisteröityneet>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vSyömäänRekisteröityneet");

                entity.Property(e => e.Päivämäärä).HasColumnType("date");

                entity.Property(e => e.RavintolanNimi)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
