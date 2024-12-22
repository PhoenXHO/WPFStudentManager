using StudentManager.ViewModels;
using StudentManager.ViewModels.Pages;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using StudentManager.Models;
using StudentManager.Views.Windows;

namespace StudentManager.Views.Pages
{
    /// <summary>
    /// Interaction logic for StudentsPage.xaml
    /// </summary>
    public partial class StudentsPage : Page
    {
        private bool _isShiftPressed;
        private int _lastSelectedIndex = -1;

        public StudentsPage()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var mainViewModel = (MainViewModel)DataContext;
            var dialog = new Dialogs.AddStudentDialog
            {
                DataContext = mainViewModel.MajorsViewModel
            };

            if (dialog.ShowDialog() == true)
            {

                var student = dialog.NewStudent;
                AddStudentToDatabase(student);
                mainViewModel.StudentsViewModel.Students.Add(student);
            }
        }

        private static void AddStudentToDatabase(Student student)
        {
            try
            {
                using var connection = DBConnection.GetConnection();
                connection?.Open();
                var query = @"INSERT INTO students (FirstName, LastName, Email, MajorId, DateOfBirth)
                              VALUES (@FirstName, @LastName, @Email, @MajorId, @DateOfBirth)";
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", student.FirstName);
                command.Parameters.AddWithValue("@LastName", student.LastName);
                command.Parameters.AddWithValue("@Email", student.Email);
                command.Parameters.AddWithValue("@MajorId", student.Major?.Id);
                command.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);

                command.ExecuteNonQuery();
                long id=command.LastInsertedId;
                student.Id=(int)id;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ajout de l'étudiant : {ex.Message}");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Voulez-vous vraiment supprimer l'étudiant sélectionné(s) ?",
                "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                return;
            }

            var mainViewModel = (MainViewModel)DataContext;
            var selectedStudents = mainViewModel.StudentsViewModel.Students.Where(s => s.IsSelected).ToList();
            foreach (var student in selectedStudents)
            {
                // Remove the student from the database
                DeleteStudentFromDatabase(student);
                // Remove the student from the Students collection
                mainViewModel.StudentsViewModel.Students.Remove(student);
            }

            // Uncheck the Select All checkbox if all students are unselected
            if (mainViewModel.StudentsViewModel.SelectedStudents.Count == 0)
            {
                SelectAllCheckBox.IsChecked = false;
            }
        }

        private static void DeleteStudentFromDatabase(Student student)
        {
            try
            {
                using var connection = DBConnection.GetConnection();
                connection?.Open();
                var query = "DELETE FROM students WHERE Id = @Id";
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", student.Id);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la suppression de l'étudiant : {ex.Message}");
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var student = (Student)((CheckBox)sender).DataContext;
            student.IsSelected = true;
            // Set the last selected index to the current index
            _lastSelectedIndex = ((DataGridRow)UIDataGrid.ItemContainerGenerator.ContainerFromItem(student)).GetIndex();
            // Raise the PropertyChanged event for SelectedStudents
            ((MainViewModel)DataContext).StudentsViewModel.RaisePropertyChanged(nameof(StudentsViewModel.SelectedStudents));
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var student = (Student)((CheckBox)sender).DataContext;
            student.IsSelected = false;
            // Raise the PropertyChanged event for SelectedStudents
            ((MainViewModel)DataContext).StudentsViewModel.RaisePropertyChanged(nameof(StudentsViewModel.SelectedStudents));
        }

        private void SelectAllCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var mainViewModel = (MainViewModel)DataContext;
            for (int i = 0; i < mainViewModel.StudentsViewModel.Students.Count; i++)
            {
                mainViewModel.StudentsViewModel.Students[i].IsSelected = true;
            }
            // Raise the PropertyChanged event for SelectedStudents
            mainViewModel.StudentsViewModel.RaisePropertyChanged(nameof(StudentsViewModel.SelectedStudents));
        }

        private void SelectAllCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var mainViewModel = (MainViewModel)DataContext;
            for (int i = 0; i < mainViewModel.StudentsViewModel.Students.Count; i++)
            {
                mainViewModel.StudentsViewModel.Students[i].IsSelected = false;
            }
            // Raise the PropertyChanged event for SelectedStudents
            mainViewModel.StudentsViewModel.RaisePropertyChanged(nameof(StudentsViewModel.SelectedStudents));
        }

        private void DataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_isShiftPressed)
            {
                var dataGrid = sender as DataGrid;
                if (ItemsControl.ContainerFromElement(dataGrid, e.OriginalSource as DependencyObject) is DataGridRow row)
                {
                    int currentIndex = row.GetIndex();
                    if (_lastSelectedIndex >= 0)
                    {
                        int start = Math.Min(_lastSelectedIndex, currentIndex);
                        int end = Math.Max(_lastSelectedIndex, currentIndex);
                        int offset = (start <= end) ? 1 : -1; // Skip the last selected index
                        for (int i = start + offset; i <= end; i++)
                        {
                            if (dataGrid?.Items[i] is Student student)
                            {
                                student.IsSelected = !student.IsSelected;
                            }
                        }
                    }
                    _lastSelectedIndex = currentIndex;
                    
                    e.Handled = true;
                }
            }
        }

        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                _isShiftPressed = true;
            }
        }

        private void DataGrid_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                _isShiftPressed = false;
            }
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            var student = (Student)((Button)sender).DataContext;
            MessageBox.Show($"Viewing details for {student.FirstName} {student.LastName}");
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var mainViewModel = (MainViewModel)DataContext;
            //var filter = SearchTextBox.Text;
            var filter = '%' + SearchTextBox.Text + '%';

            try
            {
                using var connection = DBConnection.GetConnection();
                connection?.Open();
                var query = @"SELECT students.Id, students.FirstName, students.LastName, students.Email, 
                              students.MajorId, students.DateOfBirth, majors.Name as MajorName, majors.Description as MajorDescription , majors.Responsable as Responsable
                              FROM students 
                              LEFT JOIN majors ON students.MajorId = majors.Id 
                              WHERE CAST(students.Id AS CHAR) LIKE @filter 
                              OR students.FirstName LIKE @filter 
                              OR students.LastName LIKE @filter";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@filter", $"{filter}");

                using var reader = command.ExecuteReader();
                var students = new List<Student>();

                while (reader.Read())
                {
                    students.Add(new Student
                    {
                        Id = reader.GetInt32("Id"),
                        FirstName = reader.GetString("FirstName"),
                        LastName = reader.GetString("LastName"),
                        Email = reader.GetString("Email"),
                        Major = new Major
                        {
                            Id = reader.GetInt32("MajorId"),
                            Name = reader.GetString("MajorName"),
                            Description = reader.GetString("MajorDescription"),
                            Responsable = reader.GetString("Responsable")
                        },
                        DateOfBirth = reader.GetDateTime("DateOfBirth")
                    });
                }

                // Update the DataGrid's ItemsSource
                //UIDataGrid.ItemsSource = students;
                // Clear and update the Students collection instead of replacing the ItemsSource
                mainViewModel.StudentsViewModel.Students.Clear();
                foreach (var student in students)
                {
                    mainViewModel.StudentsViewModel.Students.Add(student);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la recherche : {ex.Message}");
            }
        }

        private void MajorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var mainViewModel = (MainViewModel)DataContext;
            var selectedMajor = MajorComboBoxSe.SelectedItem as Major;

            try
            {
                using var connection = DBConnection.GetConnection();
                connection?.Open();
                string query;
                MySqlCommand command;

                // If "Tout" is selected, show all students, regardless of major
                if (MajorComboBoxSe.SelectedIndex == 0 || (selectedMajor != null && selectedMajor.Name == "Tout"))
                {
                    query = @"SELECT students.Id, students.FirstName, students.LastName, students.Email, 
                              students.MajorId, students.DateOfBirth, majors.Name as MajorName, majors.Description as MajorDescription , majors.Responsable as Responsable
                              FROM students 
                              LEFT JOIN majors ON students.MajorId = majors.Id"; // No filter
                    command = new MySqlCommand(query, connection);
                }
                // If a major is selected, filter the students by major
                else
                {
                    query = @"SELECT students.Id, students.FirstName, students.LastName, students.Email, 
                              students.MajorId, students.DateOfBirth, majors.Name as MajorName, majors.Description as MajorDescription , majors.Responsable as Responsable
                              FROM students 
                              LEFT JOIN majors ON students.MajorId = majors.Id 
                              WHERE majors.Name = @MajorName"; // Filter by major name
                    command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MajorName", selectedMajor.Name);
                }

                using var reader = command.ExecuteReader();
                var students = new List<Student>();

                while (reader.Read())
                {
                    students.Add(new Student
                    {
                        Id = reader.GetInt32("Id"),
                        FirstName = reader.GetString("FirstName"),
                        LastName = reader.GetString("LastName"),
                        Email = reader.GetString("Email"),
                        Major = new Major
                        {
                            Id = reader.GetInt32("MajorId"),
                            Name = reader.GetString("MajorName"),
                            Description = reader.GetString("MajorDescription"),
                            Responsable = reader.GetString("Responsable") 
                        }
,
                        DateOfBirth = reader.GetDateTime("DateOfBirth")
                    });
                }

                // Update the DataGrid's ItemsSource
                mainViewModel.StudentsViewModel.Students.Clear();
                foreach (var student in students)
                {
                    mainViewModel.StudentsViewModel.Students.Add(student);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la recherche : {ex.Message}");
            }
        }

        private void ViewUsageInfoButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new StudentsUsageInfoDialog();
            dialog.ShowDialog();
        }
    }
}