using PersonnelSystem.AppLayer.DataViewModels;
using PersonnelSystem.AppLayer.Interfaces;
using PersonnelSystem.AppLayer.RequestModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonnelSystem.AppLayer.Handlers
{
    public class GetDepartmentListForDateHandler
    {
        private readonly IDepartmentHistoryQuerySource _departmentHistoryQuerySource;
        public GetDepartmentListForDateHandler(IDepartmentHistoryQuerySource departmentHistoryQuerySource)
        {
            _departmentHistoryQuerySource = departmentHistoryQuerySource;
        }

        public DepartmentListDataViewModel Execute(DateDepartmentStructureRequestModel dateDepartment)
        {
            try
            {
                var deparments = _departmentHistoryQuerySource.GetDepartmentHistoryByDate(dateDepartment.SelectedDateTime);

                var dep = GetDepartmentsOfATree(deparments);
                if (dep.Departments.Any())
                {
                    dep.Departments.FirstOrDefault().IsSelected = true;
                }

                return dep;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static DepartmentListDataViewModel GetDepartmentsOfATree(List<DepartmentHistoryDataViewModel> departments)
        {
            List<DepartmentDataViewModel> departmentTree = new List<DepartmentDataViewModel>();
            foreach (var department in departments)
            {
                if(department.ParentDepartmentId == null)
                {
                    var selectedDepartment = department;                    
                    departmentTree.Add(GetDepartmentData(departments, selectedDepartment));
                }
            }            
            DepartmentListDataViewModel departmentList = new DepartmentListDataViewModel(new ObservableCollection<DepartmentDataViewModel>(departmentTree));
            return departmentList;
        }

        private static DepartmentDataViewModel GetDepartmentData(List<DepartmentHistoryDataViewModel> departments, DepartmentHistoryDataViewModel selectedDepartment)
        {               
            DepartmentDataViewModel department = new DepartmentDataViewModel(selectedDepartment.DepartmentId, selectedDepartment.Name, new ObservableCollection<DepartmentDataViewModel>());
            department.IsExpanded = true;
            department.IsSelected = false;
            foreach (var d in departments)
            {
                if(d.ParentDepartmentId == selectedDepartment.DepartmentId)
                {                    
                    department.ChildDepartments.Add(GetDepartmentData(departments, d));
                }
            }                       
            return department;
        }
    }
}
