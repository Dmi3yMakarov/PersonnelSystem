using PersonnelSystem.AppLayer.DataViewModels;
using PersonnelSystem.AppLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;


namespace PersonnelSystem.InfrastructureLayer.QuerySources
{
    public class DepartmentHistoryQuerySource : IDepartmentHistoryQuerySource
    {
        public void AddDepartmentToHistory(DepartmentHistoryDataViewModel department)
        {
            using (EntityContext db = new EntityContext())
            {
                // добавляем в бд
                db.DepartmentsHistory.Add(department);
                db.SaveChanges();
            }
        }
        public List<DepartmentHistoryDataViewModel> GetDepartmentHistoryByDate(DateTime date)
        {
            List<DepartmentHistoryDataViewModel> departments = new();
            var queryDate = date.Date.AddDays(1);
            using (EntityContext db = new EntityContext())
            {                
                var dept_date = db.DepartmentsHistory.Where(dh => dh.ChangeDate <= queryDate);
                
                var dept_max = dept_date.GroupBy(p => p.DepartmentId,
                                                 p => p.ChangeDate,                                                 
                                                 (key, g) => new DepartmentHistoryDataViewModel { DepartmentId = key, ChangeDate = g.Max() });
                
                var query = from x in dept_max
                            join y in dept_date on new
                            {
                                x.DepartmentId,
                                x.ChangeDate
                            } equals new
                            {
                                y.DepartmentId,
                                y.ChangeDate
                            }
                            into j1
                            from j2 in j1.DefaultIfEmpty()
                            select new { j2.DepartmentId, j2.Name, j2.ChangeDate, j2.IsDeleted, j2.ParentDepartmentId };
                
                departments = query
                    .Distinct()
                    .Where(q => !q.IsDeleted)
                    .Select(q => new DepartmentHistoryDataViewModel 
                    { 
                        Name = q.Name, 
                        DepartmentId = q.DepartmentId,
                        ParentDepartmentId = q.ParentDepartmentId,
                        ChangeDate = q.ChangeDate,
                        IsDeleted = q.IsDeleted 
                    })
                    .ToList();
            }

            return departments;
        }
    }
}
