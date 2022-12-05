using Microsoft.VisualStudio.TestTools.UnitTesting;
using PersonnelSystem.AppLayer.Handlers;
using PersonnelSystem.AppLayer.RequestModels;
using PersonnelSystem.DomainLayer.Models;
using PersonnelSystem.Tests.PersonnelSystem.InfrastructureLayer.QuerySources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonnelSystem.Tests.PersonnelSystem.AppLayer.Handlers
{
    [TestClass]
    public class EditEmployeeHandlerTest
    {
        private EditEmployeeHandler _handler;
        private DepartmentQuerySourceMock _departmentQuerySourceMock;
        private EmployeeQuerySourceMock _employeeQuerySourceMock;
        private EmployeeHistoryQuerySourceMock _employeeHistoryQuerySourceMock;

        EditEmployeeRequestModel _editEmployeeRequestModel;

        [TestInitialize]
        public void Initialize()
        {
            //using Mock
            _departmentQuerySourceMock = new DepartmentQuerySourceMock();
            _employeeQuerySourceMock = new EmployeeQuerySourceMock();
            _employeeHistoryQuerySourceMock = new EmployeeHistoryQuerySourceMock();
            //request
            var departmentId = _departmentQuerySourceMock.Create(new Department(DepartmentId.CreateNew(), "Department"));
            var oldEmployeeId = _employeeQuerySourceMock.Create(new Employee(EmployeeId.CreateNew(), "Имя", "Фамилия", "Отчество", departmentId));
            _editEmployeeRequestModel = new EditEmployeeRequestModel()
            {
                Id = oldEmployeeId.ToString(),
                FirstName = "Сотрудник",
                LastName = "Новый",
                Patronymic = null,
                DepartmentId = departmentId.ToString()
            };

            _handler = new EditEmployeeHandler(
                _employeeQuerySourceMock,
                _departmentQuerySourceMock,
                _employeeHistoryQuerySourceMock);
        }

        [TestMethod]
        public void EditEmployee_NoErrors()
        {
            _handler.Execute(_editEmployeeRequestModel);

            // Assert
            var edited_value = _employeeQuerySourceMock.GetAll().FirstOrDefault(x => x.Id.ToString() == _editEmployeeRequestModel.Id);

            Assert.IsNotNull(edited_value); // проверка что в список он добавился
            Assert.IsTrue(_employeeHistoryQuerySourceMock.GetAll().Any(d => d.EmployeeId == edited_value.Id.ToString())); // в таблице истории есть запись
            Assert.AreEqual(_editEmployeeRequestModel.FirstName, edited_value.FirstName); // имя соответвует переданному в реквесте
            Assert.IsNull(_editEmployeeRequestModel.Patronymic); // не указан родитель
        }

        [TestMethod]
        public void EditEmployee_NotFound()
        {
            _editEmployeeRequestModel.Id = $"EmployeeId_{Guid.NewGuid()}";

            var exception = Assert.ThrowsException<Exception>(() =>
                _handler.Execute(_editEmployeeRequestModel));

            // Assert
            Assert.AreEqual("Не найден сотрудник", exception.Message);
        }

        [TestMethod]
        public void EditEmployee_NotCorrectDepartmentId()
        {
            _editEmployeeRequestModel.DepartmentId = "123456";

            var exception = Assert.ThrowsException<Exception>(() =>
                _handler.Execute(_editEmployeeRequestModel));

            // Assert
            Assert.AreEqual("Невалидный идентификатор Подразделения", exception.Message);
        }

        [TestMethod]
        public void EditEmployee_DepartmentNotFound()
        {
            _editEmployeeRequestModel.DepartmentId = $"DepartmentId_{Guid.NewGuid()}";

            var exception = Assert.ThrowsException<Exception>(() =>
                _handler.Execute(_editEmployeeRequestModel));

            // Assert
            Assert.AreEqual("Не найдено родительское подразделение", exception.Message);
        }

        [TestMethod]
        public void EditEmployee_NotCorrectLastName()
        {
            _editEmployeeRequestModel.LastName = "en-En locale";

            var exception = Assert.ThrowsException<Exception>(() =>
                _handler.Execute(_editEmployeeRequestModel));

            // Assert
            Assert.AreEqual("В поле LastName можно использовать только русские символы, от 2 до 20 символов.", exception.Message);
        }

        [TestMethod]
        public void CreateEmployee_WithoutDepartment()
        {
            _editEmployeeRequestModel.DepartmentId = null;

            var exception = Assert.ThrowsException<Exception>(() =>
                _handler.Execute(_editEmployeeRequestModel));

            // Assert
            Assert.AreEqual("Идентификатор Подразделения не может быть пустым", exception.Message);
        }
    }
}

