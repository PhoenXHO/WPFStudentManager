using StudentManager.ViewModels;
using StudentManager.ViewModels.Pages;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
                mainViewModel.StudentsViewModel.Students.Add(dialog.NewStudent);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var mainViewModel = (MainViewModel)DataContext;
            var selectedStudents = mainViewModel.StudentsViewModel.Students.Where(s => s.IsSelected).ToList();
            foreach (var student in selectedStudents)
            {
                mainViewModel.StudentsViewModel.Students.Remove(student);
            }

            // Uncheck the Select All checkbox if all students are unselected
            if (mainViewModel.StudentsViewModel.SelectedStudents.Count == 0)
            {
                SelectAllCheckBox.IsChecked = false;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var student = (Models.Student)((CheckBox)sender).DataContext;
            student.IsSelected = true;
            // Set the last selected index to the current index
            _lastSelectedIndex = ((DataGridRow)UIDataGrid.ItemContainerGenerator.ContainerFromItem(student)).GetIndex();
            // Raise the PropertyChanged event for SelectedStudents
            ((MainViewModel)DataContext).StudentsViewModel.RaisePropertyChanged(nameof(StudentsViewModel.SelectedStudents));
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var student = (Models.Student)((CheckBox)sender).DataContext;
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
                            if (dataGrid?.Items[i] is Models.Student student)
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
            var student = (Models.Student)((Button)sender).DataContext;
            MessageBox.Show($"Viewing details for {student.FirstName} {student.LastName}");
        }
    }
}
