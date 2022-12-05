using PersonnelSystem.AppLayer.Interfaces;
using PersonnelSystem.DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonnelSystem.Tests.PersonnelSystem.InfrastructureLayer.QuerySources
{
    internal class EmployeeQuerySourceMock : IEmployeeQuerySource
    {
        private readonly List<Employee> _employees = new List<Employee>();
        public EmployeeId Create(Employee entity)
        {
            _employees.Add(entity);
            return entity.Id;
        }

        public void Delete(EmployeeId id)
        {
            var emp = GetById(id);

            if (emp == null)
                throw new ArgumentNullException(nameof(id));

            _employees.Remove(emp);
        }

        public List<Employee> GetAll()
        {
            return _employees;
        }

        public Employee GetById(EmployeeId id)
        {
            return _employees.FirstOrDefault(e => e.Id == id);
        }

        public void Update(Employee entity)
        {
            var emp = GetById(entity.Id);

            if (emp == null)
                throw new ArgumentNullException(nameof(entity.Id));

            emp.FirstName = entity.FirstName;
            emp.LastName = entity.LastName;
            emp.Patronymic = entity.Patronymic;
            emp.DepartmentId = entity.DepartmentId;
        }
    }
}

