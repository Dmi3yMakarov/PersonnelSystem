using System;


namespace PersonnelSystem.DomainLayer.Models
{
    public record EmployeeId : EntityId
    {
        private EmployeeId(string id) : base(id) { }

        public override string ToString() => Value;

        public static EmployeeId FromString(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(id, "Идентификатор Сотрудника не может быть пустым");

            var sectors = id.Split('_');

            if (sectors.Length != 2 || sectors[0] != nameof(EmployeeId) || !Guid.TryParse(sectors[1], out _))
                throw new ArgumentException("Невалидный идентификатор Сотрудника");

            return new EmployeeId(id);
        }

        public static EmployeeId CreateNew() => new EmployeeId($"{nameof(EmployeeId)}_{Guid.NewGuid()}");
    }

}
