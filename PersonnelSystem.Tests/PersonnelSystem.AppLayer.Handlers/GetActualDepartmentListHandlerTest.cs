using Microsoft.VisualStudio.TestTools.UnitTesting;
using PersonnelSystem.AppLayer.Handlers;
using PersonnelSystem.DomainLayer.Models;
using PersonnelSystem.Tests.PersonnelSystem.InfrastructureLayer.QuerySources;

namespace PersonnelSystem.Tests.PersonnelSystem.AppLayer.Handlers
{
    [TestClass]
    public class GetActualDepartmentListHandlerTest
    {
        private GetActualDepartmentListHandler _handler;
        private DepartmentQuerySourceMock _departmentQuerySourceMock;

        [TestInitialize]
        public void Initialize()
        {
            //using Mock
            _departmentQuerySourceMock = new DepartmentQuerySourceMock();

            _handler = new GetActualDepartmentListHandler(_departmentQuerySourceMock);
        }

        [TestMethod]
        public void DeleteDepartment_NoErrors()
        {
            int countItems = 4;
            for (int i = 0; i < countItems; i++)
                _departmentQuerySourceMock.Create(new Department(DepartmentId.CreateNew(), $"Department_{i}"));

            var result = _handler.Execute();

            // Assert
            Assert.AreEqual(countItems, result.ActualDepartments.Count); // проверка кол-ва записей
        }
    }
}

