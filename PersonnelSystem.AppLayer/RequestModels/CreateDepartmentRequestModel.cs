using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonnelSystem.AppLayer.RequestModels
{
    public record CreateDepartmentRequestModel
    {           
        public string DepartmentName { get; set; }
        public string ParentDepartmentId { get; set; }        
    }
}
