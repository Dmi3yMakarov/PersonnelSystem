using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonnelSystem.DomainLayer.Models;
using PersonnelSystem.InfrastructureLayer.Context.ValueConverters;


namespace PersonnelSystem.InfrastructureLayer.Context.EntityTypeConfiguration
{
    internal class EmployeeEntityTypeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            // Сущность "хранится" в таблице.
            builder.ToTable("employees");
            // PK будет считаться в этом поле.
            builder.HasKey(a => a.Id);
            // Говорим, что данное свойство сущности конвертируется
            // особым образом
            builder.Property(a => a.Id)
                .HasConversion(new EmployeeIdToStringValueConverter());

            builder.Property(a => a.DepartmentId).HasConversion(new DepartmentIdToStringValueConverter());

            builder.Ignore(a => a.FullName);
        }
    }

}
