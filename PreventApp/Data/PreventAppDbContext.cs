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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>()
            .HasQueryFilter(a => !a.IsDeleted);

        modelBuilder.Entity<Accidente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Accident__3214EC07B0BA6FBC");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(280)
                .IsUnicode(false);
            entity.Property(e => e.Latitud).HasColumnType("numeric(20, 16)");
            entity.Property(e => e.Longitud).HasColumnType("numeric(20, 16)");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Accidentes)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Accidente__Categ__3E52440B");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Accidentes) 
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__Accidente__Usuar__3D5E1FD2");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07A506588A");

            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rol__3214EC079C97DC56");

            entity.ToTable("Rol");

            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuario__3214EC079C39524D");

            entity.ToTable("Usuario");

            entity.Property(e => e.Contraseña)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuario__RolId__3F466844");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
