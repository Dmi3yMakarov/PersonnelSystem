using PersonnelSystem.DomainLayer.Models;
using System;
using System.Collections.Generic;

namespace PersonnelSystem.AppLayer.Interfaces
{
    public interface IDepartmentQuerySource : IBaseQuerySource<Department, DepartmentId>
    {
        public bool HasChildren(DepartmentId departmentId);
    }

}
