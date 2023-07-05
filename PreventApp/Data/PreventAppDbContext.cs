using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PreventApp.Models;

namespace PreventApp.Data;

public partial class PreventAppDbContext : DbContext
{
    public PreventAppDbContext()
    {
    }

    public PreventAppDbContext(DbContextOptions<PreventAppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Accidente> Accidentes { get; set; }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:Database");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Accidente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Accident__3214EC07A17E7B7F");

            entity.Property(e => e.Fecha)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.Categoria).WithMany(p => p.Accidentes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Accidente__Categ__440B1D61");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Accidentes).HasConstraintName("FK__Accidente__Usuar__4316F928");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC0740F8E8CA");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rol__3214EC07BF5048A0");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuario__3214EC07826F9062");

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuario__RolId__398D8EEE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
