using PersonnelSystem.AppLayer.DataViewModels;
using System;
using System.Collections.Generic;

namespace PersonnelSystem.AppLayer.Interfaces
{
    public interface IEmployeeHistoryQuerySource
    {        
        public void AddEmployeeToHistory(EmployeeHistoryDataViewModel employee);
        public List<EmployeeHistoryDataViewModel> GetEmployeeHistoryByDateAndDepartment(DateTime firstDate, DateTime secondDate, string departmentId);
    }

}
