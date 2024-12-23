using MySql.Data.MySqlClient;
using StudentManager.Services;
using System.ComponentModel;
using System.Windows;

namespace StudentManager.Models
{
    public class Student : INotifyPropertyChanged, IEquatable<Student>
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private int _id;
        private string? _firstName;
        private string? _lastName;
        private string? _email;
        private int? _majorId;
        private Major? _major;
        private DateTime? _dateOfBirth;
		private string? _picture;
        private bool _isSelected;

        public int Id
		{
			get => _id;
			set
			{
				if (_id != value)
				{
					_id = value;
					OnPropertyChanged(nameof(Id));
                }
			}
		}
        public string? FirstName
		{
			get => _firstName;
			set
			{
				if (_firstName != value)
				{
					_firstName = value;
					OnPropertyChanged(nameof(FirstName));
                }
			}
		}
		public string? LastName
		{
			get => _lastName;
			set
			{
				if (_lastName != value)
				{
					_lastName = value;
					OnPropertyChanged(nameof(LastName));
                }
			}
		}
		public string? Email
		{
			get => _email;
			set
			{
				if (_email != value)
				{
					_email = value;
					OnPropertyChanged(nameof(Email));
                }
			}
		}
        public int? MajorId
        {
            get => Major?.MajorId;
            set
            {
                if (_majorId != value)
                {
                    _majorId = value;
                    OnPropertyChanged(nameof(MajorId));
                }
            }
        }
        public Major? Major
		{
			get => _major;
			set
			{
				if (_major != value)
				{
					_major = value;
					OnPropertyChanged(nameof(Major));
                }
			}
		}
		public DateTime? DateOfBirth
		{
			get => _dateOfBirth;
			set
			{
				if (_dateOfBirth != value)
				{
					_dateOfBirth = value;
					OnPropertyChanged(nameof(DateOfBirth));
                }
			}
		}
        public string? Picture
        {
            get => _picture;
            set
            {
                if (_picture != value)
                {
                    _picture = value;
                    OnPropertyChanged(nameof(Picture));
                }
            }
        }
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public string FullName => $"{FirstName} {LastName}";

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

        public bool Equals(Student? other)
        {
			if (other is null) return false;
			return Id == other.Id;
        }

		public override bool Equals(object? obj) => Equals(obj as Student);

		public override int GetHashCode() => Id.GetHashCode();

		public static bool operator ==(Student? left, Student? right) => Equals(left, right);

		public static bool operator !=(Student? left, Student? right) => !Equals(left, right);
    }
}
