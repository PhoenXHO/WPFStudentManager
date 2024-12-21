using StudentManager.Models;
using StudentManager.ViewModels;
using System.Windows;
using Wpf.Ui.Controls;


namespace StudentManager.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for AddStudentDialog.xaml
    /// </summary>
    public partial class AddStudentDialog : FluentWindow
    {
        public Student NewStudent { get; private set; }

        public AddStudentDialog()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (MajorComboBox.SelectedItem is Major selectedMajor)
            {
                NewStudent = new Student
                {
                    FirstName = FirstNameTextBox.Text,
                    LastName = LastNameTextBox.Text,
                    Email = EmailTextBox.Text,
                    Major = selectedMajor, 
                    DateOfBirth = DateOfBirthPicker.SelectedDate
                };
                DialogResult = true;
            }
            else
            {
                System.Windows.MessageBox.Show("Veuillez sélectionner une filière.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void MajorComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
