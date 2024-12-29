using StudentManager.Models;
using System.Collections.ObjectModel;

namespace StudentManager.Services
{
    public static class CacheService
    {
        public static event Action? StudentsChanged;
        public static event Action? MajorsChanged;

        private static ObservableCollection<Student>? _students;
        
        public static ObservableCollection<Student> Students
        {
            get => _students ??= [];
            set
            {
                _students = value;
                StudentsChanged?.Invoke();
            }
        }

        public static void ClearStudentsCache()
        {
            Students.Clear();
        }

        public static void InvalidateStudentsCache()
        {
            _students = null;
        }

        private static ObservableCollection<Major>? _majors;
        
        public static ObservableCollection<Major> Majors
        {
            get => _majors ??= [];
            set
            {
                _majors = value;
                MajorsChanged?.Invoke();
            }
        }

        public static void ClearMajorsCache()
        {
            Majors.Clear();
        }

        public static void InvalidateMajorsCache()
        {
            _majors = null;
        }
    }
}
