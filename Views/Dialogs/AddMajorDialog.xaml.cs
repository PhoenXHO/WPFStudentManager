using StudentManager.Models;
using System.Windows;
using Wpf.Ui.Controls;

namespace StudentManager.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for AddMajorDialog.xaml
    /// </summary>
    public partial class AddMajorDialog : FluentWindow
    {
        public Major NewMajor { get; set; }

        public AddMajorDialog()
        {
            InitializeComponent();
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

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return;

            NewMajor = new Major
            {
                Name = NameTextBox.Text,
                Description = DescriptionTextBox.Text,
                Responsable = ResponsableTextBox.Text
            };
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
