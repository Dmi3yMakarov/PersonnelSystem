using PersonnelSystem.AppLayer.DataViewModels;
using PersonnelSystem.AppLayer.Interfaces;
using PersonnelSystem.AppLayer.RequestModels;
using System;
using System.Collections.ObjectModel;


namespace PersonnelSystem.AppLayer.Handlers
{
    public class GetEmployeeListForPeriodHandler
    {
        private readonly IEmployeeHistoryQuerySource _employeeHistoryQuerySource;
        public GetEmployeeListForPeriodHandler(IEmployeeHistoryQuerySource employeeHistoryQuerySource)
        {
            _employeeHistoryQuerySource = employeeHistoryQuerySource;
        }

        public EmployeeListDataViewModel Execute(DateEmployeeDepartmentRequestModel dateEmployeeDepartment)
        {
            try
            {
                ObservableCollection<EmployeeDataViewModel> employees = new();

                var employeeHistory = _employeeHistoryQuerySource.GetEmployeeHistoryByDateAndDepartment(dateEmployeeDepartment.FromDateTime,
                                                                                                        dateEmployeeDepartment.ToDateTime,
                                                                                                        dateEmployeeDepartment.DepartmentId);

                foreach (var empHistory in employeeHistory)
                    employees.Add(new EmployeeDataViewModel(empHistory.EmployeeId, empHistory.FullName, empHistory.DepartmentId));

                return new EmployeeListDataViewModel(employees);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
