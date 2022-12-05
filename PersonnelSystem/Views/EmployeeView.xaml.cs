using PersonnelSystem.AppLayer.DataViewModels;
using PersonnelSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PersonnelSystem.Views
{
    /// <summary>
    /// Логика взаимодействия для EmployeeView.xaml
    /// </summary>
    public partial class EmployeeView : Window
    {
        public EmployeeView(ObservableCollection<ActualDepartmentDataViewModel> departmentList, ActualDepartmentDataViewModel actualDepartment, EmployeeDataViewModel employeeData = null)
        {
            InitializeComponent();
            DataContext = new EmployeeViewModel(this, departmentList, actualDepartment, employeeData);
        }
    }
}
