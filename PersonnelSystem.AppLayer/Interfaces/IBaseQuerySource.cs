using PersonnelSystem.DomainLayer.Models;
using System.Collections.Generic;

namespace PersonnelSystem.AppLayer.Interfaces
{
    public interface IBaseQuerySource<TEntity, TId>
        where TEntity : Entity<TId>
        where TId : EntityId
    {
        public TId Create(TEntity entity);
        public void Update(TEntity entity);
        public TEntity GetById(TId id);
        public List<TEntity> GetAll();
        public void Delete(TId id);
    }

}
