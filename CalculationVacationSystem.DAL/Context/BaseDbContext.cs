using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CalculationVacationSystem.DAL.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CalculationVacationSystem.DAL.Context
{
    public partial class BaseDbContext : DbContext
    {
        public BaseDbContext()
        {
        }

        public BaseDbContext(DbContextOptions<BaseDbContext> options)
            : base(options)  {
        }

        public virtual DbSet<Auth> Auths { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeRight> EmployeeRights { get; set; }
        public virtual DbSet<RequestStatus> RequestStatuses { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SpecialRight> SpecialRights { get; set; }
        public virtual DbSet<StructureUnit> StructureUnits { get; set; }
        public virtual DbSet<VacationRequest> VacationRequests { get; set; }
        public virtual DbSet<VacationType> VacationTypes { get; set; }
        public virtual DbSet<VacationTypeSpecialRight> VacationTypeSpecialRights { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.utf8");

            modelBuilder.Entity<Auth>(entity =>
            {
                entity.HasKey(e => e.EmployeeId)
                    .HasName("auth_pkey");

                entity.ToTable("auth");

                entity.HasIndex(e => e.Username, "authentification_index");

                entity.HasIndex(e => e.Username, "username_unique")
                    .IsUnique();

                entity.Property(e => e.EmployeeId)
                    .ValueGeneratedNever()
                    .HasColumnName("employee_id");

                entity.Property(e => e.Passhash)
                    .IsRequired()
                    .HasColumnName("passhash");

                entity.Property(e => e.Role)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("role");

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasColumnName("salt");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("username");

                entity.HasOne(d => d.Employee)
                    .WithOne(p => p.Auth)
                    .HasForeignKey<Auth>(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("employee_id_fkey");

                entity.HasOne(d => d.RoleNavigation)
                    .WithMany(p => p.Auths)
                    .HasForeignKey(d => d.Role)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("role_id");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("employee");

                entity.HasIndex(e => e.Id, "employee_id_index");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.EmploymentDate)
                    .HasColumnType("date")
                    .HasColumnName("employment_date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("last_name");

                entity.Property(e => e.PersonalPhone)
                    .IsRequired()
                    .HasMaxLength(12)
                    .HasColumnName("personal_phone");

                entity.Property(e => e.Position)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("position");

                entity.Property(e => e.SecondName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("second_name");

                entity.Property(e => e.StructureId).HasColumnName("structure_id");

                entity.Property(e => e.WorkPhone)
                    .IsRequired()
                    .HasMaxLength(12)
                    .HasColumnName("work_phone");

                entity.HasOne(d => d.Structure)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.StructureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("structure_unit_id_fkey");
            });

            modelBuilder.Entity<EmployeeRight>(entity =>
            {
                entity.HasKey(e => new { e.EmployeeId, e.SpecialRightId })
                    .HasName("employee_rights_pkey");

                entity.ToTable("employee_rights");

                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

                entity.Property(e => e.SpecialRightId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("special_right_id");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeRights)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("employee_id_fkey");

                entity.HasOne(d => d.SpecialRight)
                    .WithMany(p => p.EmployeeRights)
                    .HasForeignKey(d => d.SpecialRightId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("specieal_right_id_fkey");
            });

            modelBuilder.Entity<RequestStatus>(entity =>
            {
                entity.ToTable("request_status");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<SpecialRight>(entity =>
            {
                entity.ToTable("special_rights");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<StructureUnit>(entity =>
            {
                entity.ToTable("structure_unit");

                entity.HasIndex(e => e.Code, "structure_unit_code_key")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("address");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("code");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.ParentId).HasColumnName("parent_id");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("structure_unit_parent_id_fkey");
            });

            modelBuilder.Entity<VacationType>(entity =>
            {
                entity.ToTable("vacation_type");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<VacationRequest>(entity =>
            {
                entity.ToTable("vacation_request");

                entity.HasIndex(e => e.Id, "request_index");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.DateChanged)
                    .HasColumnType("date")
                    .HasColumnName("date_changed");

                entity.Property(e => e.DateStart)
                    .HasColumnType("date")
                    .HasColumnName("date_start");

                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

                entity.Property(e => e.EmployerId).HasColumnName("employer_id");

                entity.Property(e => e.Period).HasColumnName("period");

                entity.Property(e => e.Reason).HasColumnName("reason");

                entity.Property(e => e.StatusId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("status_id");

                entity.Property(e => e.TypeId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("type_id");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.VacationRequestEmployees)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("employee_id_fkey");

                entity.HasOne(d => d.Employer)
                    .WithMany(p => p.VacationRequestEmployers)
                    .HasForeignKey(d => d.EmployerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("employer_id_fkey");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.VacationRequests)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("request_status_id_fkey");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.VacationRequests)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("request_type_id_fkey");
            });

            modelBuilder.Entity<VacationTypeSpecialRight>(entity =>
            {
                entity.HasKey(e => new { e.TypeId, e.SpecialRightId })
                    .HasName("vacation_type_special_rights_pkey");

                entity.ToTable("vacation_type_special_rights");

                entity.Property(e => e.TypeId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("type_id");

                entity.Property(e => e.SpecialRightId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("special_right_id");

                entity.Property(e => e.IsNeedSign).HasColumnName("is_need_sign");

                entity.Property(e => e.MaxPeriod).HasColumnName("max_period");

                entity.HasOne(d => d.SpecialRight)
                    .WithMany(p => p.VacationTypeSpecialRights)
                    .HasForeignKey(d => d.SpecialRightId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("vacation_type_special_rights_special_right_id_fkey");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.VacationTypeSpecialRights)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("vacation_type_special_rights_type_id_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
