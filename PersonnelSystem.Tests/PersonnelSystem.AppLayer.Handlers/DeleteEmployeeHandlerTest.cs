using Microsoft.VisualStudio.TestTools.UnitTesting;
using PersonnelSystem.AppLayer.Handlers;
using PersonnelSystem.AppLayer.RequestModels;
using PersonnelSystem.DomainLayer.Models;
using PersonnelSystem.Tests.PersonnelSystem.InfrastructureLayer.QuerySources;
using System;
using System.Linq;

namespace PersonnelSystem.Tests.PersonnelSystem.AppLayer.Handlers
{
    public class DeleteEmployeeHandlerTest
    {
        private DeleteEmployeeHandler _handler;
        private DepartmentQuerySourceMock _departmentQuerySourceMock;
        private EmployeeQuerySourceMock _employeeQuerySourceMock;
        private EmployeeHistoryQuerySourceMock _employeeHistoryQuerySourceMock;

        DeleteEmployeeRequestModel _deleteEmployeeRequestModel;

        [TestInitialize]
        public void Initialize()
        {
            //using Mock
            _employeeQuerySourceMock = new EmployeeQuerySourceMock();
            _departmentQuerySourceMock = new DepartmentQuerySourceMock();
            _employeeHistoryQuerySourceMock = new EmployeeHistoryQuerySourceMock();
            //request
            var departmentId = _departmentQuerySourceMock.Create(new Department(DepartmentId.CreateNew(), "Department"));
            var oldEmployeeId = _employeeQuerySourceMock.Create(new Employee(EmployeeId.CreateNew(), "Имя", "Фамилия", "Отчество", departmentId));
            _deleteEmployeeRequestModel = new DeleteEmployeeRequestModel()
            {
                Id = oldEmployeeId.ToString(),
            };

            _handler = new DeleteEmployeeHandler(
                _employeeQuerySourceMock,
                _employeeHistoryQuerySourceMock);
        }

        [TestMethod]
        public void DeleteEmployee_NoErrors()
        {
            _handler.Execute(_deleteEmployeeRequestModel);

            // Assert
            var deleted_value = _employeeQuerySourceMock.GetAll().FirstOrDefault(x => x.Id.ToString() == _deleteEmployeeRequestModel.Id);

            Assert.IsNull(deleted_value); // проверка что из списка он удалился
            Assert.IsTrue(_employeeHistoryQuerySourceMock.GetAll()
                .Any(d => d.EmployeeId == _deleteEmployeeRequestModel.Id.ToString() && d.IsDeleted)); // в таблице истории есть запись на то, что этот сотрудник удален
        }

        [TestMethod]
        public void DeleteEmployee_NotFound()
        {
            _deleteEmployeeRequestModel = new DeleteEmployeeRequestModel { Id= $"EmployeeId_{Guid.NewGuid}" };            

            var exception = Assert.ThrowsException<Exception>(() =>
                _handler.Execute(_deleteEmployeeRequestModel));

            // Assert
            Assert.AreEqual("Не найден сотрудник", exception.Message);
        }
    }
}

