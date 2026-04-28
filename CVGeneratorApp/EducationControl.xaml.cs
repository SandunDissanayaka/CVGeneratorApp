using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CVGeneratorApp
{
    /// <summary>
    /// Interaction logic for EducationControl.xaml
    /// Handles dynamic forms for Degree, A/L, and O/L data entry.
    /// </summary>
    public partial class EducationControl : UserControl
    {
        // 🔴 වෙනස 1: අලුත් Collection එකක් හදන්නේ නෑ, Global එකට ලින්ක් කරන්න variable එකක් විතරක් හදනවා.
        public ObservableCollection<EducationItem> EducationList { get; set; }

        public EducationControl()
        {
            InitializeComponent();

            // 🔴 වෙනස 2: Data List එක කෙළින්ම අපේ Global 'CVDataStore' එකෙන් අරන් බයින්ඩ් කරනවා.
            EducationList = CVDataStore.Profile.EducationList;
            lstEducation.ItemsSource = EducationList;

            // Populate year dropdowns dynamically when the control loads
            PopulateYears();
        }

        /// <summary>
        /// Generates a list of years (e.g., from 1990 to current year + 4) 
        /// and populates all year-related ComboBoxes.
        /// </summary>
        private void PopulateYears()
        {
            int currentYear = DateTime.Now.Year;
            // Generate years from current year + 4 (for expected graduation) down to 1990
            for (int year = currentYear + 4; year >= 1990; year--)
            {
                cmbDegreeStart.Items.Add(year.ToString());
                cmbDegreeEnd.Items.Add(year.ToString());
                cmbALYear.Items.Add(year.ToString());
                cmbOLYear.Items.Add(year.ToString());
            }
        }

        // --- DYNAMIC UI LOGIC ---

        private void cmbEduLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pnlDegree == null || pnlAL == null || pnlOL == null) return;

            string selectedLevel = ((ComboBoxItem)cmbEduLevel.SelectedItem).Content.ToString();

            pnlDegree.Visibility = Visibility.Collapsed;
            pnlAL.Visibility = Visibility.Collapsed;
            pnlOL.Visibility = Visibility.Collapsed;

            if (selectedLevel.Contains("Degree")) pnlDegree.Visibility = Visibility.Visible;
            else if (selectedLevel.Contains("A/L")) pnlAL.Visibility = Visibility.Visible;
            else if (selectedLevel.Contains("O/L")) pnlOL.Visibility = Visibility.Visible;
        }

        // --- DATA ENTRY LOGIC ---

        private void btnAddEducation_Click(object sender, RoutedEventArgs e)
        {
            // Now we read from the ComboBox text instead of a TextBox
            if (string.IsNullOrEmpty(cmbInstitution.Text))
            {
                MessageBox.Show("Please enter or select the Institution / School Name.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string level = ((ComboBoxItem)cmbEduLevel.SelectedItem).Content.ToString();
            EducationItem newItem = new EducationItem { Institution = cmbInstitution.Text };

            // Format data based on education level
            if (level.Contains("Degree"))
            {
                newItem.MainTitle = cmbDegree.Text;
                newItem.Details = $"{cmbDegreeStart.Text} - {cmbDegreeEnd.Text} | GPA: {txtGPA.Text}";
            }
            else if (level.Contains("A/L"))
            {
                newItem.MainTitle = $"G.C.E. Advanced Level ({cmbStream.Text})";
                newItem.Details = $"Year: {cmbALYear.Text} | Index: {txtALIndex.Text}\nResults: {txtALResults.Text}";
            }
            else if (level.Contains("O/L"))
            {
                newItem.MainTitle = "G.C.E. Ordinary Level";
                newItem.Details = $"Year: {cmbOLYear.Text} | Index: {txtOLIndex.Text}\nResults: {txtOLResults.Text}";
            }

            EducationList.Add(newItem);

            // 🔴 වෙනස 3: අලුතින් Education එකක් Add කරපු ගමන් JSON ෆයිල් එකට Save කරනවා.
            CVDataStore.Save();

            // Clear fields after successful addition
            cmbInstitution.Text = string.Empty;
            cmbDegreeStart.Text = string.Empty;
            cmbDegreeEnd.Text = string.Empty;
            txtGPA.Clear();

            cmbALYear.Text = string.Empty;
            txtALIndex.Clear();
            txtALResults.Clear();

            cmbOLYear.Text = string.Empty;
            txtOLIndex.Clear();
            txtOLResults.Clear();
        }

        private void RemoveEducation_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.DataContext is EducationItem item)
            {
                EducationList.Remove(item);

                // 🔴 වෙනස 4: ලිස්ට් එකෙන් එකක් Remove කරපු ගමන් JSON ෆයිල් එක අප්ඩේට් කරනවා.
                CVDataStore.Save();
            }
        }
    }

    
}