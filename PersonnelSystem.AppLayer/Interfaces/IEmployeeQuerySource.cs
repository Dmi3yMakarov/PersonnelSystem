using PersonnelSystem.DomainLayer.Models;
using System;
using System.Collections.Generic;

namespace PersonnelSystem.AppLayer.Interfaces
{
    public interface IEmployeeQuerySource : IBaseQuerySource<Employee, EmployeeId>
    {
    }

}
