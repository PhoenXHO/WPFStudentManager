using StudentManager.ViewModels.Pages;
using System.Windows.Controls;

namespace StudentManager.Views.Pages
{
    /// <summary>
    /// Interaction logic for MajorsPage.xaml
    /// </summary>
    public partial class MajorsPage : Page
    {
        public MajorsPage()
        {
            InitializeComponent();
            DataContext = new MajorsViewModel();
        }
    }
}
