using StudentManager.Models;
using StudentManager.ViewModels;
using System.Windows;
using System.Windows.Media.Imaging;
using Wpf.Ui.Controls;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;
using StudentManager.Services;

namespace StudentManager.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for AddStudentDialog.xaml
    /// </summary>
    public partial class AddStudentDialog : FluentWindow
    {
        private string? _selectedImagePath;

        public Student NewStudent { get; private set; }

        public AddStudentDialog()
        {
            InitializeComponent();
        }

        private void ChooseImage_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedImagePath = openFileDialog.FileName;
                PreviewImage.Source = new BitmapImage(new Uri(_selectedImagePath));
            }
        }

        private bool ValidateInput()
        {
            bool isValid = true;

            // Validate First Name
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text))
            {
                FirstNameError.Text = "Le prénom est obligatoire";
                FirstNameError.Visibility = Visibility.Visible;
                isValid = false;
            }
            else
            {
                FirstNameError.Visibility = Visibility.Collapsed;
            }

            // Validate Last Name
            if (string.IsNullOrWhiteSpace(LastNameTextBox.Text))
            {
                LastNameError.Text = "Le nom est obligatoire";
                LastNameError.Visibility = Visibility.Visible;
                isValid = false;
            }
            else
            {
                LastNameError.Visibility = Visibility.Collapsed;
            }

            // Validate Date of Birth
            if (!DateOfBirthPicker.SelectedDate.HasValue)
            {
                DateOfBirthError.Text = "La date de naissance est obligatoire";
                DateOfBirthError.Visibility = Visibility.Visible;
                isValid = false;
            }
            else
            {
                DateOfBirthError.Visibility = Visibility.Collapsed;
            }

            // Validate Picture
            if (string.IsNullOrEmpty(_selectedImagePath))
            {
                PictureError.Text = "La photo est obligatoire";
                PictureError.Visibility = Visibility.Visible;
                isValid = false;
            }
            else
            {
                PictureError.Visibility = Visibility.Collapsed;
            }

            // Validate Major
            if (MajorComboBox.SelectedItem == null)
            {
                MajorError.Text = "La filière est obligatoire";
                MajorError.Visibility = Visibility.Visible;
                isValid = false;
            }
            else
            {
                MajorError.Visibility = Visibility.Collapsed;
            }

            return isValid;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return;

            if (MajorComboBox.SelectedItem is Major selectedMajor)
            {
                string imageUrl = UploadImageToCloudinary(_selectedImagePath);

                NewStudent = new Student
                {
                    FirstName = FirstNameTextBox.Text,
                    LastName = LastNameTextBox.Text,
                    Email = EmailTextBox.Text,
                    Major = selectedMajor,
                    DateOfBirth = DateOfBirthPicker.SelectedDate,
                    Picture = imageUrl
                };
                DialogResult = true;
            }
            else
            {
                System.Windows.MessageBox.Show("Veuillez sélectionner une filière.");
            }
        }

        private string UploadImageToCloudinary(string filePath)
        {
            var cloudinary = CloudinaryService.GetCloudinary();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(filePath)
            };
            var uploadResult = cloudinary.Upload(uploadParams);
            return uploadResult.SecureUrl.AbsoluteUri;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void MajorComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
