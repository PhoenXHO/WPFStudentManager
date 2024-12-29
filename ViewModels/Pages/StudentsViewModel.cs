using GalaSoft.MvvmLight;
using StudentManager.Models;
using StudentManager.Services;
using System.Collections.ObjectModel;
using System.Windows;
using StudentManager.DataAccess;

namespace StudentManager.ViewModels.Pages
{
    public class StudentsViewModel : ViewModelBase
    {
        private ObservableCollection<Student> _students;
        public ObservableCollection<Student> Students
        {
            get => _students;
            private set
            {
                _students = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<Major> MajorsWithAll { get; set; }
        private ObservableCollection<Student> _selectedStudents;
        public ObservableCollection<Student> SelectedStudents
        {
            get => _selectedStudents;
            private set
            {
                _selectedStudents = value;
                RaisePropertyChanged();
            }
        }

        private Major? _selectedMajor;
        public Major? SelectedMajor
        {
            get => _selectedMajor;
            set
            {
                _selectedMajor = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsMajorSelected));
                RaisePropertyChanged(nameof(IsMajorNotSelected));
            }
        }

        public bool IsMajorSelected => _selectedMajor != null && _selectedMajor.Name != "Tout";
        public bool IsMajorNotSelected => !IsMajorSelected;

        public StudentsViewModel(MajorsViewModel majorsViewModel)
        {
            _students = [];
            _selectedStudents = [];

            MajorsWithAll = majorsViewModel.MajorsWithAll;

            //_ = UpdateStudentsAsync(); // Load students asynchronously
        }

        public async Task UpdateStudentsAsync()
        {
            try
            {
                CacheService.Students = new(await DatabaseRepository.GetAllStudentsAsync());
                Students = new(CacheService.Students);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des étudiants: {ex.Message}");
            }
        }

        public void AddStudent(Student student)
        {
            CacheService.Students.Add(student);
            Students.Add(student);
        }

        public void RemoveStudent(Student student)
        {
            CloudinaryService.DeleteImage(student.Picture);
            CacheService.Students.Remove(student);
            Students.Remove(student);
            RemoveSelectedStudent(student);
        }

        public void AddSelectedStudent(Student student)
        {
            if (!SelectedStudents.Contains(student))
            {
                SelectedStudents.Add(student);
                RaisePropertyChanged(nameof(SelectedStudents));
            }
        }

        public void RemoveSelectedStudent(Student student)
        {
            if (SelectedStudents.Contains(student))
            {
                SelectedStudents.Remove(student);
                RaisePropertyChanged(nameof(SelectedStudents));
            }
        }

        public void FilterStudents(string searchText = "", Major? selectedMajor = null)
        {
            var filteredStudents = CacheService.Students.Where(s =>
                (string.IsNullOrEmpty(searchText) ||
                 s.FirstName.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                 s.LastName.Contains(searchText, StringComparison.OrdinalIgnoreCase)) &&
                (selectedMajor == null || selectedMajor.Name == "Tout" ||
                 s.Major?.Name == selectedMajor.Name));

            Students = new ObservableCollection<Student>(filteredStudents);
        }
    }
}