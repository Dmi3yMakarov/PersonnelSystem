using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonnelSystem.AppLayer.RequestModels
{
    public record DateDepartmentStructureRequestModel
    {
        public DateTime SelectedDateTime { get; set; }
    }
}
