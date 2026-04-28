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

            // Default section when the page loads
            SubContentFrame.Navigate(new PersonalInfoControl());
        }

        // --- SIDEBAR NAVIGATION LOGIC ---
        private void SubTab_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                // Navigate based on which sidebar button was clicked
                switch (btn.Name)
                {
                    case "btnPersonalInfo":
                        SubContentFrame.Navigate(new PersonalInfoControl());
                        txtSectionTitle.Text = "Personal Information";
                        break;
                    case "btnEducation":
                        SubContentFrame.Navigate(new EducationControl());
                        txtSectionTitle.Text = "Education Details";
                        break;
                    case "btnExperience":
                        SubContentFrame.Navigate(new ExperienceControl());
                        txtSectionTitle.Text = "Work Experience";
                        break;
                    case "btnSkills":
                        SubContentFrame.Navigate(new SkillsControl());
                        txtSectionTitle.Text = "Skills & Projects";
                        break;
                    case "btnLanguages":
                        SubContentFrame.Navigate(new LanguagesControl());
                        txtSectionTitle.Text = "Languages";
                        break;
                }
                UpdateSubTabSelection(btn);
            }
        }

        // --- SAVE & NEXT BUTTON LOGIC ---
        private void btnSaveNext_Click(object sender, RoutedEventArgs e)
        {
            var currentContent = SubContentFrame.Content;

            // 1. If currently on Personal Info, Save data and Move to Education
            if (currentContent is PersonalInfoControl personalPage)
            {
                var data = CVDataStore.Profile.PersonalInfo;
                data.FullName = personalPage.txtFullName.Text;
                data.JobTitle = personalPage.cmbTitle.Text;
                data.Email = personalPage.txtEmail.Text;
                data.Phone = personalPage.txtPhone.Text;
                data.LinkedIn = personalPage.txtLinkedIn.Text;
                data.GitHub = personalPage.txtGitHub.Text;
                data.Location = personalPage.txtAddress.Text;

                CVDataStore.Save(); // Persistent JSON Save

                SubContentFrame.Navigate(new EducationControl());
                UpdateSubTabSelection(btnEducation);
                txtSectionTitle.Text = "Education Details";
            }
            // 2. Navigation logic for other pages
            else if (currentContent is EducationControl)
            {
                SubContentFrame.Navigate(new ExperienceControl());
                UpdateSubTabSelection(btnExperience);
                txtSectionTitle.Text = "Work Experience";
            }
            else if (currentContent is ExperienceControl)
            {
                SubContentFrame.Navigate(new SkillsControl());
                UpdateSubTabSelection(btnSkills);
                txtSectionTitle.Text = "Skills & Projects";
            }
        }

        // --- BACK BUTTON LOGIC ---
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (SubContentFrame.CanGoBack)
            {
                SubContentFrame.GoBack();
                // Optionally add code here to sync the sidebar highlight
            }
        }

        // --- HELPER: Update Sidebar Styles ---
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