using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonnelSystem.AppLayer.DataViewModels
{
    public record ActualDepartmentDataViewModel(string? Id, string Name, string? ParentId)
    {
        public override string ToString()
        {
            return Name;
        }
    }
}
