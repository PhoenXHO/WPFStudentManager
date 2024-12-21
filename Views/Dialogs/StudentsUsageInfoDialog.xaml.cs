using System.Windows;
using Wpf.Ui.Controls;

namespace StudentManager.Views.Windows
{
    public partial class StudentsUsageInfoDialog : FluentWindow
    {
        public StudentsUsageInfoDialog()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
