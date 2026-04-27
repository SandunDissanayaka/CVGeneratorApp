using System;
using System.Windows;

namespace CVGeneratorApp
{
    public partial class DashboardWindow : Window
    {
        public DashboardWindow()
        {
            InitializeComponent();

            // Load the Home Page automatically when the Dashboard opens
            MainContentFrame.Navigate(new HomePage());
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            // Load the Home Page when the Home button is clicked
            MainContentFrame.Navigate(new HomePage());
        }

        private void btnEditDetails_Click(object sender, RoutedEventArgs e)
        {
            // We will load the Edit Details page here later
        }

        private void btnTemplates_Click(object sender, RoutedEventArgs e)
        {
            // We will load the Templates page here later
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            // We will load the Settings page here later
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            // Ask the user for confirmation before logging out
            MessageBoxResult result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // If the user clicks 'Yes', go back to the Login screen
            if (result == MessageBoxResult.Yes)
            {
                MainWindow login = new MainWindow();
                login.Show();       // Show the login window
                this.Close();       // Close the dashboard window
            }
        }
    }
}