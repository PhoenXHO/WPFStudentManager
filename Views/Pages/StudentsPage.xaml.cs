using StudentManager.ViewModels;
using StudentManager.ViewModels.Pages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using StudentManager.Models;
using StudentManager.Views.Windows;
using System.Windows.Media;
using StudentManager.Services;
using StudentManager.DataAccess;

namespace StudentManager.Views.Pages
{
    /// <summary>
    /// Interaction logic for StudentsPage.xaml
    /// </summary>
    public partial class StudentsPage : Page
    {
        private bool _isShiftPressed;
        private int _lastSelectedIndex = -1;
        private bool _lastSelectedIndexSet = false;

        public StudentsPage()
        {
            InitializeComponent();
            DataContext = new MainViewModel();

            // Set the BreadcrumbBar
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.BreadcrumbBar.ItemsSource = new[] { "Gestion des étudiants" };
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var mainViewModel = (MainViewModel)DataContext;
            var dialog = new Dialogs.AddStudentDialog
            {
                DataContext = mainViewModel.MajorsViewModel
            };

            if (dialog.ShowDialog() == true)
            {
                var student = dialog.NewStudent;
                if (await DatabaseRepository.AddStudentAsync(student))
                {
                    mainViewModel.StudentsViewModel.Students.Add(student);
                }
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var student = (Student)((Image)sender).DataContext;
                _ = HandleImageUploadAsync(student);
            }
        }

        private void Image_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var student = (Student)((Image)sender).DataContext;
            var dialog = new Dialogs.ViewImageDialog
            {
                DataContext = student
            };
            dialog.ShowDialog();
        }

        private static async Task HandleImageUploadAsync(Student student)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                student.Picture = CloudinaryService.UploadImage(filePath);
                await DatabaseRepository.UpdateStudentAsync(student);

                MessageBox.Show("L'image a été mise à jour avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
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
                if (await DatabaseRepository.DeleteStudentAsync(student.Id))
                {
                    CloudinaryService.DeleteImage(student.Picture);
                    mainViewModel.StudentsViewModel.Students.Remove(student);
                    mainViewModel.StudentsViewModel.RemoveSelectedStudent(student);
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var student = (Student)((CheckBox)sender).DataContext;
            student.IsSelected = true;
            // Set the last selected index to the current index
            _lastSelectedIndex = ((DataGridRow)UIDataGrid.ItemContainerGenerator.ContainerFromItem(student)).GetIndex();
            _lastSelectedIndexSet = true;
            // Add the student to the selected students collection
            ((MainViewModel)DataContext).StudentsViewModel.AddSelectedStudent(student);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var student = (Student)((CheckBox)sender).DataContext;
            student.IsSelected = false;
            // Set the last selected index to the current index
            _lastSelectedIndex = ((DataGridRow)UIDataGrid.ItemContainerGenerator.ContainerFromItem(student)).GetIndex();
            _lastSelectedIndexSet = false;
            // Remove the student from the selected students collection
            ((MainViewModel)DataContext).StudentsViewModel.RemoveSelectedStudent(student);
        }

        private static T? FindVisualChild<T>(DependencyObject? obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T item)
                {
                    return item;
                }
                else
                {
                    T? childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }

        private void UpdateCheckBoxes()
        {
            var mainViewModel = (MainViewModel)DataContext;
            foreach (var student in mainViewModel.StudentsViewModel.Students)
            {
                // Queue container check after UI update
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (UIDataGrid.ItemContainerGenerator.ContainerFromItem(student) is DataGridRow row)
                    {
                        var cell = UIDataGrid.Columns[0].GetCellContent(row)?.Parent as DataGridCell;
                        if (cell != null)
                        {
                            var checkBox = FindVisualChild<CheckBox>(cell);
                            if (checkBox != null)
                            {
                                checkBox.IsChecked = student.IsSelected;
                            }
                        }
                    }
                }), System.Windows.Threading.DispatcherPriority.Background);
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
                                student.IsSelected = _lastSelectedIndexSet;
                            }
                        }
                    }
                    _lastSelectedIndex = currentIndex;
                    _lastSelectedIndexSet = !_lastSelectedIndexSet;

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

        private async void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var mainViewModel = (MainViewModel)DataContext;
            var filter = SearchTextBox.Text;

            try
            {
                var students = await DatabaseRepository.GetAllStudentsAsync();
                var filteredStudents = students.Where(s => 
                    s.Id.ToString().Contains(filter) ||
                    s.FirstName.Contains(filter) ||
                    s.LastName.Contains(filter)
                );

                mainViewModel.StudentsViewModel.Students.Clear();
                foreach (var student in filteredStudents)
                {
                    student.IsSelected = mainViewModel.StudentsViewModel.SelectedStudents
                        .Any(s => s.Id == student.Id);
                    mainViewModel.StudentsViewModel.Students.Add(student);
                }

                UpdateCheckBoxes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la recherche : {ex.Message}");
            }
        }

        private async void MajorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var mainViewModel = (MainViewModel)DataContext;
            var selectedMajor = MajorComboBox.SelectedItem as Major;

            try
            {
                IEnumerable<Student> students;
                if (MajorComboBox.SelectedIndex == 0 || (selectedMajor != null && selectedMajor.Name == "Tout"))
                {
                    students = await DatabaseRepository.GetAllStudentsAsync();
                    mainViewModel.StudentsViewModel.SelectedMajor = null;
                }
                else
                {
                    students = await DatabaseRepository.GetStudentsByMajorAsync(selectedMajor.Name);
                    mainViewModel.StudentsViewModel.SelectedMajor = selectedMajor;
                }

                mainViewModel.StudentsViewModel.Students.Clear();
                foreach (var student in students)
                {
                    student.IsSelected = mainViewModel.StudentsViewModel.SelectedStudents
                        .Any(s => s.Id == student.Id);
                    mainViewModel.StudentsViewModel.Students.Add(student);
                }

                UpdateCheckBoxes();
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

        private void DeselectAllButton_Click(object sender, RoutedEventArgs e)
        {
            var mainViewModel = (MainViewModel)DataContext;
            var SelectedStudents = mainViewModel.StudentsViewModel.SelectedStudents.ToList();
            foreach (var student in SelectedStudents)
            {
                student.IsSelected = false;
            }
            // Raise the PropertyChanged event for SelectedStudents
            mainViewModel.StudentsViewModel.RaisePropertyChanged(nameof(StudentsViewModel.SelectedStudents));
        }

        private async void UIDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var student = (Student)e.Row.Item;
                if (await DatabaseRepository.UpdateStudentAsync(student))
                {
                    // Success - the property has already been updated via binding
                }
                else
                {
                    // Failed to update database - revert the change
                    MessageBox.Show("Failed to update the student information", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    // Refresh the row to show original data
                    UIDataGrid.Items.Refresh();
                }
            }
        }
    }
}