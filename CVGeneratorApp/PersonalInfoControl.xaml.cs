using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CVGeneratorApp
{
    public partial class PersonalInfoControl : UserControl
    {
        public PersonalInfoControl()
        {
            InitializeComponent();
        }

        // This runs when the "Upload Photo" button is clicked
        private void btnUploadPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a profile photo";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png";

            if (op.ShowDialog() == true)
            {
                // 1. Set the image source
                imgProfile.Source = new BitmapImage(new Uri(op.FileName));

                // 2. Hide the icon and show the 'Remove' button
                txtPhotoPlaceholder.Visibility = Visibility.Collapsed;
                btnRemovePhoto.Visibility = Visibility.Visible;
            }
        }

        // This runs when the "Remove Photo" button is clicked
        private void btnRemovePhoto_Click(object sender, RoutedEventArgs e)
        {
            // 1. Clear the image source
            imgProfile.Source = null;

            // 2. Show the icon again and hide the 'Remove' button
            txtPhotoPlaceholder.Visibility = Visibility.Visible;
            btnRemovePhoto.Visibility = Visibility.Collapsed;
        }
    }
}