using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using Microsoft.Data.SqlClient; // Required for SQL Server Connection

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
                // 1. Clear any old errors first
                ResetErrors();

                // 2. Check if the Username field is empty
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    errUsername.Visibility = Visibility.Visible;
                    txtUsername.BorderBrush = redBorder;
                    return; // Stop here and wait for user input
                }

                // 3. Check if the Password field is empty
                if (string.IsNullOrWhiteSpace(txtPassword.Password))
                {
                    errPassword.Visibility = Visibility.Visible;
                    brdPassword.BorderBrush = redBorder;
                    return; // Stop here
                }

                // --- DATABASE LOGIC STARTS HERE ---

                // Connect to the database using our DatabaseHelper class
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open(); // Open the door to the database

                    // SQL Query to check if the Username and Password match any record in the Users table
                    // COUNT(1) checks how many matching rows exist (1 = success, 0 = wrong details)
                    string query = "SELECT COUNT(1) FROM Users WHERE Username = @Username AND Password = @Password";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Securely pass the parameters to prevent SQL Injection
                        cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                        cmd.Parameters.AddWithValue("@Password", txtPassword.Password);

                        // ExecuteScalar returns the result of the COUNT query
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count == 1)
                        {
                            // If count is 1, the user exists and the password is correct!
                            MessageBox.Show("Login Successful! Welcome back.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                            // TODO: Later, we will open the Main CV Dashboard window here.
                            // DashboardWindow dash = new DashboardWindow();
                            // dash.Show();
                            // this.Close();
                        }
                        else
                        {
                            // If count is 0, the username or password is wrong
                            MessageBox.Show("Invalid Username or Password. Please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }

                // --- DATABASE LOGIC ENDS HERE ---
            }
            catch (Exception ex)
            {
                // Show system error if the app or database crashes
                MessageBox.Show("Something went wrong: " + ex.Message, "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // --- LIVE UI UPDATE EVENTS ---

        // Triggers IMMEDIATELY when the user types in the Username box
        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtUsername != null)
            {
                txtUsername.BorderBrush = defaultBorder;
                errUsername.Visibility = Visibility.Collapsed;
            }
        }

        // Triggers IMMEDIATELY when the user types in the Password box
        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (brdPassword != null)
            {
                brdPassword.BorderBrush = defaultBorder;
                errPassword.Visibility = Visibility.Collapsed;
            }

            if (PasswordPlaceholder != null)
            {
                PasswordPlaceholder.Visibility = string.IsNullOrEmpty(txtPassword.Password) ? Visibility.Visible : Visibility.Hidden;
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