using System.ComponentModel;

namespace StudentManager.Models
{
    public class Stats : INotifyPropertyChanged
    {
        private string _major;
        private int _studentCount;

        public string Major
        {
            get => _major;
            set
            {
                _major = value;
                OnPropertyChanged(nameof(Major));
            }
        }

        public int StudentCount
        {
            get => _studentCount;
            set
            {
                _studentCount = value;
                OnPropertyChanged(nameof(StudentCount));
            }
        }

        public float Y { get { return 0.5f; } set { } }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
