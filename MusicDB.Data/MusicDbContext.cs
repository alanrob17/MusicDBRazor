using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MusicDB.Data.Entities;

namespace MusicDB.Data;

public partial class MusicDbContext : DbContext
{
    public MusicDbContext(DbContextOptions<MusicDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<Disc> Discs { get; set; }

    public virtual DbSet<Record> Records { get; set; }

    public virtual DbSet<Track> Tracks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.ToTable("Artist");

            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Folder).HasMaxLength(400);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.RecordArtistId).HasComment("This field matches the ArtistId in the RocordDB dtabase. I can use this to do releated queries between the databases.");
        });

        modelBuilder.Entity<Disc>(entity =>
        {
            entity.ToTable("Disc");

            entity.Property(e => e.Folder).HasMaxLength(400);
            entity.Property(e => e.Length).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.SubTitle).HasMaxLength(100);

            entity.HasOne(d => d.Record).WithMany(p => p.DiscsNavigation)
                .HasForeignKey(d => d.RecordId)
                .HasConstraintName("FK_Disc_Record");
        });

        modelBuilder.Entity<Record>(entity =>
        {
            entity.ToTable("Record");

            entity.Property(e => e.CoverName).HasMaxLength(400);
            entity.Property(e => e.Field).HasMaxLength(50);
            entity.Property(e => e.Folder).HasMaxLength(400);
            entity.Property(e => e.Length).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.SubTitle).HasMaxLength(100);

            entity.HasOne(d => d.Artist).WithMany(p => p.Records)
                .HasForeignKey(d => d.ArtistId)
                .HasConstraintName("FK_Record_Artist");
        });

        modelBuilder.Entity<Track>(entity =>
        {
            entity.ToTable("Track");

            entity.Property(e => e.Album).HasMaxLength(200);
            entity.Property(e => e.Artist).HasMaxLength(600);
            entity.Property(e => e.Field).HasMaxLength(100);
            entity.Property(e => e.Folder).HasMaxLength(400);
            entity.Property(e => e.Length).HasMaxLength(50);
            entity.Property(e => e.Media).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(300);

            entity.HasOne(d => d.Disc).WithMany(p => p.Tracks)
                .HasForeignKey(d => d.DiscId)
                .HasConstraintName("FK_Track_Disc");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
