using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonnelSystem.AppLayer.DataViewModels;

namespace PersonnelSystem.InfrastructureLayer.Context.EntityTypeConfiguration
{
    internal class DepartmentHistoryDataViewModelEntityTypeConfiguration : IEntityTypeConfiguration<DepartmentHistoryDataViewModel>
    {
        public void Configure(EntityTypeBuilder<DepartmentHistoryDataViewModel> builder)
        {
            builder.ToTable("departments_history_view_model");

            builder.HasKey(avm => avm.Id);
            builder.Property(a => a.Name).HasColumnName("name");
            builder.Property(a => a.DepartmentId).HasColumnName("department_id");
            builder.Property(a => a.ParentDepartmentId).HasColumnName("parent_department_id");
            builder.Property(a => a.IsDeleted).HasColumnName("is_deleted");
            builder.Property(a => a.ChangeDate).HasColumnName("change_date");
        }
    }

}
