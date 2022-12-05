using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonnelSystem.AppLayer.DataViewModels;

namespace PersonnelSystem.InfrastructureLayer.Context.EntityTypeConfiguration
{
    internal class EmployeeHistoryDataViewModelEntityTypeConfiguration : IEntityTypeConfiguration<EmployeeHistoryDataViewModel>
    {
        public void Configure(EntityTypeBuilder<EmployeeHistoryDataViewModel> builder)
        {
            builder.ToTable("employees_history_view_model");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.EmployeeId).HasColumnName("employee_id");
            builder.Property(a => a.FullName).HasColumnName("full_name");
            builder.Property(a => a.DepartmentId).HasColumnName("department_id");
            builder.Property(a => a.IsDeleted).HasColumnName("is_deleted");
            builder.Property(a => a.ChangeDate).HasColumnName("change_date");                        
        }
    }

}
