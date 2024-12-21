using System.ComponentModel;

namespace StudentManager.Models
{
    public class Student : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private int _id;
        private string? _firstName;
        private string? _lastName;
        private string? _email;
        private Major? _major;
        private DateTime? _dateOfBirth;
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
        public required string? FirstName
		{
			get => _firstName;
			init
			{
				if (_firstName != value)
				{
					_firstName = value;
					OnPropertyChanged(nameof(FirstName));
				}
			}
		}
		public required string? LastName
		{
			get => _lastName;
			init
			{
				if (_lastName != value)
				{
					_lastName = value;
					OnPropertyChanged(nameof(LastName));
				}
			}
		}
		public required string? Email
		{
			get => _email;
			init
			{
				if (_email != value)
				{
					_email = value;
					OnPropertyChanged(nameof(Email));
				}
			}
		}
		public required Major? Major
		{
			get => _major;
			init
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
			init
			{
				if (_dateOfBirth != value)
				{
					_dateOfBirth = value;
					OnPropertyChanged(nameof(DateOfBirth));
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
	}
}
