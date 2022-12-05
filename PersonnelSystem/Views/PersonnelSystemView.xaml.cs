using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using PersonnelSystem.AppLayer.DataViewModels;
using PersonnelSystem.ViewModels;

namespace PersonnelSystem.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///     
    public partial class PersonnelSystemView : Window
    {
        private const string ProcessName = "PersonnelSystem";
        public PersonnelSystemView()
        {
            InitializeComponent();            
        }        

        private void ApplicationClosed(object sender, EventArgs e)
        {
            foreach (Process p in Process.GetProcessesByName(ProcessName))
            {
                p.Kill();
            }
        }

        private void TreeViewItem_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;

            //HiddenTreeSelectedItem.Content = (TreeViewDepartment.SelectedItem as DepartmentDataViewModel).Id;
            item.Focusable = true;
            item.Focus();
            if (item.IsSelected)
            {
                HiddenTreeSelectedItem.Content = (TreeViewDepartment.SelectedItem as DepartmentDataViewModel).Id;
            }
            item.Focusable = false;
            e.Handled = true;
        }
    }
}
