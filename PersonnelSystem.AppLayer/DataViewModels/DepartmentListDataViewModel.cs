using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonnelSystem.AppLayer.DataViewModels
{
    public record DepartmentListDataViewModel(ObservableCollection<DepartmentDataViewModel> Departments)
    {
    }
}
