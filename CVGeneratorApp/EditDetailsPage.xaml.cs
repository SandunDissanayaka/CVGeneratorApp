using System;
using System.Windows;
using System.Windows.Controls;

namespace CVGeneratorApp
{
    public partial class EditDetailsPage : Page
    {
        public EditDetailsPage()
        {
            InitializeComponent();

            // Attach the event handler to update UI automatically when pages change (Forward or Back)
            SubContentFrame.Navigated += SubContentFrame_Navigated;

            // Load the default starting page
            SubContentFrame.Navigate(new PersonalInfoControl());
        }

        /// <summary>
        /// Automatically updates the Sidebar Highlight, Page Title, and Buttons
        /// whenever the Frame navigates to a new page.
        /// </summary>
        private void SubContentFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            var content = SubContentFrame.Content;

            // --- Reset buttons to default state for all pages ---
            if (btnSaveNext != null) btnSaveNext.Content = "Save & Next";
            if (btnGoToTemplates != null) btnGoToTemplates.Visibility = Visibility.Collapsed;

            // 1. Manage Back Button Visibility
            if (content is PersonalInfoControl)
            {
                // Hide on the first page
                btnBack.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Show on all other pages
                btnBack.Visibility = Visibility.Visible;
            }

            // 2. Update Sidebar Highlight, Title text, and specific buttons based on the active page
            if (content is PersonalInfoControl)
            {
                UpdateSubTabSelection(btnPersonalInfo);
                txtSectionTitle.Text = "Personal Information";
            }
            else if (content is EducationControl)
            {
                UpdateSubTabSelection(btnEducation);
                txtSectionTitle.Text = "Education Details";
            }
            else if (content is ExperienceControl)
            {
                UpdateSubTabSelection(btnExperience);
                txtSectionTitle.Text = "Work Experience";
            }
            else if (content is SkillsControl)
            {
                UpdateSubTabSelection(btnSkills);
                txtSectionTitle.Text = "Skills & Projects";
            }
            else if (content is LanguagesControl)
            {
                UpdateSubTabSelection(btnLanguages);
                txtSectionTitle.Text = "Languages";

                // Update buttons specifically for the final page
                if (btnSaveNext != null) btnSaveNext.Content = "Save";
                if (btnGoToTemplates != null) btnGoToTemplates.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Handles manual navigation when a user clicks a button in the sidebar.
        /// </summary>
        private void SubTab_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                switch (btn.Name)
                {
                    case "btnPersonalInfo":
                        SubContentFrame.Navigate(new PersonalInfoControl());
                        break;
                    case "btnEducation":
                        SubContentFrame.Navigate(new EducationControl());
                        break;
                    case "btnExperience":
                        SubContentFrame.Navigate(new ExperienceControl());
                        break;
                    case "btnSkills":
                        SubContentFrame.Navigate(new SkillsControl());
                        break;
                    case "btnLanguages":
                        SubContentFrame.Navigate(new LanguagesControl());
                        break;
                }
            }
        }

        /// <summary>
        /// Handles the "Save & Next" button logic. Saves data for the current page,
        /// then navigates to the next logical page.
        /// </summary>
        private void btnSaveNext_Click(object sender, RoutedEventArgs e)
        {
            var currentContent = SubContentFrame.Content;

            if (currentContent is PersonalInfoControl personalPage)
            {
                // Capture data from UI
                var data = CVDataStore.Profile.PersonalInfo;
                data.FullName = personalPage.txtFullName.Text;
                data.JobTitle = personalPage.cmbTitle.Text;
                data.Email = personalPage.txtEmail.Text;
                data.Phone = personalPage.txtPhone.Text;
                data.LinkedIn = personalPage.txtLinkedIn.Text;
                data.GitHub = personalPage.txtGitHub.Text;
                data.Location = personalPage.txtAddress.Text;

                // Save persistent data to JSON
                CVDataStore.Save();

                // Navigate to the next section
                SubContentFrame.Navigate(new EducationControl());
            }
            else if (currentContent is EducationControl)
            {
                SubContentFrame.Navigate(new ExperienceControl());
            }
            else if (currentContent is ExperienceControl)
            {
                SubContentFrame.Navigate(new SkillsControl());
            }
            else if (currentContent is SkillsControl)
            {
                SubContentFrame.Navigate(new LanguagesControl());
            }
            else if (currentContent is LanguagesControl)
            {
                // On the last page, just save the data. No need to navigate.
                CVDataStore.Save();
                MessageBox.Show("All details saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Navigates back to the previous logical page manually.
        /// </summary>
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var currentContent = SubContentFrame.Content;

            if (currentContent is EducationControl)
            {
                SubContentFrame.Navigate(new PersonalInfoControl());
            }
            else if (currentContent is ExperienceControl)
            {
                SubContentFrame.Navigate(new EducationControl());
            }
            else if (currentContent is SkillsControl)
            {
                SubContentFrame.Navigate(new ExperienceControl());
            }
            else if (currentContent is LanguagesControl)
            {
                SubContentFrame.Navigate(new SkillsControl());
            }
        }

        /// <summary>
        /// Handles the new "Go to Templates" button logic.
        /// </summary>
        // --- GO TO TEMPLATES LOGIC ---
        private void btnGoToTemplates_Click(object sender, RoutedEventArgs e)
        {
            // Save any remaining data
            CVDataStore.Save();

            // Find the main parent window (DashboardWindow)
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                // Find the 'Templates & Preview' button in the main sidebar by its x:Name.
                // 🔴 IMPORTANT: If your button is named something else (e.g., "btnPreview"), change "btnTemplates" below!
                Button templatesSidebarButton = parentWindow.FindName("btnTemplates") as Button;

                if (templatesSidebarButton != null)
                {
                    // Simulate a physical mouse click on that sidebar button
                    templatesSidebarButton.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
                    return;
                }
            }

            // Fallback navigation if the button is not found
            this.NavigationService?.Navigate(new PreviewPage());
        }

        /// <summary>
        /// Resets all sidebar buttons to their default style, then applies the active style
        /// to the currently selected button.
        /// </summary>
        public void UpdateSubTabSelection(Button selectedButton)
        {
            btnPersonalInfo.Style = (Style)FindResource("SubTabButton");
            btnEducation.Style = (Style)FindResource("SubTabButton");
            btnExperience.Style = (Style)FindResource("SubTabButton");
            btnSkills.Style = (Style)FindResource("SubTabButton");
            btnLanguages.Style = (Style)FindResource("SubTabButton");

            selectedButton.Style = (Style)FindResource("ActiveSubTabButton");
        }
    }
}