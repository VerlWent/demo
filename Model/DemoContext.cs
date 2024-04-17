using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace demo2024.Model;

public partial class DemoContext : DbContext
{
    public DemoContext()
    {
    }

    public DemoContext(DbContextOptions<DemoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<RequestStatusT> RequestStatusTs { get; set; }

    public virtual DbSet<RequestT> RequestTs { get; set; }

    public virtual DbSet<ResponsibleT> ResponsibleTs { get; set; }

    public virtual DbSet<RoleT> RoleTs { get; set; }

    public virtual DbSet<StatusChangeT> StatusChangeTs { get; set; }

    public virtual DbSet<TypeFailT> TypeFailTs { get; set; }

    public virtual DbSet<UserT> UserTs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=chloe700A;database=demo", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.30-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<RequestStatusT>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("request_status_t");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.StatusName).HasMaxLength(45);
        });

        modelBuilder.Entity<RequestT>(entity =>
        {
            entity.HasKey(e => e.Number).HasName("PRIMARY");

            entity.ToTable("request_t");

            entity.HasIndex(e => e.TypeFail, "Request/TypeFailId_FK_idx");

            entity.Property(e => e.ClientFio)
                .HasMaxLength(100)
                .HasColumnName("ClientFIO");
            entity.Property(e => e.DateAdded).HasColumnType("datetime");
            entity.Property(e => e.DescriptionFail).HasMaxLength(255);
            entity.Property(e => e.DeviceName).HasMaxLength(45);

            entity.HasOne(d => d.TypeFailNavigation).WithMany(p => p.RequestTs)
                .HasForeignKey(d => d.TypeFail)
                .HasConstraintName("Request/TypeFailId_FK");
        });

        modelBuilder.Entity<ResponsibleT>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("responsible_t");

            entity.HasIndex(e => e.RequestId, "Responsible/RequestId_FK_idx");

            entity.HasIndex(e => e.UserId, "Responsible/UserId_FK_idx");

            entity.HasOne(d => d.Request).WithMany(p => p.ResponsibleTs)
                .HasForeignKey(d => d.RequestId)
                .HasConstraintName("Responsible/RequestId_FK");

            entity.HasOne(d => d.User).WithMany(p => p.ResponsibleTs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Responsible/UserId_FK");
        });

        modelBuilder.Entity<RoleT>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("role_t");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.RoleName).HasMaxLength(45);
        });

        modelBuilder.Entity<StatusChangeT>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("status_change_t");

            entity.HasIndex(e => e.RequestId, "Status/RequestId_FK_idx");

            entity.HasIndex(e => e.RequestStatus, "Status/RequestStatusId_FK_idx");

            entity.Property(e => e.ChangeDate).HasColumnType("datetime");
            entity.Property(e => e.Comment).HasMaxLength(255);

            entity.HasOne(d => d.Request).WithMany(p => p.StatusChangeTs)
                .HasForeignKey(d => d.RequestId)
                .HasConstraintName("Status/RequestId_FK");

            entity.HasOne(d => d.RequestStatusNavigation).WithMany(p => p.StatusChangeTs)
                .HasForeignKey(d => d.RequestStatus)
                .HasConstraintName("Status/RequestStatusId_FK");
        });

        modelBuilder.Entity<TypeFailT>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("type_fail_t");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FailName).HasMaxLength(45);
        });

        modelBuilder.Entity<UserT>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user_t");

            entity.HasIndex(e => e.Role, "RoleId_FK_idx");

            entity.Property(e => e.Login).HasMaxLength(45);
            entity.Property(e => e.Name).HasMaxLength(45);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Patronomic).HasMaxLength(45);
            entity.Property(e => e.Surname).HasMaxLength(45);

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.UserTs)
                .HasForeignKey(d => d.Role)
                .HasConstraintName("RoleId_FK");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
