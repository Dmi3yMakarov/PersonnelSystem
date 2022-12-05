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
    public class DeleteDepartmentHandlerTest
    {
        private DeleteDepartmentHandler _handler;
        private DepartmentQuerySourceMock _departmentQuerySourceMock;
        private DepartmentHistoryQuerySourceMock _departmentHistoryQuerySourceMock;

        DeleteDepartmentRequestModel _deleteDepartmentRequestModel;

        [TestInitialize]
        public void Initialize()
        {
            //using Mock
            _departmentQuerySourceMock = new DepartmentQuerySourceMock();
            _departmentHistoryQuerySourceMock = new DepartmentHistoryQuerySourceMock();
            //request
            var oldDepartmentId = _departmentQuerySourceMock.Create(new Department(DepartmentId.CreateNew(), "Old department"));
            _deleteDepartmentRequestModel = new DeleteDepartmentRequestModel()
            {
                Id = oldDepartmentId.ToString(),
            };

            _handler = new DeleteDepartmentHandler(
                _departmentQuerySourceMock,
                _departmentHistoryQuerySourceMock);
        }

        [TestMethod]
        public void DeleteDepartment_NoErrors()
        {
            _handler.Execute(_deleteDepartmentRequestModel);

            // Assert
            var deleted_value = _departmentQuerySourceMock.GetAll().FirstOrDefault(x => x.Id.ToString() == _deleteDepartmentRequestModel.Id);

            Assert.IsNull(deleted_value); // проверка что из списка он удалился
            Assert.IsTrue(_departmentHistoryQuerySourceMock.GetAll()
                .Any(d => d.DepartmentId == _deleteDepartmentRequestModel.Id.ToString() && d.IsDeleted)); // в таблице истории есть запись на то, что это подраделение удалено
        }

        [TestMethod]
        public void DeleteDepartment_NotFound()
        {
            _deleteDepartmentRequestModel.Id = $"DepartmentId_{Guid.NewGuid()}";

            var exception = Assert.ThrowsException<Exception>(() =>
                _handler.Execute(_deleteDepartmentRequestModel));

            // Assert
            Assert.AreEqual("Не найдено подразделение", exception.Message);
        }
                
        [TestMethod]
        public void DeleteDepartment_HasChildren()
        {
            var depId = DepartmentId.CreateNew();
            _departmentQuerySourceMock.Create(new Department(depId, "Name"));            
            _departmentQuerySourceMock.Create(new Department(DepartmentId.CreateNew(), "Name", depId));

            _deleteDepartmentRequestModel.Id = depId.ToString();

            var exception = Assert.ThrowsException<Exception>(() =>
                _handler.Execute(_deleteDepartmentRequestModel));

            // Assert
            Assert.AreEqual("Удалять можно только подразделения нижнего звена!", exception.Message);
        }
    }
}

