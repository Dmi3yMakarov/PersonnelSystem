using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PersonnelSystem.DomainLayer.Models;

namespace PersonnelSystem.InfrastructureLayer.Context.ValueConverters
{
    internal class EmployeeIdToStringValueConverter : ValueConverter<EmployeeId, string>
    {
        public static readonly ValueComparer<EmployeeId> Comparer = new(
            (id1, id2) => id1 == id2,
            id => id.GetHashCode(),
            id => EmployeeId.FromString(id.ToString()));

        public EmployeeIdToStringValueConverter()
            : base(strongId => strongId.ToString(),
                   databaseId => EmployeeId.FromString(databaseId),
                   null)
        {
        }
    }

}
