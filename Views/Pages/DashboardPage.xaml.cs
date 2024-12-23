﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using StudentManager.ViewModels.Pages;

namespace StudentManager.Views.Pages
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            InitializeComponent();
            DataContext = new DashboardViewModel();

            // Set the BreadcrumbBar
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.BreadcrumbBar.ItemsSource = new[] { "Tableau de bord" };
        }
    }
}
