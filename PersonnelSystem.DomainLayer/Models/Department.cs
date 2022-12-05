using System;
using System.Text.RegularExpressions;

namespace PersonnelSystem.DomainLayer.Models
{
    public class Department : Entity<DepartmentId>
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(value, "Наименование подразделения не может быть пустым");

                if (!Regex.IsMatch(value, @"^([+"""".-]|[a-z A-Z а-я А-Я]|[0-9])"))
                    throw new ArgumentException("В поле Name можно использовать только русские и латинские символы, цифры и спецсимволы (+.-).");
                _name = value;
            }
        }        

        private DepartmentId? _parentDepartmentId;
        public DepartmentId? ParentDepartmentId
        {
            get => _parentDepartmentId;
            set
            {
                if (value == Id)
                    throw new ArgumentException("Подразделение не может быть внутри самого себя");

                _parentDepartmentId = value;
            }
        }
                
        public Department(DepartmentId id, string name, DepartmentId parentDepartmentId = null)
            : base(id)
        {
            Name = name;            
            ParentDepartmentId = parentDepartmentId;            
        }                
    }
}
