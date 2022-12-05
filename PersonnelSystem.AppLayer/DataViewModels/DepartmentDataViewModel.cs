using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PersonnelSystem.AppLayer.DataViewModels
{
    public record DepartmentDataViewModel(string Id, string DepartmentName, ObservableCollection<DepartmentDataViewModel>? ChildDepartments = null) : INotifyPropertyChanged
    {
        private bool _isExpanded;
        private bool _isSelected;

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                OnPropertyChanged();
            }
        }
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
