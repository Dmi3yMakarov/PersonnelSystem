using PersonnelSystem.AppLayer.DataViewModels;
using PersonnelSystem.AppLayer.Interfaces;
using PersonnelSystem.AppLayer.RequestModels;
using PersonnelSystem.DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonnelSystem.AppLayer.Handlers
{
    public class CreateDepartmentHandler
    {
        private readonly IDepartmentQuerySource _departmentQuerySource;
        private readonly IDepartmentHistoryQuerySource _departmentHistoryQuerySource;
        public CreateDepartmentHandler(IDepartmentQuerySource departmentQuerySource, IDepartmentHistoryQuerySource departmentHistoryQuerySource)
        {
            _departmentQuerySource = departmentQuerySource;
            _departmentHistoryQuerySource= departmentHistoryQuerySource;
        }

        public void Execute(CreateDepartmentRequestModel department)
        {
            try
            {
                DepartmentId parentDepartmentId = null;
                if (department.ParentDepartmentId != null)
                {
                    parentDepartmentId = DepartmentId.FromString(department.ParentDepartmentId);

                    if (_departmentQuerySource.GetById(parentDepartmentId) == null)
                        throw new ArgumentException("Не найдено родительское подразделение");
                }                

                if (_departmentQuerySource.GetAll().Any(x => x.Name == department.DepartmentName))                
                    throw new ArgumentException("Подразделение с таким наименованием уже существует!");
                
                DepartmentId depId = DepartmentId.CreateNew();
                Department dep = new Department(depId, department.DepartmentName, parentDepartmentId);
                _departmentQuerySource.Create(dep);

                DepartmentHistoryDataViewModel departmentHistory = new DepartmentHistoryDataViewModel
                {
                    DepartmentId = depId.ToString(),
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
