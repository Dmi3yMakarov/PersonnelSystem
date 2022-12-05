using PersonnelSystem.AppLayer.DataViewModels;
using PersonnelSystem.AppLayer.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace PersonnelSystem.AppLayer.Handlers
{
    public class GetActualDepartmentListHandler    
    {
        IDepartmentQuerySource _departmentQuerySource;
        public GetActualDepartmentListHandler(IDepartmentQuerySource departmentQuerySource)
        {
            _departmentQuerySource = departmentQuerySource;
        }

        public ActualDepartmentListDataViewModel Execute()
        {
            try
            {
                var department = _departmentQuerySource.GetAll();
                ActualDepartmentListDataViewModel actualDepartmentListDataViewModel = new ActualDepartmentListDataViewModel(
                    new ObservableCollection<ActualDepartmentDataViewModel>(
                        department.Select(x => new ActualDepartmentDataViewModel(x.Id.ToString(), x.Name, x.ParentDepartmentId?.ToString()))));
                return actualDepartmentListDataViewModel;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }           
        }
    }
}
