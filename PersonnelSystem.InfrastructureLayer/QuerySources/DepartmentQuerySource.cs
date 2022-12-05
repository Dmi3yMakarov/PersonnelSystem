using PersonnelSystem.AppLayer.Interfaces;
using PersonnelSystem.DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonnelSystem.InfrastructureLayer.QuerySources
{
    public class DepartmentQuerySource : IDepartmentQuerySource
    {
        public DepartmentId Create(Department department)
        {
            using (EntityContext db = new EntityContext())
            {
                // добавляем в бд
                db.Departments.Add(department);
                db.SaveChanges();
            }

            return department.Id;
        }

        public void Update(Department department)
        {
            using (EntityContext db = new EntityContext())
            {
                Department currentDepartment = GetById(department.Id);
                if (currentDepartment == null)
                    throw new ArgumentNullException($"В базе не найдено объекта {department.Name}");

                db.Departments.Update(department);
                db.SaveChanges();
            }
        }

        public Department GetById(DepartmentId departmentId)
        {
            Department department = null;
            using (EntityContext db = new EntityContext())
            {
                department = db.Departments.FirstOrDefault(emp => emp.Id == departmentId);
            }
            return department;
        }

        public List<Department> GetAll()
        {
            List<Department> departments = new List<Department>();
            using (EntityContext db = new EntityContext())
            {
                departments = db.Departments?.ToList();
            }
            return departments;
        }

        public void Delete(DepartmentId departmentId)
        {
            using (EntityContext db = new EntityContext())
            {
                Department currentDepartment = GetById(departmentId);

                if (currentDepartment == null)
                    throw new ArgumentNullException($"В базе не найдено объекта {departmentId}");

                db.Departments.Remove(currentDepartment);
                db.SaveChanges();
            }
        }

        public bool HasChildren(DepartmentId departmentId)
        {
            bool result = false;
            using (EntityContext db = new EntityContext())
            {                
                result = db.Departments.Any(emp => emp.ParentDepartmentId == departmentId);
            }
            return result;
        }
    }

}
