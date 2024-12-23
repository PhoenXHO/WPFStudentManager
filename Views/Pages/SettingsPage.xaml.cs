using MySql.Data.MySqlClient;
using StudentManager.Models;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using StudentManager.ViewModels;

namespace StudentManager.Views.Pages
{
    public partial class SettingsPage : Page
    {
        private User _user;

        public SettingsPage()
        {
            InitializeComponent();

            // Set the BreadcrumbBar
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.BreadcrumbBar.ItemsSource = new[] { "Paramètres" };

            // Load user details from the database
            _user = LoadUser(MainViewModel.CurrentSession.UserId);
            DataContext = _user; // Bind user data to the page's DataContext
        }

        private User LoadUser(int userId)
        {
            try
            {
                using MySqlConnection? connection = DBConnection.GetConnection();
                connection?.Open();

                // Fetch user details from the database
                string query = "SELECT * FROM users WHERE id = @id";
                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", userId);

                using MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows && reader.Read())
                {
                    return new User
                    {
                        Id = reader.GetInt32("id"),
                        Username = reader.GetString("username"),
                        Email = reader.GetString("email"),
                        Password = reader.GetString("password")
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
            var passwordBox = sender as PasswordBox;

            if (passwordBox != null)
            {
                // Mettre à jour la propriété Password de l'objet User
                _user.Password = passwordBox.Password;
            }
            else
            {
                Debug.WriteLine("PasswordBox est nul.");
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Save the changes to the user information
            _user.Username = usernameTextBox.Text;
            _user.Email = emailTextBox.Text;
            _user.Password = passwordBox.Password;

            MessageBox.Show("Changes saved successfully!");
        }
    }
}

