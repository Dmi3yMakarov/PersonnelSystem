using System;
using System.Text.RegularExpressions;

namespace PersonnelSystem.DomainLayer.Models
{
    public class Employee : Entity<EmployeeId>
    {
        private string _firstName;        
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("Параметр Имя не может быть пусто");

                CheckField(value, nameof(FirstName));

                _firstName = value;                
            }
        }
        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("Параметр Фамилия не может быть пусто");

                CheckField(value, nameof(LastName));

                _lastName = value;                
            }
        }

        private string? _patronymic;
        public string? Patronymic 
        { 
            get => _patronymic;
            set 
            {
                if(!string.IsNullOrWhiteSpace(value))
                    CheckField(value, nameof(Patronymic));

                _patronymic = value;
            } 
        }

        public string FullName
        {
            get => $"{LastName} {FirstName}" + (string.IsNullOrWhiteSpace(Patronymic) ? "" : $" {Patronymic}");
        }

        private DepartmentId _departmentId;
        public DepartmentId DepartmentId
        {
            get => _departmentId;
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Параметр Подразделение не может быть пусто");

                _departmentId = value;
            }
        }
        
        public Employee(EmployeeId id, string firstName, string lastName, string? patronymic, DepartmentId departmentId)
            : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            Patronymic = patronymic;
            DepartmentId = departmentId;            
        }

        private void CheckField(string field, string fieldName)
        {
            if (!Regex.IsMatch(field, @"^([а-я А-Я]){2,20}$"))
                throw new ArgumentException($"В поле {fieldName} можно использовать только русские символы, от 2 до 20 символов.");
        }
    }

}
