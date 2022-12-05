using Microsoft.VisualStudio.TestTools.UnitTesting;
using PersonnelSystem.AppLayer.Handlers;
using PersonnelSystem.AppLayer.RequestModels;
using PersonnelSystem.DomainLayer.Models;
using PersonnelSystem.Tests.PersonnelSystem.InfrastructureLayer.QuerySources;
using System;
using System.Linq;
using System.Xml.Linq;

namespace PersonnelSystem.Tests.PersonnelSystem.AppLayer.Handlers
{
    [TestClass]
    public class CreateDepartmentHandlerTest
    {
        private CreateDepartmentHandler _handler;
        private DepartmentQuerySourceMock _departmentQuerySourceMock;
        private DepartmentHistoryQuerySourceMock _departmentHistoryQuerySourceMock;

        CreateDepartmentRequestModel _createDepartmentRequestModel;

        [TestInitialize]
        public void Initialize()
        {
            //using Mock
            _departmentQuerySourceMock = new DepartmentQuerySourceMock();
            _departmentHistoryQuerySourceMock = new DepartmentHistoryQuerySourceMock();
            //request
            _createDepartmentRequestModel = new CreateDepartmentRequestModel();

            _handler = new CreateDepartmentHandler(
                _departmentQuerySourceMock,
                _departmentHistoryQuerySourceMock);
        }

        [TestMethod]
        public void CreateDepartment_NoErrors()
        {
            _createDepartmentRequestModel.DepartmentName = "New Department";

            _handler.Execute(_createDepartmentRequestModel);

            // Assert
            var inserted_value = _departmentQuerySourceMock.GetAll()
                .FirstOrDefault(x => x.Name == _createDepartmentRequestModel.DepartmentName);

            Assert.IsNotNull(inserted_value); // проверка что в список он добавился
            Assert.IsTrue(inserted_value.Id.ToString().Contains(nameof(DepartmentId))); // проверка что идентификатор вида DepartmentId_
            Assert.IsTrue(_departmentHistoryQuerySourceMock.GetAll().Any(d => d.DepartmentId == inserted_value.Id.ToString())); // в таблицу истории запись тоже добавилась
            Assert.AreEqual(_createDepartmentRequestModel.DepartmentName, inserted_value.Name); // имя соответвует переданному в реквесте
        }

        [TestMethod]
        public void CreateDepartment_NotCorrectParentId()
        {
            _createDepartmentRequestModel.DepartmentName = "New Department";
            _createDepartmentRequestModel.ParentDepartmentId = "123456";

            var exception = Assert.ThrowsException<Exception>(() =>
                _handler.Execute(_createDepartmentRequestModel));

            Assert.AreEqual("Невалидный идентификатор Подразделения", exception.Message);
        }

        [TestMethod]
        public void CreateDepartment_ParentNotFound()
        {
            _createDepartmentRequestModel.DepartmentName = "New Department";
            _createDepartmentRequestModel.ParentDepartmentId = $"DepartmentId_{Guid.NewGuid()}";

            var exception = Assert.ThrowsException<Exception>(() =>
                _handler.Execute(_createDepartmentRequestModel));

            // Assert
            Assert.AreEqual("Не найдено родительское подразделение", exception.Message);
        }

        [TestMethod]
        public void CreateDepartment_EmptyName()
        {
            _createDepartmentRequestModel.DepartmentName = "";            

            var exception = Assert.ThrowsException<Exception>(() =>
                _handler.Execute(_createDepartmentRequestModel));

            // Assert
            Assert.AreEqual("Наименование подразделения не может быть пустым", exception.Message);
        }

        [TestMethod]
        public void CreateDepartment_NotCorrectName()
        {
            _createDepartmentRequestModel.DepartmentName = "!@#$%^&&*";            

            var exception = Assert.ThrowsException<Exception>(() =>
                _handler.Execute(_createDepartmentRequestModel));

            // Assert
            Assert.AreEqual("В поле Name можно использовать только русские и латинские символы, цифры и спецсимволы (+.-).", exception.Message);
        }

        [TestMethod]
        public void CreateDepartment_NameExists()
        {
            _departmentQuerySourceMock.Create(new Department(DepartmentId.CreateNew(), "Name"));
            _createDepartmentRequestModel.DepartmentName = "Name";            

            var exception = Assert.ThrowsException<Exception>(() =>
                _handler.Execute(_createDepartmentRequestModel));

            // Assert
            Assert.AreEqual("Подразделение с таким наименованием уже существует!", exception.Message);
        }        
    }
}

