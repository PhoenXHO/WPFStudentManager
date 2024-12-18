using StudentManager.Models;
using StudentManager.ViewModels;
using StudentManager.ViewModels.Pages;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManager.Views.Pages
{
    /// <summary>
    /// Interaction logic for MajorsPage.xaml
    /// </summary>
    public partial class MajorsPage : Page
    {
        public MajorsViewModel ViewModel { get; set; }

        private int _lastSelectedIndex = -1;

        public MajorsPage()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var mainViewModel = (MainViewModel)DataContext;
            var dialog = new Dialogs.AddMajorDialog
            {
                DataContext = mainViewModel.MajorsViewModel
            };
            if (dialog.ShowDialog() == true)
            {
                mainViewModel.MajorsViewModel.Majors.Add(dialog.NewMajor);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var mainViewModel = (MainViewModel)DataContext;
            var selectedMajors = mainViewModel.MajorsViewModel.Majors.Where(m => m.IsSelected).ToList();
            foreach (var major in selectedMajors)
            {
                mainViewModel.MajorsViewModel.Majors.Remove(major);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var major = (Major)((CheckBox)sender).DataContext;
            major.IsSelected = true;
            ((MainViewModel)DataContext).MajorsViewModel.RaisePropertyChanged(nameof(MajorsViewModel.SelectedMajors));
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var major = (Major)((CheckBox)sender).DataContext;
            major.IsSelected = false;
            ((MainViewModel)DataContext).MajorsViewModel.RaisePropertyChanged(nameof(MajorsViewModel.SelectedMajors));
        }

        //private void SelectAllCheckBox_Checked(object sender, RoutedEventArgs e)
        //{
        //    var mainViewModel = (MainViewModel)DataContext;
        //    foreach (var major in mainViewModel.MajorsViewModel.Majors)
        //    {
        //        major.IsSelected = true;
        //    }
        //}

        //private void SelectAllCheckBox_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    var mainViewModel = (MainViewModel)DataContext;
        //    foreach (var major in mainViewModel.MajorsViewModel.Majors)
        //    {
        //        major.IsSelected = false;
        //    }
        //}

        private void UpdateDeleteButtonState()
        {
            var mainViewModel = (MainViewModel)DataContext;
            DeleteButton.IsEnabled = mainViewModel.MajorsViewModel.Majors.Any(m => m.IsSelected);
        }

        
        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            var major = (Major)((Button)sender).DataContext;
            MessageBox.Show($"Viewing details for {major.Name}");
        }
    }
}
