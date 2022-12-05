using PersonnelSystem.AppLayer.DataViewModels;
using PersonnelSystem.AppLayer.Interfaces;
using PersonnelSystem.DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonnelSystem.Tests.PersonnelSystem.InfrastructureLayer.QuerySources
{
    internal class EmployeeHistoryQuerySourceMock : IEmployeeHistoryQuerySource
    {
        private readonly List<EmployeeHistoryDataViewModel> _employees = new();
        public void AddEmployeeToHistory(EmployeeHistoryDataViewModel employee)
        {
            _employees.Add(employee);
        }

        public List<EmployeeHistoryDataViewModel> GetEmployeeHistoryByDateAndDepartment(DateTime firstDate, DateTime secondDate, string departmentId)
        {
            var dateFrom = firstDate.Date.AddDays(1);
            var dateTo = secondDate.Date.AddDays(1);

            var emp_hist = _employees.Where(emp => emp.ChangeDate <= dateTo
                                                        && emp.DepartmentId == departmentId);

            var emp_max = emp_hist.GroupBy(p => p.EmployeeId,
                                            p => p.ChangeDate,
                                            (key, g) => new EmployeeHistoryDataViewModel { EmployeeId = key, ChangeDate = g.Max() });

            var full_query = from x in emp_max
                             join y in emp_hist on new
                             {
                                 x.EmployeeId,
                                 x.ChangeDate
                             } equals new
                             {
                                 y.EmployeeId,
                                 y.ChangeDate
                             }
                             into j1
                             from j2 in j1.DefaultIfEmpty()
                             select new { j2.EmployeeId, j2.FullName, j2.DepartmentId, j2.ChangeDate, j2.IsDeleted };

            var except_query = full_query.Where(q => q.IsDeleted && q.ChangeDate <= dateFrom);

            var query = full_query.Except(except_query);

            var employees = query
                .Distinct()
                .Select(q => new EmployeeHistoryDataViewModel
                {
                    FullName = q.FullName,
                    EmployeeId = q.EmployeeId,
                    DepartmentId = q.DepartmentId,
                    ChangeDate = q.ChangeDate,
                    IsDeleted = q.IsDeleted
                })
                .ToList();

            return employees;
        }
        public List<EmployeeHistoryDataViewModel> GetAll() => _employees;
    }
}

