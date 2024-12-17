using MySql.Data.MySqlClient;
using StudentManager.Models;
using System.Windows;
using Wpf.Ui.Controls;

namespace StudentManager.Views.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : FluentWindow
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            using MySqlConnection? connection = DBConnection.GetConnection();
            try
            {
                // Open the connection
                connection?.Open();

                // Create a new command
                using MySqlCommand command = new("SELECT * FROM users WHERE username = @username AND password = @password", connection);
                command.Parameters.AddWithValue("@username", usernameField.Text);
                command.Parameters.AddWithValue("@password", passwordField.Password);
                command.ExecuteNonQuery();

                // Create a new data reader
                using MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    System.Windows.MessageBox.Show("Login successful!");
                    MainWindow mainWindow = new();
                    mainWindow.Show();
                    Close();
                }
                else
                {
                    System.Windows.MessageBox.Show("Login failed!");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Connection failed: " + ex.Message);
            }
        }
    }
}
