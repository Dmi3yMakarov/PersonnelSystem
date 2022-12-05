using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonnelSystem.AppLayer.RequestModels
{
    public record EditDepartmentRequestModel
    {
        public string Id { get; set; }
        public string DepartmentName { get; set; }
        public string ParentDepartmentId { get; set; }        
    }
}
