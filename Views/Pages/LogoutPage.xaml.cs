using StudentManager.Views.Windows;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace StudentManager.Views.Pages
{
    public partial class LogoutPage : Page
    {
        public LogoutPage()
        {
            InitializeComponent();
        }

        public static void Logout(Window currentWindow)
        {
            // Delete the session file if it exists
            if (File.Exists("session.json"))
            {
                File.Delete("session.json");
            }

            // Create and show login window before closing the current window
            LoginWindow loginWindow = new();
            loginWindow.Show();
            currentWindow.Close();
        }
    }
}
