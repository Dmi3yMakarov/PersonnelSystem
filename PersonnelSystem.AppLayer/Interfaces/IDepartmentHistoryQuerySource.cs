using PersonnelSystem.AppLayer.DataViewModels;
using System;
using System.Collections.Generic;

namespace PersonnelSystem.AppLayer.Interfaces
{
    public interface IDepartmentHistoryQuerySource
    {        
        public void AddDepartmentToHistory(DepartmentHistoryDataViewModel departmentHistory);
        public List<DepartmentHistoryDataViewModel> GetDepartmentHistoryByDate(DateTime date);
    }

}
