using GalaSoft.MvvmLight;
using MySql.Data.MySqlClient;
using StudentManager.Models;
using StudentManager.Services;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using StudentManager.DataAccess;

namespace StudentManager.ViewModels.Pages
{
    public class StudentsViewModel : ViewModelBase
    {
        public ObservableCollection<Student> Students { get; }
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
            // Create a new ObservableCollection with the 'All' item at the beginning
            MajorsWithAll = new ObservableCollection<Major>(majorsViewModel.Majors);
            MajorsWithAll.Insert(0, new Major { MajorId = 0, Name = "Tout", Description = "All Majors", Responsable = "All" });

            // Initialisation de la liste des étudiants
            Students = new ObservableCollection<Student>();
            SelectedStudents = new ObservableCollection<Student>();
            _ = LoadStudentsAsync(); // Load students asynchronously
            _ = LoadMajorsAsync(); // Load majors asynchronously
        }

        private async Task LoadStudentsAsync()
        {
            try
            {
                var students = await DatabaseRepository.GetAllStudentsAsync();
                foreach (var student in students)
                {
                    Students.Add(student);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des étudiants: {ex.Message}");
            }
        }

        private async Task LoadMajorsAsync()
        {
            try
            {
                var majors = await DatabaseRepository.GetAllMajorsAsync();
                foreach (var major in majors)
                {
                    MajorsWithAll.Add(major);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des majeures: {ex.Message}");
            }
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
    }
}