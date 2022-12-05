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
    public class DeleteEmployeeHandler
    {
        private readonly IEmployeeQuerySource _employeeQuerySource;        
        private readonly IEmployeeHistoryQuerySource _employeeHistoryQuerySource;
        public DeleteEmployeeHandler(IEmployeeQuerySource employeeQuerySource, IEmployeeHistoryQuerySource employeeHistoryQuerySource)
        {
            _employeeQuerySource = employeeQuerySource;            
            _employeeHistoryQuerySource = employeeHistoryQuerySource;
        }

        public void Execute(DeleteEmployeeRequestModel employee)
        {
            try
            {
                EmployeeId empId = EmployeeId.FromString(employee.Id);
                var emp = _employeeQuerySource.GetById(empId);
                if (emp == null)
                    throw new NullReferenceException(nameof(emp));

                _employeeQuerySource.Delete(empId);

                EmployeeHistoryDataViewModel employeeHistory = new EmployeeHistoryDataViewModel
                {
                    EmployeeId = empId.ToString(),
                    FullName = emp.FullName,
                    DepartmentId = emp.DepartmentId.ToString(),
                    IsDeleted = true,
                    ChangeDate = DateTime.Now
                };
                _employeeHistoryQuerySource.AddEmployeeToHistory(employeeHistory);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
