using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using PersonnelSystem.AppLayer.DataViewModels;
using PersonnelSystem.AppLayer.Handlers;
using PersonnelSystem.AppLayer.Interfaces;
using PersonnelSystem.AppLayer.RequestModels;
using PersonnelSystem.Configuration;
using PersonnelSystem.Views;

namespace PersonnelSystem.ViewModels
{
    public class PersonnelSystemViewModel : INotifyPropertyChanged
    {           
        private ObservableCollection<EmployeeDataViewModel> _depCardEmployees;
        private ObservableCollection<EmployeeDataViewModel> _empCardEmployees;
        private EmployeeDataViewModel _empCardEmployee;
        private EmployeeDataViewModel _depCardEmployee;
        private bool _periodIsChecked;
        private DateTime _departmentDate;
        private DateTime _employeeFromDate;
        private DateTime _employeeToDate;
        private ObservableCollection<DepartmentDataViewModel> _treeDepartments;        
        private ObservableCollection<ActualDepartmentDataViewModel> _departments;
        private ActualDepartmentDataViewModel _department;        
        private DepartmentDataViewModel _treeDepartment;
        private bool _employeeDateIsChanged;
        private bool _isToDayDep;
        private bool _isToDayEmp;
        private IServiceProvider _serviceProvider;                
        private string _treeSelectedItemId;        
        private DateTime _toDay;

        public PersonnelSystemViewModel(IServiceProvider serviceProvider)
        {               
            _serviceProvider = serviceProvider;
            
            SetParameters();
            
            CreateDepartmentCommand = new CommandHandler(CreateDepartment);
            EditDepartmentCommand = new CommandHandler(EditDepartment);
            DeleteDepartmentCommand = new CommandHandler(DeleteDepartment);
            CreateEmployeeCommand = new CommandHandler(CreateEmployee);
            EditEmployeeCommand = new CommandHandler(EditEmployee);
            DeleteEmployeeCommand = new CommandHandler(DeleteEmployee);
            ShowEmployeeListCommand = new CommandHandler(ShowEmployeeForPeriod);
            //SelectedItemChangedCommand = new CommandHandler(SelectedItemChanged);

            IsLoad = true;            
        }

        protected THandler GetHandler<THandler>() where THandler : class
        {
            return _serviceProvider.GetRequiredService<THandler>();
        }

        public ICommand CreateDepartmentCommand { get; }
        public ICommand EditDepartmentCommand { get; }
        public ICommand DeleteDepartmentCommand { get; }        
        public ICommand CreateEmployeeCommand { get; }
        public ICommand EditEmployeeCommand { get; }
        public ICommand DeleteEmployeeCommand { get; }        
        public ICommand ShowEmployeeListCommand { get; }
        public ICommand SelectedItemChangedCommand { get; }

        public bool IsLoad { get; set; }

        public string TreeSelectedItemId
        {
            get { return _treeSelectedItemId; }
            set
            {
                _treeSelectedItemId = value;
                if(_treeSelectedItemId != null)
                {
                    ShowEmployeeForDepartment();
                }
                OnPropertyChanged();
            }
        }        
        public DepartmentDataViewModel TreeDepartment
        {
            get { return _treeDepartment; }
            set
            {                
                _treeDepartment = value;
                if(TreeDepartment != null)
                {
                    TreeSelectedItemId = _treeDepartment.Id;
                }                
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
                ShowEmployeeForPeriod();
            }
        }
        public ObservableCollection<DepartmentDataViewModel> TreeDepartments
        {
            get { return _treeDepartments; }
            set
            {
                _treeDepartments = value;
                if (_treeDepartments.Any())
                {
                    TreeDepartment = _treeDepartments.FirstOrDefault();
                }
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
        

        public ObservableCollection<EmployeeDataViewModel> DepCardEmployees
        {
            get { return _depCardEmployees; }
            set
            {
                _depCardEmployees = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<EmployeeDataViewModel> EmpCardEmployees
        {
            get { return _empCardEmployees; }
            set
            {
                _empCardEmployees = value;
                OnPropertyChanged();
            }
        }
        public EmployeeDataViewModel EmpCartEmployee
        {
            get { return _empCardEmployee; }
            set
            {
                _empCardEmployee = value;
                OnPropertyChanged();
            }
        }
        public EmployeeDataViewModel DepCartEmployee
        {
            get { return _depCardEmployee; }
            set
            {
                _depCardEmployee = value;
                OnPropertyChanged();
            }
        }
        public bool PeriodIsChecked
        {
            get { return _periodIsChecked; }
            set
            {
                _periodIsChecked = value;
                OnPropertyChanged();
            }
        }
        public bool EmployeeDateIsChanged
        {
            get { return _employeeDateIsChanged; }
            set
            {
                _employeeDateIsChanged = value;
                OnPropertyChanged();
            }
        }
        public bool IsToDayDep
        {
            get { return _isToDayDep; }
            set
            {
                _isToDayDep = value;
                OnPropertyChanged();
            }
        }
        public bool IsToDayEmp
        {
            get { return _isToDayEmp; }
            set
            {
                _isToDayEmp = value;
                OnPropertyChanged();
            }
        }        
        public DateTime DepartmentDate
        {
            get { return _departmentDate; }
            set
            {
                _departmentDate = value;
                OnPropertyChanged();
                IsToDayDep = _departmentDate.Date == ToDay;
                ShowDepartmentsStructureForDate();
                ShowEmployeeForDepartment();
            }
        }
        public DateTime EmployeeFromDate
        {
            get { return _employeeFromDate; }
            set
            {
                _employeeFromDate = value;
                OnPropertyChanged();
                IsToDayEmp = _employeeFromDate.Date == ToDay;
                if (_employeeFromDate > EmployeeToDate)
                {
                    EmployeeToDate = _employeeFromDate;
                }                    
            }
        }
        public DateTime EmployeeToDate
        {
            get { return _employeeToDate; }
            set
            {
                _employeeToDate = value;
                OnPropertyChanged();
                if (_employeeToDate < EmployeeFromDate)
                    EmployeeFromDate = _employeeToDate;
            }
        }
        public DateTime ToDay
        {
            get { return _toDay; }
            set
            {
                _toDay = value;
                OnPropertyChanged();                
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
               

        private void SetParameters()
        {
            ToDay = DateTime.Today;
            DepartmentDate = DateTime.Now;
            EmployeeFromDate = DateTime.Now;
            EmployeeToDate = DateTime.Now;
            RefreshActualDepartment();
            ShowEmployeeForPeriod();            
            ShowEmployeeForDepartment();
        }

        private void RefreshActualDepartment()
        {
            try
            {
                Departments = GetHandler<GetActualDepartmentListHandler>().Execute().ActualDepartments;
                Department = Departments.Contains(Department) ? Department : Departments.FirstOrDefault();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
                        
        }
              

        private void CreateEmployee()
        {
            if(!Departments.Any())
            {
                MessageBox.Show("Список подразделений пуст, необходимо добавить хотя бы одно подразделение!");
            }
            else
            {
                EmployeeView employeeView = new EmployeeView(Departments, Department);
                if (employeeView.ShowDialog() == true)
                {
                    
                }
                if ((employeeView.DataContext as EmployeeViewModel).EmployeeRequestModel != null)
                {
                    try
                    {
                        CreateEmployeeRequestModel createEmployeeRequestModel = (employeeView.DataContext as EmployeeViewModel).EmployeeRequestModel;
                        GetHandler<CreateEmployeeHandler>().Execute(createEmployeeRequestModel);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    
                    
                    
                SetParameters();
                }
            }            
        }
        private void EditEmployee() 
        {
            if(!Departments.Any())
            {
                MessageBox.Show("Список подразделений пуст, необходимо добавить хотя бы одно подразделение!");
            }
            else if (EmpCartEmployee == null)
            {
                MessageBox.Show("Список сотрудников пуст!");
            }
            else
            {
                EmployeeView employeeView = new EmployeeView(Departments, Department, EmpCartEmployee);
                if (employeeView.ShowDialog() == true)
                {

                }
                if ((employeeView.DataContext as EmployeeViewModel).EmployeeRequestModel != null)
                {
                    EditEmployeeRequestModel editEmployeeRequestModel = new EditEmployeeRequestModel();
                    editEmployeeRequestModel.Id = EmpCartEmployee.Id;
                    editEmployeeRequestModel.FirstName = (employeeView.DataContext as EmployeeViewModel).EmployeeRequestModel.FirstName;
                    editEmployeeRequestModel.LastName = (employeeView.DataContext as EmployeeViewModel).EmployeeRequestModel.LastName;
                    editEmployeeRequestModel.Patronymic = (employeeView.DataContext as EmployeeViewModel).EmployeeRequestModel.Patronymic;
                    editEmployeeRequestModel.DepartmentId = (employeeView.DataContext as EmployeeViewModel).EmployeeRequestModel.DepartmentId;

                    try
                    {
                        GetHandler<EditEmployeeHandler>().Execute(editEmployeeRequestModel);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    
                    SetParameters();
                }
                    
            }            
        }
        private void DeleteEmployee() 
        {
            if(EmpCartEmployee == null)
            {
                MessageBox.Show("Список сотрудников пуст!");
            }
            else
            {
                DeleteEmployeeRequestModel deleteEmployeeRequestModel = new DeleteEmployeeRequestModel
                {
                    Id = EmpCartEmployee.Id 
                };
                try
                {
                    GetHandler<DeleteEmployeeHandler>().Execute(deleteEmployeeRequestModel);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
                SetParameters();
            }            
        }
        private void CreateDepartment()
        {            
            DepartmentView departmentView = new DepartmentView(true, Departments, Departments?.FirstOrDefault(x => x.Id == TreeSelectedItemId));
            if (departmentView.ShowDialog() == true)
            {

            }
            if((departmentView.DataContext as DepartmentViewModel).CreateDepartmentRequestModel != null)
            {                 
                CreateDepartmentRequestModel createDepartmentRequestModel = (departmentView.DataContext as DepartmentViewModel).CreateDepartmentRequestModel;                
                GetHandler<CreateDepartmentHandler>().Execute(createDepartmentRequestModel);                
                SetParameters();
            }            
        }
        private void EditDepartment()
        {               
            if (!Departments.Any() || TreeSelectedItemId == null)
            {
                MessageBox.Show("Список подразделений пуст!");
            }
            else
            {
                DepartmentView departmentView = new DepartmentView(false, Departments, Departments.FirstOrDefault(x => x.Id == TreeSelectedItemId));
                if (departmentView.ShowDialog() == true)
                {

                }
                if ((departmentView.DataContext as DepartmentViewModel).CreateDepartmentRequestModel != null)
                {
                    EditDepartmentRequestModel editDepartmentRequestModel = new EditDepartmentRequestModel();
                    editDepartmentRequestModel.Id = TreeSelectedItemId;
                    editDepartmentRequestModel.DepartmentName = (departmentView.DataContext as DepartmentViewModel).CreateDepartmentRequestModel.DepartmentName;
                    editDepartmentRequestModel.ParentDepartmentId = (departmentView.DataContext as DepartmentViewModel).CreateDepartmentRequestModel.ParentDepartmentId;
                    try
                    {
                        GetHandler<EditDepartmentHandler>().Execute(editDepartmentRequestModel);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                                   
                    SetParameters();
                }                
            }            
        }
                
        private void DeleteDepartment()
        {
            if (!Departments.Any() || TreeSelectedItemId == null)
            {
                MessageBox.Show("Список подразделений пуст!");
            }            
            else
            {
                DeleteDepartmentRequestModel deleteDepartmentRequestModel = new DeleteDepartmentRequestModel();
                deleteDepartmentRequestModel.Id = TreeSelectedItemId;
                try
                {
                    GetHandler<DeleteDepartmentHandler>().Execute(deleteDepartmentRequestModel);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
                SetParameters();
            }                
        }
        private void ShowEmployeeForPeriod()
        {
            if(Department != null)
            {
                DateEmployeeDepartmentRequestModel dateEmployeeDepartmentRequest = new DateEmployeeDepartmentRequestModel();
                dateEmployeeDepartmentRequest.FromDateTime = EmployeeFromDate;
                dateEmployeeDepartmentRequest.ToDateTime = EmployeeToDate;
                dateEmployeeDepartmentRequest.DepartmentId = Department.Id;
                try
                {
                    var employeeList = GetHandler<GetEmployeeListForPeriodHandler>().Execute(dateEmployeeDepartmentRequest);
                    EmpCardEmployees = new ObservableCollection<EmployeeDataViewModel>(employeeList.Employees);
                    EmpCartEmployee = EmpCardEmployees.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                                
            }            
        }
        private void ShowEmployeeForDepartment()
        {
            if (TreeDepartments.Any())
            {
                DateEmployeeDepartmentRequestModel dateEmployeeDepartmentRequest = new DateEmployeeDepartmentRequestModel();
                dateEmployeeDepartmentRequest.FromDateTime = DepartmentDate;
                dateEmployeeDepartmentRequest.ToDateTime = DepartmentDate;
                dateEmployeeDepartmentRequest.DepartmentId = TreeSelectedItemId;
                try
                {
                    DepCardEmployees = new ObservableCollection<EmployeeDataViewModel>(GetHandler<GetEmployeeListForPeriodHandler>()
                                                                                        .Execute(dateEmployeeDepartmentRequest).Employees);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
                DepCartEmployee = DepCardEmployees.FirstOrDefault();
            }
        }
        
        private void ShowDepartmentsStructureForDate()
        {
            DateDepartmentStructureRequestModel dateDepartmentStructureRequest = new DateDepartmentStructureRequestModel { SelectedDateTime = DepartmentDate };
            try
            {
                TreeDepartments = new ObservableCollection<DepartmentDataViewModel>(GetHandler<GetDepartmentListForDateHandler>()
                                                                                        .Execute(dateDepartmentStructureRequest).Departments);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            TreeDepartment = TreeDepartments.FirstOrDefault(x => x.IsSelected);
        }       

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
