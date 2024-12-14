using System.Windows;

namespace StudentManager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StatistiquesButton.Click += StatistiquesButton_Click;
            Dashboard.Click+= DashboardButton_Click;
            // Add handlers for other buttons (Dashboard, Gestion Étudiants)
        }

        private void StatistiquesButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the Statistiques page
            MainContentFrame.Navigate(new Statistiques());
        }
        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the Statistiques page
            MainContentFrame.Navigate(new Dashboard());
        }

        // Add similar methods for other buttons if needed
    }
}
