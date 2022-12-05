using PersonnelSystem.AppLayer.DataViewModels;
using PersonnelSystem.AppLayer.Interfaces;
using PersonnelSystem.AppLayer.RequestModels;
using PersonnelSystem.DomainLayer.Models;
using System;

namespace PersonnelSystem.AppLayer.Handlers
{
    public class EditDepartmentHandler
    {
        private readonly IDepartmentQuerySource _departmentQuerySource;
        private readonly IDepartmentHistoryQuerySource _departmentHistoryQuerySource;
        public EditDepartmentHandler(IDepartmentQuerySource departmentQuerySource, IDepartmentHistoryQuerySource departmentHistoryQuerySource)
        {
            _departmentQuerySource = departmentQuerySource;
            _departmentHistoryQuerySource = departmentHistoryQuerySource;
        }

        public void Execute(EditDepartmentRequestModel department)
        {
            try
            {
                DepartmentId departmentId = DepartmentId.FromString(department.Id);
                var editDepartment = _departmentQuerySource.GetById(departmentId);
                if (editDepartment == null)
                    throw new NullReferenceException("Не найдено подразделение");

                DepartmentId parentDepartmentId = department.ParentDepartmentId != null ?
                    DepartmentId.FromString(department.ParentDepartmentId) : null;
                
                if (parentDepartmentId != null && _departmentQuerySource.GetById(parentDepartmentId) == null)
                    throw new NullReferenceException("Не найдено родительское подразделение");

                Department dep = new Department(departmentId, department.DepartmentName, parentDepartmentId);
                _departmentQuerySource.Update(dep);

                DepartmentHistoryDataViewModel departmentHistory = new DepartmentHistoryDataViewModel
                {
                    DepartmentId = department.Id,
                    Name = department.DepartmentName,
                    ParentDepartmentId = department.ParentDepartmentId,
                    IsDeleted = false,
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
