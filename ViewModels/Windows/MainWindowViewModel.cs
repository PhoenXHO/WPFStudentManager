using GalaSoft.MvvmLight;
using StudentManager.Views.Pages;
using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace StudentManager.ViewModels.Windows
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<NavigationViewItem> MenuItems { get; } =
        [
            new() {
                Icon = new SymbolIcon(SymbolRegular.Board24),
                Content = "Tableau de bord",
                TargetPageType = typeof(DashboardPage)
            },
            new NavigationViewItem
            {
                Icon = new SymbolIcon(SymbolRegular.People24),
                Content = "Gestion des étudiants",
                TargetPageType = typeof(StudentsPage)
            },
            new NavigationViewItem
            {
                Icon = new SymbolIcon(SymbolRegular.Library24),
                Content = "Gestion des filières",
                TargetPageType = typeof(MajorsPage)
            },
            new NavigationViewItem
            {
                Icon = new SymbolIcon(SymbolRegular.DataUsage24),
                Content = "Statistiques",
                TargetPageType = typeof(StatsPage)
            }
        ];

        public ObservableCollection<NavigationViewItem> FooterMenuItems { get; } =
        [
            new NavigationViewItem
            {
                Icon = new SymbolIcon(SymbolRegular.Settings24),
                Content = "Paramètres",
                TargetPageType = typeof(SettingsPage)
            },
            new NavigationViewItem
            {
                Icon = new SymbolIcon(SymbolRegular.SignOut24),
                Content = "Déconnexion"
            }
        ];


        public MainWindowViewModel()
        {
        }
    }
}
