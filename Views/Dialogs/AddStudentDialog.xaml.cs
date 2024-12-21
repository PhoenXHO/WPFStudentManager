using StudentManager.Models;
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
            NewStudent = new Student
            {
                FirstName = FirstNameTextBox.Text,
                LastName = LastNameTextBox.Text,
                Email = EmailTextBox.Text,
                Major = (Major)MajorComboBox.SelectedItem,
                DateOfBirth = DateOfBirthPicker.SelectedDate
            };
            DialogResult = true;
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
