using System;


namespace PersonnelSystem.DomainLayer.Models
{
    public record DepartmentId : EntityId
    {
        private DepartmentId(string id) : base(id) { }

        public override string ToString() => Value;

        public static DepartmentId FromString(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(id, "Идентификатор Подразделения не может быть пустым");

            var sectors = id.Split('_');            

            if (sectors.Length != 2 || sectors[0] != nameof(DepartmentId) || !Guid.TryParse(sectors[1], out _))
                throw new ArgumentException("Невалидный идентификатор Подразделения");

            return new DepartmentId(id);
        }

        public static DepartmentId CreateNew() => new DepartmentId($"{nameof(DepartmentId)}_{Guid.NewGuid()}");
    }

}
