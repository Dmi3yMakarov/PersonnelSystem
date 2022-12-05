

namespace PersonnelSystem.AppLayer.RequestModels
{
    public record CreateEmployeeRequestModel
    {                       
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string DepartmentId { get; set; }
    }
}
