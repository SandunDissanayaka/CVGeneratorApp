using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;

namespace CVGeneratorApp
{
    public partial class MainWindow : Window
    {
        // Colors for Validation (Red for errors, Transparent for normal state)
        SolidColorBrush redBorder = new SolidColorBrush(Color.FromRgb(211, 47, 47));
        SolidColorBrush defaultBorder = Brushes.Transparent;

        public MainWindow()
        {
            InitializeComponent();
        }

        // --- VALIDATION LOGIC ---

        // This method hides all error messages and resets borders to normal
        private void ResetErrors()
        {
            txtUsername.BorderBrush = defaultBorder;
            brdPassword.BorderBrush = defaultBorder;

            errUsername.Visibility = Visibility.Collapsed;
            errPassword.Visibility = Visibility.Collapsed;
        }

        // Triggers when the user clicks the "Login" button
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Clear any old errors first
                ResetErrors();

                // 1. Check if the Username field is empty
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    errUsername.Visibility = Visibility.Visible;
                    txtUsername.BorderBrush = redBorder;
                    return; // Stop here
                }

                // 2. Check if the Password field is empty
                if (string.IsNullOrWhiteSpace(txtPassword.Password))
                {
                    errPassword.Visibility = Visibility.Visible;
                    brdPassword.BorderBrush = redBorder;
                    return; // Stop here
                }

                // If both fields are filled, show success (Database connection comes later)
                MessageBox.Show("Login Successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // --- LIVE UI UPDATE EVENTS (THE FIX) ---

        // This triggers IMMEDIATELY when the user types in the Username box
        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            // If the user starts typing, hide the error and reset the border
            if (txtUsername != null)
            {
                txtUsername.BorderBrush = defaultBorder;
                errUsername.Visibility = Visibility.Collapsed;
            }
        }

        // This triggers IMMEDIATELY when the user types in the Password box
        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // If the user starts typing, hide the error and reset the border
            if (brdPassword != null)
            {
                brdPassword.BorderBrush = defaultBorder;
                errPassword.Visibility = Visibility.Collapsed;
            }

            // Handle the "Password" placeholder text visibility
            if (PasswordPlaceholder != null)
            {
                if (string.IsNullOrEmpty(txtPassword.Password))
                    PasswordPlaceholder.Visibility = Visibility.Visible;
                else
                    PasswordPlaceholder.Visibility = Visibility.Hidden;
            }
        }

        // --- WINDOW CONTROLS ---

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow regWindow = new RegisterWindow();
            regWindow.Show();
            this.Close();
        }

        // --- EYE BUTTON CONTROLS (SHOW/HIDE PASSWORD) ---

        private void btnShowPassword_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            txtVisiblePassword.Text = txtPassword.Password;
            txtPassword.Visibility = Visibility.Collapsed;
            txtVisiblePassword.Visibility = Visibility.Visible;
            EyeIcon.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
        }

        private void btnShowPassword_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtPassword.Password = txtVisiblePassword.Text;
            txtVisiblePassword.Visibility = Visibility.Collapsed;
            txtPassword.Visibility = Visibility.Visible;
            EyeIcon.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#888888"));
            txtPassword.Focus();
        }
    }
}