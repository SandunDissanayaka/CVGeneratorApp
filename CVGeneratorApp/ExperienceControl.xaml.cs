using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CVGeneratorApp
{
    /// <summary>
    /// Interaction logic for ExperienceControl.xaml.
    /// Manages the data entry for work history and persists it to JSON.
    /// </summary>
    public partial class ExperienceControl : UserControl
    {
        // Property to link the UI list to our Global Data Store
        public ObservableCollection<ExperienceItem> ExperienceList { get; set; }

        public ExperienceControl()
        {
            InitializeComponent();

            // 1. Point this local collection to the Global Profile list.
            // This ensures data is shared across different pages of the app.
            ExperienceList = CVDataStore.Profile.ExperienceList;

            // 2. Bind the list to the UI ItemsControl (lstExperience)
            lstExperience.ItemsSource = ExperienceList;
        }

        /// <summary>
        /// Handles the Add Experience button click.
        /// Captures input, adds it to the global list, and saves to JSON.
        /// </summary>
        private void btnAddExperience_Click(object sender, RoutedEventArgs e)
        {
            // Simple validation: Ensure Job Title and Company are provided
            if (!string.IsNullOrEmpty(cmbJobTitle.Text) && !string.IsNullOrEmpty(txtCompany.Text))
            {
                // Create a new Experience item with the input values
                ExperienceList.Add(new ExperienceItem
                {
                    JobTitle = cmbJobTitle.Text,
                    Company = txtCompany.Text,
                    // Format duration and location into a single display string
                    DurationLocation = txtDuration.Text + " | " + txtLocation.Text,
                    Responsibilities = txtResponsibilities.Text
                });

                // 3. PERSISTENCE: Save the entire updated profile to the JSON file.
                // This prevents data loss if the app is closed.
                CVDataStore.Save();

                // Clear input fields for the next entry
                ClearInputFields();
            }
            else
            {
                MessageBox.Show("Please fill the Job Title and Company fields.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Resets all input fields to their default empty state.
        /// </summary>
        private void ClearInputFields()
        {
            cmbJobTitle.Text = "";
            txtCompany.Clear();
            txtDuration.Clear();
            txtLocation.Clear();
            txtResponsibilities.Clear();
        }

        /// <summary>
        /// Handles removing an experience item from the list.
        /// </summary>
        private void RemoveExperience_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var item = button?.DataContext as ExperienceItem;

            if (item != null)
            {
                // Remove from the collection (updates UI automatically)
                ExperienceList.Remove(item);

                // 4. PERSISTENCE: Update the JSON file to reflect the deletion.
                CVDataStore.Save();
            }
        }
    }
}