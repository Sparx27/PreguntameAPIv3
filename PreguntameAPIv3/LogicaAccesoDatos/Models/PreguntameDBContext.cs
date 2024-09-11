using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PreguntameAPIv3.LogicaAccesoDatos.Models;

public partial class PreguntameDbContext : DbContext
{
    public PreguntameDbContext(DbContextOptions<PreguntameDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Like> Likes { get; set; }

    public virtual DbSet<Paise> Paises { get; set; }

    public virtual DbSet<Pregunta> Preguntas { get; set; }

    public virtual DbSet<Respuesta> Respuestas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasKey(e => new { e.IdRespuesta, e.IdUsuarioEnvia, e.UsernameUsuarioRecibe }).HasName("pk_likes");

            entity.HasIndex(e => e.IdRespuesta, "idx_RespuestaLike");

            entity.HasIndex(e => e.IdUsuarioEnvia, "idx_UsuarioEnviaLike");

            entity.HasIndex(e => e.UsernameUsuarioRecibe, "idx_UsuarioRecibeLike");

            entity.Property(e => e.IdRespuesta).HasColumnName("ID_Respuesta");
            entity.Property(e => e.IdUsuarioEnvia).HasColumnName("ID_Usuario_Envia");
            entity.Property(e => e.UsernameUsuarioRecibe)
                .HasMaxLength(20)
                .HasColumnName("Username_Usuario_Recibe");
            entity.Property(e => e.Existe).HasDefaultValue(true);

            entity.HasOne(d => d.IdRespuestaNavigation).WithMany(p => p.Likes)
                .HasForeignKey(d => d.IdRespuesta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_respuesta");

            entity.HasOne(d => d.IdUsuarioEnviaNavigation).WithMany(p => p.LikeIdUsuarioEnviaNavigations)
                .HasForeignKey(d => d.IdUsuarioEnvia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Usuario_Envia");

            entity.HasOne(d => d.UsernameUsuarioRecibeNavigation).WithMany(p => p.LikeUsernameUsuarioRecibeNavigations)
                .HasPrincipalKey(p => p.Username)
                .HasForeignKey(d => d.UsernameUsuarioRecibe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Usuario_Recibe");
        });

        modelBuilder.Entity<Paise>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Paises__3214EC2744F7CD65");

            entity.HasIndex(e => e.Nombre, "UQ__Paises__75E3EFCF03509B14").IsUnique();

            entity.HasIndex(e => e.Abr, "UQ__Paises__C69676F085B102F3").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newsequentialid())")
                .HasColumnName("ID");
            entity.Property(e => e.Abr).HasMaxLength(2);
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Pregunta>(entity =>
        {
            entity.HasIndex(e => e.UsuarioEnvia, "IX_Usuario_Envia");

            entity.HasIndex(e => e.UsuarioRecibe, "IX_Usuario_Recibe");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newsequentialid())")
                .HasColumnName("ID");
            entity.Property(e => e.Anonima).HasDefaultValue(true);
            entity.Property(e => e.Dsc).HasMaxLength(300);
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UsuarioEnvia)
                .HasMaxLength(20)
                .HasColumnName("Usuario_Envia");
            entity.Property(e => e.UsuarioRecibe)
                .HasMaxLength(20)
                .HasColumnName("Usuario_Recibe");

            entity.HasOne(d => d.UsuarioEnviaNavigation).WithMany(p => p.PreguntaUsuarioEnviaNavigations)
                .HasPrincipalKey(p => p.Username)
                .HasForeignKey(d => d.UsuarioEnvia)
                .HasConstraintName("FK_UsuarioEnvia");

            entity.HasOne(d => d.UsuarioRecibeNavigation).WithMany(p => p.PreguntaUsuarioRecibeNavigations)
                .HasPrincipalKey(p => p.Username)
                .HasForeignKey(d => d.UsuarioRecibe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioRecibe");
        });

        modelBuilder.Entity<Respuesta>(entity =>
        {
            entity.HasIndex(e => e.PreguntaId, "IX_PreguntaID");

            entity.HasIndex(e => e.PreguntaId, "UQ_PreguntaIDRespuestas").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newsequentialid())")
                .HasColumnName("ID");
            entity.Property(e => e.Dsc).HasMaxLength(300);
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NLikes).HasColumnName("n_likes");
            entity.Property(e => e.PreguntaId).HasColumnName("Pregunta_ID");

            entity.HasOne(d => d.Pregunta).WithOne(p => p.Respuesta)
                .HasForeignKey<Respuesta>(d => d.PreguntaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PreguntaID");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasIndex(e => e.Email, "UQ_Email").IsUnique();

            entity.HasIndex(e => e.Username, "UQ_Username").IsUnique();

            entity.HasIndex(e => e.Apellido, "idx_ApellidoUsuario");

            entity.HasIndex(e => e.Nombre, "idx_NombreUsuario");

            entity.HasIndex(e => e.PaisId, "idx_PaisUsuario");

            entity.HasIndex(e => e.Username, "idx_UsernameUsuario");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newsequentialid())")
                .HasColumnName("ID");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Apellido).HasMaxLength(40);
            entity.Property(e => e.Bio)
                .HasMaxLength(150)
                .HasColumnName("bio");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FondoPath)
                .HasMaxLength(255)
                .HasColumnName("Fondo_Path");
            entity.Property(e => e.FotoPath)
                .HasMaxLength(255)
                .HasColumnName("Foto_Path");
            entity.Property(e => e.NLikes).HasColumnName("n_likes");
            entity.Property(e => e.NSeguidores).HasColumnName("n_seguidores");
            entity.Property(e => e.Nombre).HasMaxLength(30);
            entity.Property(e => e.PaisId).HasColumnName("Pais_ID");
            entity.Property(e => e.PreguntaBio)
                .HasMaxLength(40)
                .HasColumnName("pregunta_bio");
            entity.Property(e => e.UPassword)
                .HasMaxLength(65)
                .HasColumnName("U_Password");
            entity.Property(e => e.Username).HasMaxLength(20);

            entity.HasOne(d => d.Pais).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.PaisId)
                .HasConstraintName("FK_PaisUsuarios");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
