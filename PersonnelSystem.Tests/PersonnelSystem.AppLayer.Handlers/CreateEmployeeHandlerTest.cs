using Microsoft.VisualStudio.TestTools.UnitTesting;
using PersonnelSystem.AppLayer.Handlers;
using PersonnelSystem.AppLayer.RequestModels;
using PersonnelSystem.DomainLayer.Models;
using PersonnelSystem.Tests.PersonnelSystem.InfrastructureLayer.QuerySources;
using System;
using System.Linq;

namespace PersonnelSystem.Tests.PersonnelSystem.AppLayer.Handlers
{
    [TestClass]
    public class CreateEmployeeHandlerTest
    {
        private CreateEmployeeHandler _handler;
        private EmployeeQuerySourceMock _employeeQuerySourceMock;
        private EmployeeHistoryQuerySourceMock _employeeHistoryQuerySourceMock;
        private DepartmentQuerySourceMock _departmentQuerySourceMock;

        CreateEmployeeRequestModel _createEmployeeRequestModel;

        [TestInitialize]
        public void Initialize()
        {
            //using Mock
            _employeeQuerySourceMock = new EmployeeQuerySourceMock();
            _employeeHistoryQuerySourceMock = new EmployeeHistoryQuerySourceMock();
            _departmentQuerySourceMock = new DepartmentQuerySourceMock();
            //request
            DepartmentId departmentId = _departmentQuerySourceMock.Create(new Department(DepartmentId.CreateNew(), "department"));
            _createEmployeeRequestModel = new CreateEmployeeRequestModel()
            {
                FirstName = "Петр",
                LastName = "Петров",
                Patronymic = "Петрович",
                DepartmentId = departmentId.ToString()
            };

            _handler = new CreateEmployeeHandler(
                _employeeQuerySourceMock,
                _departmentQuerySourceMock,
                _employeeHistoryQuerySourceMock);
        }

        [TestMethod]
        public void CreateEmployee_NoErrors()
        {
            _handler.Execute(_createEmployeeRequestModel);

            // Assert
            var inserted_value = _employeeQuerySourceMock.GetAll().FirstOrDefault(x => x.FirstName == _createEmployeeRequestModel.FirstName
                                                                                    && x.LastName == _createEmployeeRequestModel.LastName);

            Assert.IsNotNull(inserted_value); // проверка что в список он добавился
            Assert.IsTrue(inserted_value.Id.ToString().Contains(nameof(EmployeeId))); // проверка что идентификатор вида EmployeeId_
            Assert.IsTrue(_employeeHistoryQuerySourceMock.GetAll().Any(d => d.EmployeeId == inserted_value.Id.ToString())); // в таблицу истории запись тоже добавилась
            Assert.AreEqual(_createEmployeeRequestModel.FirstName, inserted_value.FirstName); // имя соответвует переданному в реквесте
            Assert.AreEqual(_createEmployeeRequestModel.LastName, inserted_value.LastName); // фамилия соответвует переданному в реквесте
            Assert.AreEqual(_createEmployeeRequestModel.Patronymic, inserted_value.Patronymic); // отчество соответвует переданному в реквесте
            Assert.AreEqual(_createEmployeeRequestModel.DepartmentId, inserted_value.DepartmentId.ToString()); // подразделение соответвует переданному в реквесте

        }

        [TestMethod]
        public void CreateEmployee_NotCorrectDepartmentId()
        {
            _createEmployeeRequestModel.DepartmentId = "123456";

            var exception = Assert.ThrowsException<Exception>(() =>
                _handler.Execute(_createEmployeeRequestModel));

            // Assert
            Assert.AreEqual("Невалидный идентификатор Подразделения", exception.Message);
        }

        [TestMethod]
        public void CreateEmployee_DepartmenNotFound()
        {
            _createEmployeeRequestModel.DepartmentId = $"DepartmentId_{Guid.NewGuid()}";

            var exception = Assert.ThrowsException<Exception>(() =>
                _handler.Execute(_createEmployeeRequestModel));

            // Assert
            Assert.AreEqual("Не найдено подразделение", exception.Message);
        }

        [TestMethod]
        public void CreateEmployee_EmptyFirstName()
        {
            _createEmployeeRequestModel.FirstName = "";

            var exception = Assert.ThrowsException<Exception>(() =>
                _handler.Execute(_createEmployeeRequestModel));

            // Assert
            Assert.IsTrue(exception.Message.Contains("Параметр Имя не может быть пусто"));
        }

        [TestMethod]
        public void CreateEmployee_EmptyLastName()
        {
            _createEmployeeRequestModel.LastName = "";

            var exception = Assert.ThrowsException<Exception>(() =>
                _handler.Execute(_createEmployeeRequestModel));

            // Assert
            Assert.IsTrue(exception.Message.Contains("Параметр Фамилия не может быть пусто"));
        }

        [TestMethod]
        public void CreateEmployee_NotCorrectName()
        {
            _createEmployeeRequestModel.FirstName = "LongName11111111!@#$%^&&*";

            var exception = Assert.ThrowsException<Exception>(() =>
                _handler.Execute(_createEmployeeRequestModel));

            // Assert
            Assert.AreEqual("В поле FirstName можно использовать только русские символы, от 2 до 20 символов.", exception.Message);
        }

        [TestMethod]
        public void CreateEmployee_ShortName()
        {
            _createEmployeeRequestModel.LastName = "в";

            var exception = Assert.ThrowsException<Exception>(() =>
                _handler.Execute(_createEmployeeRequestModel));

            // Assert
            Assert.AreEqual("В поле LastName можно использовать только русские символы, от 2 до 20 символов.", exception.Message);
        }

        [TestMethod]
        public void CreateEmployee_WithoutDepartment()
        {
            _createEmployeeRequestModel.DepartmentId = null;

            var exception = Assert.ThrowsException<Exception>(() =>
                _handler.Execute(_createEmployeeRequestModel));

            // Assert
            Assert.AreEqual("Идентификатор Подразделения не может быть пустым", exception.Message);
        }
    }
}

