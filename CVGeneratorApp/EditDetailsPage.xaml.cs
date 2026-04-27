using System;
using System.Windows;
using System.Windows.Controls;

namespace CVGeneratorApp
{
    /// <summary>
    /// Logic for navigating between different CV sections within the Edit Details page.
    /// </summary>
    public partial class EditDetailsPage : Page
    {
        public EditDetailsPage()
        {
            InitializeComponent();

            // Load Personal Info as the default view when the page opens
            SubContentFrame.Navigate(new PersonalInfoControl());

            // Highlight the 'Personal Info' button in the sidebar by default
            UpdateSubTabSelection(btnPersonalInfo);
        }

        /// <summary>
        /// Resets the styles of all sub-tab buttons and highlights the currently active one.
        /// </summary>
        /// <param name="activeBtn">The button that was clicked/selected</param>
        private void UpdateSubTabSelection(Button activeBtn)
        {
            // Reset all sub-tab buttons to the default 'SubTabButton' style
            btnPersonalInfo.Style = (Style)FindResource("SubTabButton");
            btnEducation.Style = (Style)FindResource("SubTabButton");
            btnExperience.Style = (Style)FindResource("SubTabButton");
            btnSkills.Style = (Style)FindResource("SubTabButton");

            // Apply the 'ActiveSubTabButton' style to the selected button
            activeBtn.Style = (Style)FindResource("ActiveSubTabButton");
        }

        /// <summary>
        /// Handles click events for all sidebar sub-tabs to switch between different forms.
        /// </summary>
        private void SubTab_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;

            // Update the UI to show which tab is currently selected
            UpdateSubTabSelection(clickedButton);

            // Get the text content of the button to identify the target section
            string sectionName = clickedButton.Content.ToString();

            // 1. Navigation for the Personal Information section
            if (sectionName.Contains("Personal"))
            {
                txtSectionTitle.Text = "Personal Information";
                SubContentFrame.Navigate(new PersonalInfoControl());
            }
            // 2. Navigation for the Education section
            else if (sectionName.Contains("Education"))
            {
                txtSectionTitle.Text = "Education Details";
                SubContentFrame.Navigate(new EducationControl());
            }
            // 3. Navigation for the Work Experience section
            else if (sectionName.Contains("Work"))
            {
                txtSectionTitle.Text = "Work Experience";
                SubContentFrame.Navigate(new ExperienceControl());
            }
            // 4. Navigation for the Skills & Projects section
            else if (sectionName.Contains("Skills"))
            {
                txtSectionTitle.Text = "Skills & Projects";
                // Now navigating to the SkillsControl we just implemented
                SubContentFrame.Navigate(new SkillsControl());
            }
        }
    }
}