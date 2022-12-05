using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonnelSystem.DomainLayer.Models;
using PersonnelSystem.InfrastructureLayer.Context.ValueConverters;


namespace PersonnelSystem.InfrastructureLayer.Context.EntityTypeConfiguration
{
    internal class DepartmentEntityTypeConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            // Сущность "хранится" в таблице.
            builder.ToTable("departments");
            // PK будет считаться в этом поле.
            builder.HasKey(a => a.Id);
            // Говорим, что данное свойство сущности конвертируется
            // особым образом
            builder.Property(a => a.Id)
                .HasConversion(new DepartmentIdToStringValueConverter());

            builder.HasOne<Department>()
                   .WithMany()
                   .HasForeignKey(a => a.ParentDepartmentId)
                   .OnDelete(DeleteBehavior.NoAction)
                   .HasConstraintName("fk_departments_parent_department_id_departments_id");
            builder.Property(a => a.ParentDepartmentId)
                   .HasConversion(new DepartmentIdToStringValueConverter());
        }
    }

}
