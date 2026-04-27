using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Data.SqlClient; // Added for SQL Server connection

namespace CVGeneratorApp
{
    public partial class RegisterWindow : Window
    {
        // Colors for showing errors (Red) and success (Green)
        SolidColorBrush redBorder = new SolidColorBrush(Color.FromRgb(211, 47, 47));
        SolidColorBrush greenBorder = new SolidColorBrush(Color.FromRgb(76, 175, 80));
        SolidColorBrush defaultBorder = Brushes.Transparent;

        // Timers to wait for 1 second before checking the typed input
        private DispatcherTimer? emailTimer;
        private DispatcherTimer? usernameTimer;
        private DispatcherTimer? passwordTimer;
        private DispatcherTimer? confirmPasswordTimer;

        // Opens the window and prepares the timers
        public RegisterWindow()
        {
            InitializeComponent();
            SetupTimers(); // Start the timers when the window opens
        }

        // --- SETUP TIMERS FOR TYPING DELAY ---
        private void SetupTimers()
        {
            // Set the waiting time to exactly 1 second
            TimeSpan delay = TimeSpan.FromSeconds(1);

            // Email timer setup
            emailTimer = new DispatcherTimer { Interval = delay };
            emailTimer.Tick += (s, e) => { emailTimer.Stop(); ValidateEmail(); };

            // Username timer setup
            usernameTimer = new DispatcherTimer { Interval = delay };
            usernameTimer.Tick += (s, e) => { usernameTimer.Stop(); ValidateUsername(); };

            // Password timer setup
            passwordTimer = new DispatcherTimer { Interval = delay };
            passwordTimer.Tick += (s, e) => { passwordTimer.Stop(); ValidatePassword(); };

            // Confirm Password timer setup
            confirmPasswordTimer = new DispatcherTimer { Interval = delay };
            confirmPasswordTimer.Tick += (s, e) => { confirmPasswordTimer.Stop(); ValidateConfirmPassword(); };
        }

        // --- HIDE ALL ERRORS ---
        private void ResetErrors()
        {
            // Remove all colored borders
            txtEmail.BorderBrush = defaultBorder;
            txtUsername.BorderBrush = defaultBorder;
            brdPassword.BorderBrush = defaultBorder;
            brdConfirmPassword.BorderBrush = defaultBorder;

            // Hide all warning text messages
            errEmail.Visibility = Visibility.Collapsed;
            errUsername.Visibility = Visibility.Collapsed;
            errPassword.Visibility = Visibility.Collapsed;
            errConfirmPassword.Visibility = Visibility.Collapsed;
        }

        // --- INDIVIDUAL VALIDATION RULES ---

        // Checks the Email field
        private bool ValidateEmail()
        {
            string email = txtEmail.Text;

            // Show error if email is empty
            if (ValidationHelper.IsEmpty(email))
            {
                errEmail.Text = "⚠️ Email Address cannot be empty.";
                errEmail.Visibility = Visibility.Visible;
                txtEmail.BorderBrush = redBorder;
                return false;
            }

            // Show error if email format is wrong
            if (!ValidationHelper.IsValidEmail(email))
            {
                errEmail.Text = "⚠️ Please enter a valid email address.";
                errEmail.Visibility = Visibility.Visible;
                txtEmail.BorderBrush = redBorder;
                return false;
            }

            // Hide error and show green border if everything is correct
            errEmail.Visibility = Visibility.Collapsed;
            txtEmail.BorderBrush = greenBorder;
            return true;
        }

        // Checks the Username field
        private bool ValidateUsername()
        {
            // Show error if username is empty
            if (ValidationHelper.IsEmpty(txtUsername.Text))
            {
                errUsername.Text = "⚠️ Username cannot be empty.";
                errUsername.Visibility = Visibility.Visible;
                txtUsername.BorderBrush = redBorder;
                return false;
            }

            // Hide error and show green border if correct
            errUsername.Visibility = Visibility.Collapsed;
            txtUsername.BorderBrush = greenBorder;
            return true;
        }

        // Checks the Password field for strong security rules
        private bool ValidatePassword()
        {
            string password = txtPassword.Password;

            // 1. Show error if password is empty
            if (ValidationHelper.IsEmpty(password))
            {
                errPassword.Text = "⚠️ Password cannot be empty.";
                errPassword.Visibility = Visibility.Visible;
                brdPassword.BorderBrush = redBorder;
                return false;
            }

            // 2. Show error if password is less than 6 characters
            if (password.Length < 6)
            {
                errPassword.Text = "⚠️ Password must be at least 6 characters.";
                errPassword.Visibility = Visibility.Visible;
                brdPassword.BorderBrush = redBorder;
                return false;
            }

            // 3. Show error if password has no letters
            if (!ValidationHelper.HasLetter(password))
            {
                errPassword.Text = "⚠️ Password must contain at least one letter.";
                errPassword.Visibility = Visibility.Visible;
                brdPassword.BorderBrush = redBorder;
                return false;
            }

            // 4. Show error if password has no numbers
            if (!ValidationHelper.HasNumber(password))
            {
                errPassword.Text = "⚠️ Password must contain at least one number.";
                errPassword.Visibility = Visibility.Visible;
                brdPassword.BorderBrush = redBorder;
                return false;
            }

            // 5. Show error if password has no special symbols
            if (!ValidationHelper.HasSymbol(password))
            {
                errPassword.Text = "⚠️ Password must contain at least one symbol.";
                errPassword.Visibility = Visibility.Visible;
                brdPassword.BorderBrush = redBorder;
                return false;
            }

            // Hide error and show green border if all rules are passed
            errPassword.Visibility = Visibility.Collapsed;
            brdPassword.BorderBrush = greenBorder;
            return true;
        }

        // Checks if the Confirm Password matches the original password
        private bool ValidateConfirmPassword()
        {
            string password = txtPassword.Password;
            string confirmPassword = txtConfirmPassword.Password;

            // Show error if confirm password is empty
            if (ValidationHelper.IsEmpty(confirmPassword))
            {
                errConfirmPassword.Text = "⚠️ Please confirm your password.";
                errConfirmPassword.Visibility = Visibility.Visible;
                brdConfirmPassword.BorderBrush = redBorder;
                return false;
            }

            // Show error if passwords do not match
            if (password != confirmPassword)
            {
                errConfirmPassword.Text = "⚠️ Passwords do not match.";
                errConfirmPassword.Visibility = Visibility.Visible;
                brdConfirmPassword.BorderBrush = redBorder;
                return false;
            }

            // Hide error and show green border if they match
            errConfirmPassword.Visibility = Visibility.Collapsed;
            brdConfirmPassword.BorderBrush = greenBorder;
            return true;
        }

        // --- LIVE TYPING EVENTS ---

        // Triggers when the user types in the Email field
        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtEmail.BorderBrush = defaultBorder; // Remove border color
            errEmail.Visibility = Visibility.Collapsed; // Hide error message
            emailTimer?.Stop(); // Stop the old timer
            emailTimer?.Start(); // Start a new 1-second wait
        }

        // Triggers when the user types in the Username field
        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtUsername.BorderBrush = defaultBorder;
            errUsername.Visibility = Visibility.Collapsed;
            usernameTimer?.Stop();
            usernameTimer?.Start();
        }

        // Triggers when the user types in the Password field
        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Show or hide the placeholder text
            PassPlaceholder.Visibility = string.IsNullOrEmpty(txtPassword.Password) ? Visibility.Visible : Visibility.Hidden;
            brdPassword.BorderBrush = defaultBorder;
            errPassword.Visibility = Visibility.Collapsed;
            passwordTimer?.Stop();
            passwordTimer?.Start();
        }

        // Triggers when the user types in the Confirm Password field
        private void txtConfirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Show or hide the placeholder text
            ConfirmPlaceholder.Visibility = string.IsNullOrEmpty(txtConfirmPassword.Password) ? Visibility.Visible : Visibility.Hidden;
            brdConfirmPassword.BorderBrush = defaultBorder;
            errConfirmPassword.Visibility = Visibility.Collapsed;
            confirmPasswordTimer?.Stop();
            confirmPasswordTimer?.Start();
        }

        // --- REGISTER BUTTON LOGIC (DATABASE INSERTION) ---

        // Triggers when the Register button is clicked
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ResetErrors();

                // Check all fields one by one (Sequential validation)
                // If any field is wrong, stop the process (Early Return)
                if (!ValidateEmail()) return;
                if (!ValidateUsername()) return;
                if (!ValidatePassword()) return;
                if (!ValidateConfirmPassword()) return;

                // --- DATABASE LOGIC STARTS HERE ---

                // 1. Get the connection from our DatabaseHelper class
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open(); // Open the door to the database

                    // 2. Write the SQL Query to insert data into the 'Users' table
                    string query = "INSERT INTO Users (Username, Email, Password) VALUES (@Username, @Email, @Password)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // 3. Securely pass the typed data into the SQL Query (Prevents SQL Injection)
                        cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@Password", txtPassword.Password);

                        // 4. Run the query to save data to the database
                        cmd.ExecuteNonQuery();
                    }
                }

                // --- DATABASE LOGIC ENDS HERE ---

                // If all validations and database saving are successful, show a success message
                MessageBox.Show("Registration Successful! Now you can Login.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Open the main login window
                MainWindow loginWindow = new MainWindow();
                loginWindow.Show();

                // Close this registration window
                this.Close();
            }
            catch (Exception ex)
            {
                // Show error message if the database connection or application crashes
                MessageBox.Show("Database Error: " + ex.Message, "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // --- WINDOW CONTROLS ---

        // Allows the user to drag the window around the screen
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }

        // Closes the entire application completely
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Goes back to the Login screen without registering
        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            MainWindow loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }

        // --- EYE BUTTON CONTROLS (SHOW/HIDE PASSWORD) ---

        // Show plain text password when eye icon is clicked
        private void Eye_Down(object sender, MouseButtonEventArgs e)
        {
            txtVisiblePassword.Text = txtPassword.Password;
            txtPassword.Visibility = Visibility.Collapsed;
            txtVisiblePassword.Visibility = Visibility.Visible;
            EyeIcon1.Fill = new SolidColorBrush(Color.FromRgb(0, 122, 204)); // Change icon color to blue
        }

        // Hide password and show dots when mouse is released
        private void Eye_Up(object sender, MouseButtonEventArgs e)
        {
            txtPassword.Visibility = Visibility.Visible;
            txtVisiblePassword.Visibility = Visibility.Collapsed;
            EyeIcon1.Fill = new SolidColorBrush(Color.FromRgb(136, 136, 136)); // Change icon color to gray
            txtPassword.Focus();
        }

        // Same logic for the Confirm Password eye icon
        private void ConfirmEye_Down(object sender, MouseButtonEventArgs e)
        {
            txtVisibleConfirmPassword.Text = txtConfirmPassword.Password;
            txtConfirmPassword.Visibility = Visibility.Collapsed;
            txtVisibleConfirmPassword.Visibility = Visibility.Visible;
            EyeIcon2.Fill = new SolidColorBrush(Color.FromRgb(0, 122, 204));
        }

        private void ConfirmEye_Up(object sender, MouseButtonEventArgs e)
        {
            txtConfirmPassword.Visibility = Visibility.Visible;
            txtVisibleConfirmPassword.Visibility = Visibility.Collapsed;
            EyeIcon2.Fill = new SolidColorBrush(Color.FromRgb(136, 136, 136));
            txtConfirmPassword.Focus();
        }
    }
}