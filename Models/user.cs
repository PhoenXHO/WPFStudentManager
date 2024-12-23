using MySql.Data.MySqlClient;
using System;
using System.ComponentModel;
using System.Windows;

namespace StudentManager.Models
{
    public class User : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private int _id;
        private string? _username;
        private string? _email;
        private string? _password;

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

        public required string? Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                    UpdateDatabase("Username", value);
                }
            }
        }

        public required string? Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                    UpdateDatabase("Email", value);
                }
            }
        }

        public string? Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                    UpdateDatabase("Password", value); // Consider hashing passwords before saving.
                }
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateDatabase(string propertyName, object? value)
        {
            try
            {
                using var connection = DBConnection.GetConnection();
                connection?.Open();

                var query = $"UPDATE users SET {propertyName} = @Value WHERE Id = @Id";
                using var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@Value", value ?? DBNull.Value);
                command.Parameters.AddWithValue("@Id", Id);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating user: {ex.Message}");
            }
        }
    }
}
