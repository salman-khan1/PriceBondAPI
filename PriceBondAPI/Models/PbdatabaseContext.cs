using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PriceBondAPI.Models;

public partial class PbdatabaseContext : DbContext
{
    public PbdatabaseContext()
    {
    }

    public PbdatabaseContext(DbContextOptions<PbdatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bond> Bonds { get; set; }

    public virtual DbSet<Denomination> Denominations { get; set; }

    public virtual DbSet<Draw> Draws { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-V98AAC0;Database=pbdatabase;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bond>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Bonds__3214EC074C6230A7");

            entity.Property(e => e.PurchaseDate).HasColumnType("datetime");

            entity.HasOne(d => d.Denomination).WithMany(p => p.Bonds)
                .HasForeignKey(d => d.DenominationId)
                .HasConstraintName("FK__Bonds__Denominat__6477ECF3");

            entity.HasOne(d => d.User).WithMany(p => p.Bonds)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Bonds__UserId__6383C8BA");
        });

        modelBuilder.Entity<Denomination>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Denomina__3214EC0776E44866");
        });

        modelBuilder.Entity<Draw>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Draws__3214EC075EFEFA88");

            entity.Property(e => e.DrawDate).HasColumnType("datetime");

            entity.HasOne(d => d.Denomination).WithMany(p => p.Draws)
                .HasForeignKey(d => d.DenominationId)
                .HasConstraintName("FK__Draws__Denominat__60A75C0F");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07CC34BFBA");

            entity.Property(e => e.RegistrationDate).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
