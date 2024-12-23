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
            ResponsableTextBox.Text = major.Responsable;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return;

            // Get current major
            var major = (Major)DataContext;

            // Update major properties
            major.Name = NameTextBox.Text;
            major.Description = DescriptionTextBox.Text;
            major.Responsable = ResponsableTextBox.Text;

            // Set dialog result to true
            DialogResult = true;
        }

        private bool ValidateInput()
        {
            bool isValid = true;

            // Validate Name
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                NameError.Text = "Le nom est obligatoire";
                NameError.Visibility = Visibility.Visible;
                isValid = false;
            }
            else
            {
                NameError.Visibility = Visibility.Collapsed;
            }

            // Validate Responsable
            if (string.IsNullOrWhiteSpace(ResponsableTextBox.Text))
            {
                ResponsableError.Text = "Le responsable est obligatoire";
                ResponsableError.Visibility = Visibility.Visible;
                isValid = false;
            }
            else
            {
                ResponsableError.Visibility = Visibility.Collapsed;
            }

            return isValid;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
