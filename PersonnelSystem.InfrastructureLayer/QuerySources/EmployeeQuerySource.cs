using PersonnelSystem.AppLayer.Interfaces;
using PersonnelSystem.DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonnelSystem.InfrastructureLayer.QuerySources
{
    public class EmployeeQuerySource : IEmployeeQuerySource
    {
        public EmployeeId Create(Employee employee)
        {
            using (EntityContext db = new EntityContext())
            {
                // добавляем в бд
                db.Employees.Add(employee);
                db.SaveChanges();
            }

            return employee.Id;
        }

        public void Update(Employee employee)
        {
            using (EntityContext db = new EntityContext())
            {
                Employee emp = GetById(employee.Id);
                if (emp == null)
                    throw new ArgumentNullException($"В базе не найдено объекта {employee}");

                db.Employees.Update(employee);
                db.SaveChanges();
            }
        }

        public Employee GetById(EmployeeId employeeId)
        {
            Employee employee = null;
            using (EntityContext db = new EntityContext())
            {
                employee = db.Employees.FirstOrDefault(emp => emp.Id == employeeId);
            }
            return employee;
        }

        public List<Employee> GetAll()
        {
            List<Employee> employees = new List<Employee>();
            using (EntityContext db = new EntityContext())
            {
                employees = db.Employees.ToList();
            }
            return employees;
        }

        public void Delete(EmployeeId employeeId)
        {
            using (EntityContext db = new EntityContext())
            {
                Employee employee = GetById(employeeId);

                if (employee == null)
                    throw new ArgumentNullException($"В базе не найдено объекта {employeeId}");                

                db.Employees.Remove(employee);
                db.SaveChanges();
            }
        }
    }

}
