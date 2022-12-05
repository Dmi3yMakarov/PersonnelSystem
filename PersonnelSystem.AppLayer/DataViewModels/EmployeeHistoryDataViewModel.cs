using System;

namespace PersonnelSystem.AppLayer.DataViewModels
{
    /// <summary>
    /// Модель-представление сущности Сотрудник
    /// содержит все поля Employee и дату изменения
    /// </summary>
    public record EmployeeHistoryDataViewModel
    {
        public int Id { get; init; }
        public string EmployeeId { get; init; }
        public string FullName { get; init; }
        public string DepartmentId { get; init; }
        public bool IsDeleted { get; init; }
        public DateTime ChangeDate { get; init; }

        public override string ToString()
        {
            return FullName;
        }
    }
}

