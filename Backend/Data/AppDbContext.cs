using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeSalary> EmployeeSalaries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Department Configuration
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Description).HasMaxLength(200);

                // SARGABLE Indexes for filtering and sorting
                entity.HasIndex(e => e.Code).IsUnique();
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.IsDeleted);
            });

            // Designation Configuration
            modelBuilder.Entity<Designation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Description).HasMaxLength(200);
                entity.Property(e => e.BaseSalaryRange).HasPrecision(18, 2);

                // Foreign Key
                entity.HasOne(e => e.Department)
                    .WithMany(d => d.Designations)
                    .HasForeignKey(e => e.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                // SARGABLE Indexes for filtering - order matters for composite queries
                entity.HasIndex(e => e.Code).IsUnique();
                entity.HasIndex(e => e.DepartmentId);
                entity.HasIndex(e => new { e.DepartmentId, e.IsDeleted });
                entity.HasIndex(e => e.IsDeleted);
            });

            // Employee Configuration
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);

                // Foreign Keys
                entity.HasOne(e => e.Department)
                    .WithMany(d => d.Employees)
                    .HasForeignKey(e => e.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Designation)
                    .WithMany(d => d.Employees)
                    .HasForeignKey(e => e.DesignationId)
                    .OnDelete(DeleteBehavior.Restrict);

                // SARGABLE Indexes for filtering - order matters for composite queries
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => new { e.DepartmentId, e.IsDeleted });
                entity.HasIndex(e => new { e.DesignationId, e.IsDeleted });
                entity.HasIndex(e => e.IsDeleted);
            });

            // EmployeeSalary Configuration
            modelBuilder.Entity<EmployeeSalary>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.BaseSalary).HasPrecision(18, 2);
                entity.Property(e => e.HRA).HasPrecision(18, 2);
                entity.Property(e => e.DA).HasPrecision(18, 2);
                entity.Property(e => e.Allowances).HasPrecision(18, 2);

                // Foreign Key
                entity.HasOne(e => e.Employee)
                    .WithMany(emp => emp.EmployeeSalaries)
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);

                // SARGABLE Indexes for filtering
                entity.HasIndex(e => new { e.EmployeeId, e.IsDeleted });
                entity.HasIndex(e => e.IsDeleted);
            });

            // Data Seeding
            SeedDepartments(modelBuilder);
            SeedDesignations(modelBuilder);
            SeedEmployees(modelBuilder);
            SeedEmployeeSalaries(modelBuilder);
        }

        private void SeedDepartments(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "Information Technology", Code = "IT", Description = "IT Department", IsDeleted = false },
                new Department { Id = 2, Name = "Human Resources", Code = "HR", Description = "HR Department", IsDeleted = false },
                new Department { Id = 3, Name = "Finance", Code = "FINANCE", Description = "Finance Department", IsDeleted = false },
                new Department { Id = 4, Name = "Sales", Code = "SALES", Description = "Sales Department", IsDeleted = false },
                new Department { Id = 5, Name = "Operations", Code = "OPS", Description = "Operations Department", IsDeleted = false }
            );
        }

        private void SeedDesignations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Designation>().HasData(
                new Designation { Id = 1, Title = "Software Engineer", Code = "SE", Description = "Software Development Role", DepartmentId = 1, BaseSalaryRange = 50000, IsDeleted = false },
                new Designation { Id = 2, Title = "Senior Manager", Code = "SM", Description = "Senior Management Role", DepartmentId = 1, BaseSalaryRange = 80000, IsDeleted = false },
                new Designation { Id = 3, Title = "HR Manager", Code = "HRM", Description = "HR Management Role", DepartmentId = 2, BaseSalaryRange = 60000, IsDeleted = false },
                new Designation { Id = 4, Title = "Finance Manager", Code = "FM", Description = "Finance Management Role", DepartmentId = 3, BaseSalaryRange = 70000, IsDeleted = false },
                new Designation { Id = 5, Title = "Sales Executive", Code = "SET", Description = "Sales Role", DepartmentId = 4, BaseSalaryRange = 55000, IsDeleted = false }
            );
        }

        private void SeedEmployees(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, FirstName = "John", LastName = "Smith", Email = "john.smith@company.com", DepartmentId = 1, DesignationId = 1, CreatedAt = new DateTime(2026, 01, 15, 10, 30, 0, DateTimeKind.Utc), IsDeleted = false },
                new Employee { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@company.com", DepartmentId = 2, DesignationId = 3, CreatedAt = new DateTime(2026, 02, 20, 14, 45, 0, DateTimeKind.Utc), IsDeleted = false },
                new Employee { Id = 3, FirstName = "Robert", LastName = "Johnson", Email = "robert.johnson@company.com", DepartmentId = 3, DesignationId = 4, CreatedAt = new DateTime(2025, 03, 10, 09, 15, 0, DateTimeKind.Utc), IsDeleted = false },
                new Employee { Id = 4, FirstName = "Michael", LastName = "Brown", Email = "michael.brown@company.com", DepartmentId = 4, DesignationId = 5, CreatedAt = new DateTime(2025, 04, 05, 16, 20, 0, DateTimeKind.Utc), IsDeleted = false },
                new Employee { Id = 5, FirstName = "Sarah", LastName = "Wilson", Email = "sarah.wilson@company.com", DepartmentId = 1, DesignationId = 2, CreatedAt = new DateTime(2025, 05, 12, 11, 00, 0, DateTimeKind.Utc), IsDeleted = false }
            );
        }

        private void SeedEmployeeSalaries(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeSalary>().HasData(
                new EmployeeSalary { Id = 1, EmployeeId = 1, BaseSalary = 50000, HRA = 10000, DA = 5000, Allowances = 2000, IsDeleted = false },
                new EmployeeSalary { Id = 2, EmployeeId = 2, BaseSalary = 60000, HRA = 12000, DA = 6000, Allowances = 2500, IsDeleted = false },
                new EmployeeSalary { Id = 3, EmployeeId = 3, BaseSalary = 70000, HRA = 14000, DA = 7000, Allowances = 3000, IsDeleted = false },
                new EmployeeSalary { Id = 4, EmployeeId = 4, BaseSalary = 55000, HRA = 11000, DA = 5500, Allowances = 2200, IsDeleted = false },
                new EmployeeSalary { Id = 5, EmployeeId = 5, BaseSalary = 80000, HRA = 16000, DA = 8000, Allowances = 3500, IsDeleted = false }
            );
        }
    }
}
