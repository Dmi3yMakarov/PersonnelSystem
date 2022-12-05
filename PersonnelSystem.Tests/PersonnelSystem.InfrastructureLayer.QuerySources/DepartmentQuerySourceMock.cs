using PersonnelSystem.AppLayer.Interfaces;
using PersonnelSystem.DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonnelSystem.Tests.PersonnelSystem.InfrastructureLayer.QuerySources
{
    internal class DepartmentQuerySourceMock : IDepartmentQuerySource
    {
        private readonly List<Department> _departments = new List<Department>();
        public DepartmentId Create(Department entity)
        {
            _departments.Add(entity);
            return entity.Id;
        }

        public void Delete(DepartmentId id)
        {
            var dep = _departments.FirstOrDefault(d => d.Id == id);

            if (dep == null)
                throw new ArgumentNullException(nameof(id));

            _departments.Remove(dep);
        }

        public List<Department> GetAll()
        {
            return _departments;
        }

        public Department GetById(DepartmentId id)
        {
            return _departments.FirstOrDefault(d => d.Id == id);
        }

        public bool HasChildren(DepartmentId departmentId)
        {            
            return _departments.Any(emp => emp.ParentDepartmentId == departmentId);
        }

        public void Update(Department entity)
        {
            var dep = _departments.FirstOrDefault(d => d.Id == entity.Id);

            if (dep == null)
                throw new ArgumentNullException(nameof(entity.Id));

            dep.Name = entity.Name;
            dep.ParentDepartmentId = entity.ParentDepartmentId;
        }
    }
}

