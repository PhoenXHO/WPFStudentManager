using MySql.Data.MySqlClient;
using System.ComponentModel;
using System.Windows;

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
                    UpdateDatabase("Id", value);
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
                    UpdateDatabase("FirstName", value);
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
                    UpdateDatabase("LastName", value);
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
                    UpdateDatabase("Email", value);
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
                    UpdateDatabase("MajorId", value?.Id);
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
                    UpdateDatabase("DateOfBirth", value);
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

		private void UpdateDatabase(string propertyName, object value)
        {
            try
            {
                using var connection = DBConnection.GetConnection();
                connection?.Open();

                var query = $"UPDATE students SET {propertyName} = @Value WHERE Id = @Id";
                using var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@Value", value);
                command.Parameters.AddWithValue("@Id", Id);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la mise à jour de l'étudiant: {ex.Message}");
            }
        }
    }
}
