using System.ComponentModel;
using System.IO;
using System.Text.Json;

namespace StudentManager.ViewModels.Pages
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private string _currentApplicationTheme;

        public string CurrentApplicationTheme
        {
            get => _currentApplicationTheme;
            set
            {
                if (_currentApplicationTheme != value)
                {
                    _currentApplicationTheme = value;
                    OnPropertyChanged(nameof(CurrentApplicationTheme));
                }
            }
        }

        public SettingsViewModel()
        {
            // Charger le thème depuis un fichier JSON
            if (File.Exists("theme.json"))
            {
                var themeData = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText("theme.json"));
                _currentApplicationTheme = themeData?["Theme"] ?? "Dark";
            }
            else
            {
                _currentApplicationTheme = "Dark";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
