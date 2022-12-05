using System;

namespace PersonnelSystem.DomainLayer.Models
{
    public abstract class Entity<T> where T : EntityId
    {
        public new T Id { get; }

        protected Entity(T id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }
    }

    public abstract record EntityId
    {
        protected readonly string Value;
        protected EntityId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(id, "Идентификатор не может быть пустым");

            Value = id;
        }
    }

}
