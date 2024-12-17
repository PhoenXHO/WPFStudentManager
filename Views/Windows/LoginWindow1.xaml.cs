using MySql.Data.MySqlClient;
using StudentManager.Models;
using System.Windows;

namespace StudentManager
{
    /// <summary>
    /// Interaction logic for LoginWindow1.xaml
    /// </summary>
    public partial class LoginWindow1 : Window
    {
        public LoginWindow1()
        {
            InitializeComponent();
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            using MySqlConnection connection = new(DBConnection.ConnectionString);
            try
            {
                // Open the connection
                connection.Open();

                // Create a new command
                using MySqlCommand command = new("SELECT * FROM users WHERE username = @username AND password = @password", connection);
                command.Parameters.AddWithValue("@username", usernameField.Text);
                command.Parameters.AddWithValue("@password", passwordField.Password);
                command.ExecuteNonQuery();

                // Create a new data reader
                using MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    MessageBox.Show("Login successful!");
                    MainWindow mainWindow = new();
                    mainWindow.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Login failed!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection failed: " + ex.Message);
            }
        }
    }
}