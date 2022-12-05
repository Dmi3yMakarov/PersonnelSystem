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
    /// Логика взаимодействия для DepartmentView.xaml
    /// </summary>
    public partial class DepartmentView : Window
    {
        public DepartmentView(bool isCreatedData, ObservableCollection<ActualDepartmentDataViewModel> departmentList, ActualDepartmentDataViewModel departmentData = null)
        {
            InitializeComponent();
            DataContext = new DepartmentViewModel(this, isCreatedData, departmentList, departmentData);
        }
    }
}
