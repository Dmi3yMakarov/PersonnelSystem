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
    public class EditDepartmentHandlerTest
    {
        private EditDepartmentHandler _handler;
        private DepartmentQuerySourceMock _departmentQuerySourceMock;
        private DepartmentHistoryQuerySourceMock _departmentHistoryQuerySourceMock;

        EditDepartmentRequestModel _editDepartmentRequestModel;

        [TestInitialize]
        public void Initialize()
        {
            //using Mock
            _departmentQuerySourceMock = new DepartmentQuerySourceMock();
            _departmentHistoryQuerySourceMock = new DepartmentHistoryQuerySourceMock();
            //request
            
            var oldDepartmentId = _departmentQuerySourceMock.Create(new Department(DepartmentId.CreateNew(), "Old department"));

            _editDepartmentRequestModel = new EditDepartmentRequestModel()
            {
                Id = oldDepartmentId.ToString(),
                DepartmentName = "New Department"
            };

            _handler = new EditDepartmentHandler(
                _departmentQuerySourceMock,
                _departmentHistoryQuerySourceMock);
        }

        [TestMethod]
        public void EditDepartment_NoErrors()
        {
            _handler.Execute(_editDepartmentRequestModel);

            // Assert
            var edited_value = _departmentQuerySourceMock.GetAll().FirstOrDefault(x => x.Id.ToString() == _editDepartmentRequestModel.Id);

            Assert.IsNotNull(edited_value); // проверка что в список он добавился
            Assert.IsTrue(_departmentHistoryQuerySourceMock.GetAll().Any(d => d.DepartmentId == edited_value.Id.ToString())); // в таблице истории есть строка изсенения
            Assert.AreEqual(_editDepartmentRequestModel.DepartmentName, edited_value.Name); // имя соответвует переданному в реквесте
            Assert.IsNull(_editDepartmentRequestModel.ParentDepartmentId); // не указан родитель
        }

        [TestMethod]
        public void EditDepartment_NotFound()
        {
            _editDepartmentRequestModel.Id = $"DepartmentId_{Guid.NewGuid()}";

            var exception = Assert.ThrowsException<Exception>(() =>
                _handler.Execute(_editDepartmentRequestModel));

            // Assert
            Assert.AreEqual("Не найдено подразделение", exception.Message);
        }

        [TestMethod]
        public void EditDepartment_NotCorrectParentId()
        {
            _editDepartmentRequestModel.ParentDepartmentId = "123456";

            var exception = Assert.ThrowsException<Exception>(() =>
                _handler.Execute(_editDepartmentRequestModel));

            // Assert
            Assert.AreEqual("Невалидный идентификатор Подразделения", exception.Message);
        }

        [TestMethod]
        public void EditDepartment_ParentNotFound()
        {
            _editDepartmentRequestModel.ParentDepartmentId = $"DepartmentId_{Guid.NewGuid()}";

            var exception = Assert.ThrowsException<Exception>(() =>
                _handler.Execute(_editDepartmentRequestModel));

            // Assert
            Assert.AreEqual("Не найдено родительское подразделение", exception.Message);
        }

        [TestMethod]
        public void EditDepartment_ChildOfYourself()
        {
            _editDepartmentRequestModel.ParentDepartmentId = _editDepartmentRequestModel.Id;

            var exception = Assert.ThrowsException<Exception>(() =>
                _handler.Execute(_editDepartmentRequestModel));

            // Assert
            Assert.AreEqual("Подразделение не может быть внутри самого себя", exception.Message);
        }
    }
}

