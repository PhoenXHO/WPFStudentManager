using MySql.Data.MySqlClient;
using StudentManager.Models;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using StudentManager.ViewModels;
using StudentManager.Services;

namespace StudentManager.Views.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private User _user;
        private User _tempUser;

        public SettingsPage()
        {
            InitializeComponent();

            // Set the BreadcrumbBar
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.BreadcrumbBar.ItemsSource = new[] { "Paramètres" };
            // Load user details from the database
            _user = LoadUser(MainViewModel.CurrentSession.UserId);

            _tempUser = new User
            {
                Id = _user.Id,
                Username = _user.Username,
                Email = _user.Email,
                Password = _user.Password
            };

            DataContext = _tempUser; // Bind user data to the page's DataContext
        }
        private User LoadUser(int userId)
        {
            try
            {
                using MySqlConnection? connection = DBConnection.GetConnection();
                connection?.Open();
                // Fetch user details from the database
                string query = "SELECT * FROM Users WHERE id = @id";
                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", userId);
                using MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows && reader.Read())
                {
                    return new User
                    {
                        Id = reader.GetInt32("Id"),
                        Username = reader.GetString("Username"),
                        Email = reader.GetString("Email"),
                        Password = reader.GetString("Password")
                    };
                }
                return null; // User not found
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user data: {ex.Message}");
                return null;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                _tempUser.Password = passwordBox.Password;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _tempUser.Password = passwordBox.Password;
            try
            {
                using var connection = DBConnection.GetConnection();
                connection?.Open();

                string query = "UPDATE Users SET Username = @Username, Email = @Email, Password = @Password WHERE Id = @Id";
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", _tempUser.Username);
                command.Parameters.AddWithValue("@Email", _tempUser.Email);
                command.Parameters.AddWithValue("@Password", _tempUser.Password); // Hash the password in a real application
                command.Parameters.AddWithValue("@Id", _tempUser.Id);

                command.ExecuteNonQuery();

                // Update the original user object after saving
                _user.Username = _tempUser.Username;
                _user.Email = _tempUser.Email;
                _user.Password = _tempUser.Password;
                passwordBox.Password = "";

                MessageBox.Show("Changes saved successfully!");

                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving changes: {ex.Message}");
            }
        }

    }
}