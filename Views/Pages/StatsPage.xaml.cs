using HelixToolkit.Wpf;
using StudentManager.Models;
using StudentManager.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudentManager.Views.Pages
{
    /// <summary>
    /// Interaction logic for StatsPage.xaml
    /// </summary>
    public partial class StatsPage : Page
    {
        public StatsPage()
        {
            InitializeComponent();

            // Instantiate the ViewModel and set as DataContext for binding
            var vm = new StatsViewModel();
            DataContext = vm;
            System.Diagnostics.Debug.WriteLine($"DataContext set: {DataContext != null}");
            System.Diagnostics.Debug.WriteLine($"VM MajorStats Count: {vm.MajorStats.Count}");
            foreach (var stat in vm.MajorStats)
            {
                System.Diagnostics.Debug.WriteLine($"Major: {stat.Major}, Count: {stat.StudentCount}");
            }
        }
    }
}
