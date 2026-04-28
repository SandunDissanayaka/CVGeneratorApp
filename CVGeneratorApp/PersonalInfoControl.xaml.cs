using Microsoft.Win32; // This is required for OpenFileDialog
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging; // This is required for BitmapImage

namespace CVGeneratorApp
{
    public partial class PersonalInfoControl : UserControl
    {
        public PersonalInfoControl()
        {
            InitializeComponent();

            // Load saved data from JSON when the page opens
            LoadDataFromStore();
        }

        private void LoadDataFromStore()
        {
            var data = CVDataStore.Profile.PersonalInfo;
            txtFullName.Text = data.FullName;
            cmbTitle.Text = data.JobTitle;
            txtEmail.Text = data.Email;
            txtPhone.Text = data.Phone;
            txtLinkedIn.Text = data.LinkedIn;
            txtGitHub.Text = data.GitHub;
            txtAddress.Text = data.Location;

            // Load the image if a path exists in the data store
            if (!string.IsNullOrEmpty(data.PhotoPath) && System.IO.File.Exists(data.PhotoPath))
            {
                UpdateProfileImage(data.PhotoPath);
            }
        }

        private void btnSaveNext_Click(object sender, RoutedEventArgs e)
        {
            // 1. Capture data from UI and save to Global Store
            var data = CVDataStore.Profile.PersonalInfo;

            data.FullName = txtFullName.Text;
            data.JobTitle = cmbTitle.Text;
            data.Email = txtEmail.Text;
            data.Phone = txtPhone.Text;
            data.LinkedIn = txtLinkedIn.Text;
            data.GitHub = txtGitHub.Text;
            data.Location = txtAddress.Text;

            // 2. Save the entire profile to cv_userdata.json
            CVDataStore.Save();

            MessageBox.Show("Personal Information Saved Successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Trigger Navigation (Moved logic to Parent EditDetailsPage if needed)
        }

        /// <summary>
        /// Logic to open file dialog and select a photo
        /// </summary>
        private void btnUploadPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // Filter only image files
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedPath = openFileDialog.FileName;

                // 1. Save the image path to our Data Store
                CVDataStore.Profile.PersonalInfo.PhotoPath = selectedPath;
                CVDataStore.Save();

                // 2. Show the image on UI
                UpdateProfileImage(selectedPath);
            }
        }

        /// <summary>
        /// Resets the image to the default placeholder
        /// </summary>
        private void btnRemovePhoto_Click(object sender, RoutedEventArgs e)
        {
            // 1. Clear the path from Data Store
            CVDataStore.Profile.PersonalInfo.PhotoPath = string.Empty;
            CVDataStore.Save();

            // 2. Clear UI
            imgProfile.Source = null;
            txtPhotoPlaceholder.Visibility = Visibility.Visible;
            btnRemovePhoto.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Helper method to load a BitmapImage and handle UI visibility
        /// </summary>
        private void UpdateProfileImage(string path)
        {
            try
            {
                BitmapImage bitmap = new BitmapImage(new Uri(path));
                imgProfile.Source = bitmap;

                // Hide the user icon placeholder
                txtPhotoPlaceholder.Visibility = Visibility.Collapsed;
                // Show the "Remove Photo" button
                btnRemovePhoto.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading image: " + ex.Message);
            }
        }
    }
}