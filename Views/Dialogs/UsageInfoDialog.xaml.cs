using System.Windows;
using Wpf.Ui.Controls;

namespace StudentManager.Views.Windows
{
    public partial class UsageInfoDialog : FluentWindow
    {
        public UsageInfoDialog()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
