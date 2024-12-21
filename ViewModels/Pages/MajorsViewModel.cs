using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MySql.Data.MySqlClient;
using StudentManager.Models;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace StudentManager.ViewModels.Pages
{
    public class MajorsViewModel : ViewModelBase
    {
        public ObservableCollection<Major> Majors { get; }
        public ObservableCollection<Major> SelectedMajors => new(Majors.Where(m => m.IsSelected));

        public ICommand ViewDetailsCommand { get; }

        public List<Major> MajorsWithAll => new List<Major>
{
    new Major
    {
        Id = 0,
        Name = "Toutes les filières",
        Description = string.Empty // Valeur par défaut pour la description
    }
}.Concat(Majors).ToList();



        public MajorsViewModel()
        {

            Majors = new ObservableCollection<Major>();
            LoadMajorsAsync();
            


            Majors.CollectionChanged += (m, e) => RaisePropertyChanged(nameof(SelectedMajors)); // Update SelectedMajors when Majors changes

            ViewDetailsCommand = new RelayCommand<Major>(ViewDetails);
        }



        private async Task LoadMajorsAsync()
        {
            try
            {
                using (var connection = DBConnection.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = "SELECT majors.Id,  majors.Name,  majors.Description FROM majors";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            
                            while (await reader.ReadAsync())
                            {
                                
                                var major = new Major
                                {
                                    Id = reader.GetInt32("Id"), 
                                    Name = reader.GetString("Name"), 
                                    Description = reader.GetString("Description") 
                                };

                                
                                Majors.Add(major);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               
                MessageBox.Show($"Erreur lors du chargement des fiil: {ex.Message}");
            }

        }




        private void ViewDetails(Major major)
        {
            
            MessageBox.Show($"Viewing details for {major.Name}");
        }
    }
}