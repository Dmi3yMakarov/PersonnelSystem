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
    public class CreateEmployeeHandler
    {
        private readonly IEmployeeQuerySource _employeeQuerySource;
        private readonly IDepartmentQuerySource _departmentQuerySource;
        private readonly IEmployeeHistoryQuerySource _employeeHistoryQuerySource;
        public CreateEmployeeHandler(IEmployeeQuerySource employeeQuerySource, IDepartmentQuerySource departmentQuerySource, IEmployeeHistoryQuerySource employeeHistoryQuerySource)
        {
            _employeeQuerySource = employeeQuerySource;
            _departmentQuerySource = departmentQuerySource;
            _employeeHistoryQuerySource = employeeHistoryQuerySource;
        }

        public void Execute(CreateEmployeeRequestModel employee)
        {
            try
            {
                DepartmentId departmentId = DepartmentId.FromString(employee.DepartmentId);
                var department = _departmentQuerySource.GetById(departmentId);
                if (department == null)
                    throw new NullReferenceException("Не найдено подразделение");

                EmployeeId empId = EmployeeId.CreateNew();
                Employee emp = new Employee(empId, employee.FirstName, employee.LastName, employee.Patronymic, departmentId);
                _employeeQuerySource.Create(emp);

                EmployeeHistoryDataViewModel employeeHistory = new EmployeeHistoryDataViewModel
                {
                    EmployeeId = empId.ToString(),
                    FullName = emp.FullName,
                    DepartmentId = employee.DepartmentId,
                    IsDeleted = false,
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
