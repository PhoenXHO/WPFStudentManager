using GalaSoft.MvvmLight;
using StudentManager.Models;
using StudentManager.Services;
using System.Collections.ObjectModel;
using System.Windows;
using StudentManager.DataAccess;

namespace StudentManager.ViewModels.Pages
{
    public class MajorsViewModel : ViewModelBase
    {
        public ObservableCollection<Major> Majors => CacheService.Majors;
        public ObservableCollection<Major> SelectedMajors => new(Majors.Where(m => m.IsSelected));

        public MajorsViewModel()
        {
            _ = LoadMajorsAsync();
        }

        private async Task LoadMajorsAsync()
        {
            try
            {
                if (Majors.Count == 0)
                {
                    var majors = await DatabaseRepository.GetAllMajorsAsync();
                    foreach (var major in majors)
                    {
                        Majors.Add(major);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des majeures: {ex.Message}");
            }
        }
    }
}