using PersonnelSystem.AppLayer.DataViewModels;
using PersonnelSystem.AppLayer.RequestModels;
using PersonnelSystem.Configuration;
using PersonnelSystem.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace PersonnelSystem.ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        private string _patronymic;
        private ActualDepartmentDataViewModel _department;
        private EmployeeDataViewModel _employeeData;
        private ObservableCollection<ActualDepartmentDataViewModel> _departmentList;
        private ObservableCollection<ActualDepartmentDataViewModel> _departments;
        private EmployeeView _view;
        private ActualDepartmentDataViewModel _actualDepartment;

        public EmployeeViewModel(EmployeeView owner, 
            ObservableCollection<ActualDepartmentDataViewModel> departmentList, 
            ActualDepartmentDataViewModel actualDepartment, 
            EmployeeDataViewModel employeeData = null)
        {
            _view = owner;
            _employeeData = employeeData;
            _departmentList = departmentList;
            _actualDepartment= actualDepartment;
            Departments = new ObservableCollection<ActualDepartmentDataViewModel>(_departmentList);
            SetParameters();
            CreateEmployeeCommand = new CommandHandler(CreateEmployeeRequest);
        }
        public ICommand CreateEmployeeCommand { get; }        
        private string Id { get; set; }

        public CreateEmployeeRequestModel EmployeeRequestModel { get; private set; }
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }
        public string Patronymic
        {
            get { return _patronymic; }
            set
            {
                _patronymic = value;
                OnPropertyChanged();
            }
        }
        public ActualDepartmentDataViewModel Department
        {
            get { return _department; }
            set
            {
                _department = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ActualDepartmentDataViewModel> Departments
        {
            get { return _departments; }
            set
            {
                _departments = value;
                OnPropertyChanged();
            }
        }

        private void SetParameters()
        {
            if(_employeeData != null)
            {
                Id = _employeeData.Id;
                FullNameDecomposition();                
            }
            Department = Departments.Contains(_actualDepartment) ? _actualDepartment : Departments.FirstOrDefault();
        }

        private void FullNameDecomposition()
        {
            string[] words = _employeeData.FullName.Split(' ');
            FirstName = words[0];
            LastName = words[1];
            Patronymic= words.Length == 3 ? words[2] : null;
        }

        private void CreateEmployeeRequest()
        {
            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
            {
                MessageBox.Show("Необходимо заполнить все поля!");
            }
            else
            {
                EmployeeRequestModel = new CreateEmployeeRequestModel();
                EmployeeRequestModel.FirstName = FirstName;
                EmployeeRequestModel.LastName = LastName;
                EmployeeRequestModel.Patronymic = Patronymic;
                EmployeeRequestModel.DepartmentId = Department.Id;
                _view.Close();
            }            
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
