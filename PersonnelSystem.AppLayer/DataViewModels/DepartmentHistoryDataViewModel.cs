using System;

namespace PersonnelSystem.AppLayer.DataViewModels
{
    /// <summary>
    /// Модель-представление сущности Подразделение
    /// содержит все поля Department и дату изменения
    /// </summary>
    public record DepartmentHistoryDataViewModel
    {
        public int Id { get; set; }
        public string DepartmentId { get; init; }
        public string Name { get; init; }        
        public string? ParentDepartmentId { get; init; }
        public bool IsDeleted { get; init; }
        public DateTime ChangeDate { get; init; }
    }
}


