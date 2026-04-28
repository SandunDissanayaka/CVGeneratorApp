using System;
using System.Windows;
using System.Windows.Controls;

namespace CVGeneratorApp
{
    public partial class DashboardWindow : Window
    {
        public DashboardWindow()
        {
            InitializeComponent();

            // LOAD SAVED DATA WHEN APP STARTS
            CVDataStore.Load();

            // Start the app by showing the Home Page
            MainContentFrame.Navigate(new HomePage());
        }

        // Helper method to change the button styles when clicked
        private void UpdateSidebarSelection(Button activeBtn)
        {
            // Reset all buttons to the normal style
            btnHome.Style = (Style)FindResource("MenuButton");
            btnEditDetails.Style = (Style)FindResource("MenuButton");
            btnTemplates.Style = (Style)FindResource("MenuButton");
            btnSettings.Style = (Style)FindResource("MenuButton");

            // Set the clicked button to the Active style
            activeBtn.Style = (Style)FindResource("ActiveMenuButton");
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            UpdateSidebarSelection(btnHome);
            MainContentFrame.Navigate(new HomePage());
        }

        private void btnEditDetails_Click(object sender, RoutedEventArgs e)
        {
            UpdateSidebarSelection(btnEditDetails);
            MainContentFrame.Navigate(new EditDetailsPage());
        }

        private void btnTemplates_Click(object sender, RoutedEventArgs e)
        {
            UpdateSidebarSelection(btnTemplates);
            // NOW IT WORKS: Navigates to the Preview Page
            MainContentFrame.Navigate(new PreviewPage());
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            UpdateSidebarSelection(btnSettings);
            // Navigate to Settings page later
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to logout?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                MainWindow login = new MainWindow();
                login.Show();
                this.Close();
            }
        }
    }
}