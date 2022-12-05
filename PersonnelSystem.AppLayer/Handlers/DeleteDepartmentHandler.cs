using PersonnelSystem.AppLayer.DataViewModels;
using PersonnelSystem.AppLayer.Interfaces;
using PersonnelSystem.AppLayer.RequestModels;
using PersonnelSystem.DomainLayer.Models;
using System;

namespace PersonnelSystem.AppLayer.Handlers
{
    public class DeleteDepartmentHandler
    {
        private readonly IDepartmentQuerySource _departmentQuerySource;
        private readonly IDepartmentHistoryQuerySource _departmentHistoryQuerySource;
        public DeleteDepartmentHandler(IDepartmentQuerySource departmentQuerySource, IDepartmentHistoryQuerySource departmentHistoryQuerySource)
        {
            _departmentQuerySource = departmentQuerySource;
            _departmentHistoryQuerySource = departmentHistoryQuerySource;
        }

        public void Execute(DeleteDepartmentRequestModel department)
        {
            try
            {
                DepartmentId depId = DepartmentId.FromString(department.Id);
                var dep = _departmentQuerySource.GetById(depId);
                if (dep == null)
                    throw new NullReferenceException("Не найдено подразделение");
                                
                if (_departmentQuerySource.HasChildren(depId))
                    throw new ArgumentException("Удалять можно только подразделения нижнего звена!");

                _departmentQuerySource.Delete(depId);

                DepartmentHistoryDataViewModel departmentHistory = new DepartmentHistoryDataViewModel
                {
                    DepartmentId = depId.ToString(),
                    Name = dep.Name,
                    ParentDepartmentId = dep.ParentDepartmentId?.ToString(),
                    IsDeleted = true,
                    ChangeDate = DateTime.Now
                };
                _departmentHistoryQuerySource.AddDepartmentToHistory(departmentHistory);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
