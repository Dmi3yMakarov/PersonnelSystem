using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonnelSystem.AppLayer.RequestModels
{
    public record DateEmployeeDepartmentRequestModel
    {
        public DateTime FromDateTime { get; set; }
        public DateTime ToDateTime { get; set; }
        public string DepartmentId { get; set; }
    }
}
