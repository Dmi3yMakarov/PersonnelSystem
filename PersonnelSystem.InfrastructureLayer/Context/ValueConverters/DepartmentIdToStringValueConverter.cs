using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PersonnelSystem.DomainLayer.Models;


namespace PersonnelSystem.InfrastructureLayer.Context.ValueConverters
{
    internal class DepartmentIdToStringValueConverter : ValueConverter<DepartmentId, string>
    {
        public static readonly ValueComparer<DepartmentId> Comparer = new(
            (id1, id2) => id1 == id2,
            id => id.GetHashCode(),
            id => DepartmentId.FromString(id.ToString()));

        public DepartmentIdToStringValueConverter()
            : base(strongId => strongId.ToString(),
                   databaseId => DepartmentId.FromString(databaseId),
                   null)
        {
        }
    }

}
