using GalaSoft.MvvmLight;
using StudentManager.Models;
using StudentManager.Services;
using System.Collections.ObjectModel;
using System.Windows;
using StudentManager.DataAccess;
using Telerik.Windows.Diagrams.Core;

namespace StudentManager.ViewModels.Pages
{
    public class MajorsViewModel : ViewModelBase
    {
        public ObservableCollection<Major> Majors => CacheService.Majors;
        public ObservableCollection<Major> SelectedMajors => new(Majors.Where(m => m.IsSelected));
        public ObservableCollection<Major> MajorsWithAll { get; set; }

        public MajorsViewModel()
        {
            //_ = UpdateMajorsAsync();

            // Create a new ObservableCollection with the 'All' item at the beginning
            MajorsWithAll = [];
            MajorsWithAll.Insert(0,
                new Major { MajorId = 0, Name = "Tout", Description = "Toutes les majeures", Responsable = "Tous" });
        }

        public async Task UpdateMajorsAsync()
        {
            try
            {
                CacheService.Majors = new(await DatabaseRepository.GetAllMajorsAsync());
                MajorsWithAll.AddRange(CacheService.Majors);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des majeures: {ex.Message}");
            }
        }

        public void AddMajor(Major major)
        {
            Majors.Add(major);
            MajorsWithAll.Add(major);
        }

        public void RemoveMajor(Major major)
        {
            Majors.Remove(major);
            MajorsWithAll.Remove(major);
        }
    }
}