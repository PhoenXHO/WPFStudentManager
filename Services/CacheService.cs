using StudentManager.Models;
using System.Collections.ObjectModel;

namespace StudentManager.Services
{
    public static class CacheService
    {
        private static ObservableCollection<Student>? _students;
        
        public static ObservableCollection<Student> Students
        {
            get => _students ??= new ObservableCollection<Student>();
            set => _students = value;
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
            get => _majors ??= new ObservableCollection<Major>();
            set => _majors = value;
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
