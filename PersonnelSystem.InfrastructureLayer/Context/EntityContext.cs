using Microsoft.EntityFrameworkCore;
using PersonnelSystem.AppLayer.DataViewModels;
using PersonnelSystem.DomainLayer.Models;

namespace PersonnelSystem.InfrastructureLayer
{
    public class EntityContext : DbContext
    {
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<EmployeeHistoryDataViewModel> EmployeesHistory => Set<EmployeeHistoryDataViewModel>();
        public DbSet<DepartmentHistoryDataViewModel> DepartmentsHistory => Set<DepartmentHistoryDataViewModel>();

        public EntityContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=personnelSystem8;Trusted_Connection=True;").UseSnakeCaseNamingConvention();
            //optionsBuilder.UseSqlServer(@"Server=BERSERK\SQLEXPRESS;Database=test;Trusted_Connection=True;").UseSnakeCaseNamingConvention();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Используем схему. Применяем все реализации IEntityTypeConfiguration<> в сборке.
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EntityContext).Assembly);

        }
    }
}

