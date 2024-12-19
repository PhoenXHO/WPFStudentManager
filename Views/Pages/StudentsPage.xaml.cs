using StudentManager.ViewModels;
using StudentManager.ViewModels.Pages;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using StudentManager.Models;  // Assurez-vous que le modèle `Student` est bien importé

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
                // Ajouter l'étudiant à la base de données et mettre à jour le ViewModel
                var student = dialog.NewStudent;
                AddStudentToDatabase(student);  // Ajouter l'étudiant à la base de données
                mainViewModel.StudentsViewModel.Students.Add(student);  // Mettre à jour la liste des étudiants dans le ViewModel
            }
        }

        private void AddStudentToDatabase(Student student)
        {
            try
            {
                using (var connection = DBConnection.GetConnection())
                {
                    connection.Open();
                    var query = "INSERT INTO students (FirstName, LastName, Email, MajorId, DateOfBirth) VALUES (@FirstName, @LastName, @Email, @MajorId, @DateOfBirth)";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", student.FirstName);
                        command.Parameters.AddWithValue("@LastName", student.LastName);
                        command.Parameters.AddWithValue("@Email", student.Email);
                        command.Parameters.AddWithValue("@MajorId", student.Major.Id);
                        command.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ajout de l'étudiant : {ex.Message}");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var mainViewModel = (MainViewModel)DataContext;
            var selectedStudents = mainViewModel.StudentsViewModel.Students.Where(s => s.IsSelected).ToList();
            foreach (var student in selectedStudents)
            {
                // Supprimer l'étudiant de la base de données
                DeleteStudentFromDatabase(student);
                mainViewModel.StudentsViewModel.Students.Remove(student);  // Supprimer de la liste dans le ViewModel
            }

            // Uncheck the Select All checkbox if all students are unselected
            if (mainViewModel.StudentsViewModel.SelectedStudents.Count == 0)
            {
                SelectAllCheckBox.IsChecked = false;
            }
        }

        private void DeleteStudentFromDatabase(Student student)
        {
            try
            {
                using (var connection = DBConnection.GetConnection())
                {
                    connection.Open();
                    var query = "DELETE FROM students WHERE Id = @Id";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", student.Id);
                        command.ExecuteNonQuery();
                    }
                }
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
        }

        private void SelectAllCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var mainViewModel = (MainViewModel)DataContext;
            for (int i = 0; i < mainViewModel.StudentsViewModel.Students.Count; i++)
            {
                mainViewModel.StudentsViewModel.Students[i].IsSelected = false;
            }
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

                    // Cancel the left mouse button click event
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
            var mainViewModel = (MainViewModel)DataContext;
            var student = (Student)((Button)sender).DataContext;
            MessageBox.Show($"Viewing details for {student.FirstName} {student.LastName}");
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBox != null)
            {
                var filter = SearchTextBox.Text;

                try
                {
                    using (var connection = DBConnection.GetConnection())
                    {
                        connection.Open();
                        var query = @"SELECT students.Id, students.FirstName, students.LastName, students.Email, 
                                     students.MajorId, students.DateOfBirth, 
                                     majors.Name as MajorName, majors.Description as MajorDescription
                              FROM students
                              LEFT JOIN majors ON students.MajorId = majors.Id
                              WHERE CAST(students.Id AS CHAR) LIKE @filter 
                                 OR students.FirstName LIKE @filter 
                                 OR students.LastName LIKE @filter";

                        using (var command = new MySqlCommand(query, connection))
                        {
                            // Ajout du filtre avec le caractère '%' pour une recherche partielle
                            command.Parameters.AddWithValue("@filter", $"%{filter}%");

                            using (var reader = command.ExecuteReader())
                            {
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
                                            Description = reader.GetString("MajorDescription")
                                        },
                                        DateOfBirth = reader.GetDateTime("DateOfBirth")
                                    });
                                }

                                // Mise à jour de la source de données pour le DataGrid
                                UIDataGrid.ItemsSource = students;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la recherche : {ex.Message}");
                }
            }
        }

        // Événement qui se déclenche lors du double-clic sur une ligne dans le DataGrid
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            var studentToUpdate = dataGrid?.SelectedItem as Student;  // Get the student object from the DataGrid selection

            if (studentToUpdate != null)
            {
                // Now you can call the method to update the student
                UpdateStudentInDatabase(studentToUpdate);
                MessageBox.Show($"Student {studentToUpdate.FirstName} {studentToUpdate.LastName} has been updated.");
            }
        }


        // Fonction pour mettre à jour l'étudiant dans la base de données
        private void UpdateStudentInDatabase(Student student)
        {
            try
            {
                using (var connection = DBConnection.GetConnection())
                {
                    connection.Open();

                    // SQL query to update the student record
                    var query = @"UPDATE students 
                          SET FirstName = @FirstName, LastName = @LastName, 
                              Email = @Email, MajorId = @MajorId, DateOfBirth = @DateOfBirth
                          WHERE Id = @Id";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        // Add parameters to the query
                        command.Parameters.AddWithValue("@Id", student.Id);
                        command.Parameters.AddWithValue("@FirstName", student.FirstName);
                        command.Parameters.AddWithValue("@LastName", student.LastName);
                        command.Parameters.AddWithValue("@Email", student.Email);
                        command.Parameters.AddWithValue("@MajorId", student.Major.Id);
                        command.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);

                        // Execute the update
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while updating student: {ex.Message}");
            }
        }



    }
}
