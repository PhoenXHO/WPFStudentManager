using StudentManager.Models;
using System.Windows;
using Wpf.Ui.Controls;

namespace StudentManager.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for EditMajorDialog.xaml
    /// </summary>
    public partial class EditMajorDialog : FluentWindow
    {
        public EditMajorDialog()
        {
            InitializeComponent();

            Loaded += EditMajorDialog_Loaded;

            // Set focus to the name text box
            NameTextBox.Focus();
        }

        private void EditMajorDialog_Loaded(object sender, RoutedEventArgs e)
        {
            // Get current major
            var major = (Major)DataContext;

            // Set text box values
            NameTextBox.Text = major.Name;
            DescriptionTextBox.Text = major.Description;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Get current major
            var major = (Major)DataContext;

            // Update major properties
            major.Name = NameTextBox.Text;
            major.Description = DescriptionTextBox.Text;

            // Set dialog result to true
            DialogResult = true;
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
