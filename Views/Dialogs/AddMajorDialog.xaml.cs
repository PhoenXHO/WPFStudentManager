using StudentManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui.Controls;

namespace StudentManager.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for AddMajorDialog.xaml
    /// </summary>
    public partial class AddMajorDialog : FluentWindow
    {
        public Major NewMajor { get; private set; }

        public AddMajorDialog()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NewMajor = new Major
            {
                Name = NameTextBox.Text,
                Description = DescriptionTextBox.Text
            };
            DialogResult = true;
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
