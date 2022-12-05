using PersonnelSystem.AppLayer.DataViewModels;
using PersonnelSystem.AppLayer.RequestModels;
using PersonnelSystem.Configuration;
using PersonnelSystem.DomainLayer.Models;
using PersonnelSystem.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PersonnelSystem.ViewModels
{
    public class DepartmentViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ActualDepartmentDataViewModel> _departmentList;
        private ActualDepartmentDataViewModel _departmentData;
        private ObservableCollection<ActualDepartmentDataViewModel> _departments;
        private ActualDepartmentDataViewModel _parentDepartment;
        private string _departmentName;
        private DepartmentView _view;
        private bool _isCreateCommand;

        public DepartmentViewModel(DepartmentView owner, 
                                    bool isCreateCommand, 
                                    ObservableCollection<ActualDepartmentDataViewModel> departmentList, 
                                    ActualDepartmentDataViewModel departmentData = null)
        {
            _view= owner;            
            _isCreateCommand= isCreateCommand;
            _departmentList= departmentList;
            _departmentData= departmentData;                        
            SetDepartments();            
            CreateDepartmentCommand = new CommandHandler(CreateDepartmentRequest);
        }
        public ICommand CreateDepartmentCommand { get; }
        
        public CreateDepartmentRequestModel CreateDepartmentRequestModel { get; private set; }        

        public string DepartmentName
        {
            get { return _departmentName; }
            set
            {
                _departmentName = value;                
                OnPropertyChanged();
            }
        }
        public ActualDepartmentDataViewModel ParentDepartment
        {
            get { return _parentDepartment; }
            set
            {
                _parentDepartment = value;
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
            if (_departmentData != null)
            {
                if (_isCreateCommand)
                {
                    ParentDepartment = Departments.FirstOrDefault(x => x.Id == _departmentData.Id);                                                                        
                }
                else
                {
                    ParentDepartment = Departments.FirstOrDefault(x => x.Id == _departmentData.ParentId);
                    DepartmentName = _departmentData.Name;
                }
                
            }            
        }
        private void SetDepartments()
        {
            if (_departmentList != null)
            {
                Departments = new ObservableCollection<ActualDepartmentDataViewModel>(_departmentList);
                Departments.Add(new ActualDepartmentDataViewModel(null, "(не указано)", null));
                SetParameters();
                if (!_isCreateCommand)
                {
                    Departments.Remove(_departmentData);
                }                
            }            
        }

        private void CreateDepartmentRequest() 
        {
            if (DepartmentName == null)
            {
                MessageBox.Show("Необходимо заполнить все поля!");
            }
            else if (Departments.Any(x => x.Name == DepartmentName))
            {
                MessageBox.Show("Подразделение с таким именем уже существует!");
            }   
            else 
            { 
                
                CreateDepartmentRequestModel = new CreateDepartmentRequestModel();
                CreateDepartmentRequestModel.DepartmentName = DepartmentName;
                CreateDepartmentRequestModel.ParentDepartmentId = ParentDepartment?.Id ?? null;

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
