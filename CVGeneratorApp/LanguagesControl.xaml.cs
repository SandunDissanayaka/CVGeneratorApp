using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CVGeneratorApp
{
    public partial class LanguagesControl : UserControl
    {
        // Property to link the local collection to the Global Data Store
        public ObservableCollection<LanguageItem> LanguageList { get; set; }

        public LanguagesControl()
        {
            InitializeComponent();

            // Bind the JSON Data Store list to the UI ItemsControl
            LanguageList = CVDataStore.Profile.LanguageList;
            lstLanguages.ItemsSource = LanguageList;
        }

        // --- ADD LANGUAGE LOGIC ---
        private void btnAddLanguage_Click(object sender, RoutedEventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(cmbLanguage.Text))
            {
                MessageBox.Show("Please enter or select a language.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Safely get the proficiency text
            string proficiency = cmbProficiency.Text;
            if (string.IsNullOrWhiteSpace(proficiency) && cmbProficiency.SelectedItem is ComboBoxItem selectedItem)
            {
                proficiency = selectedItem.Content.ToString();
            }

            // Add the new language to the collection
            LanguageList.Add(new LanguageItem
            {
                LanguageName = cmbLanguage.Text,
                Proficiency = proficiency
            });

            // Persist the changes to the JSON file immediately
            CVDataStore.Save();

            // Clear the inputs for the next entry
            cmbLanguage.Text = string.Empty;
            cmbProficiency.Text = string.Empty;
            cmbProficiency.SelectedIndex = -1;
        }

        // --- REMOVE LANGUAGE LOGIC ---
        private void RemoveLanguage_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button?.DataContext is LanguageItem item)
            {
                // Remove the selected item from the collection
                LanguageList.Remove(item);

                // Update the JSON file immediately after removal
                CVDataStore.Save();
            }
        }
    }
}