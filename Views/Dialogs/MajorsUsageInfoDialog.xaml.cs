using System.Windows;
using Wpf.Ui.Controls;

namespace StudentManager.Views.Windows
{
    public partial class MajorsUsageInfoDialog : FluentWindow
    {
        public MajorsUsageInfoDialog()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
