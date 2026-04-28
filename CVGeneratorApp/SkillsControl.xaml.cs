using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CVGeneratorApp
{
    public partial class SkillsControl : UserControl
    {
        // Property to link the local collection to the Global Data Store
        public ObservableCollection<ProjectItem> ProjectList { get; set; }

        public SkillsControl()
        {
            InitializeComponent();

            // Bind the JSON Data Store list to the UI ItemsControl
            ProjectList = CVDataStore.Profile.ProjectList;
            lstProjects.ItemsSource = ProjectList;

            // Load saved skills when the page opens
            LoadExistingSkills();
        }

        // --- SKILL LOADING LOGIC ---
        private void LoadExistingSkills()
        {
            // Clear existing UI elements to prevent duplicates
            pnlTechTags.Children.Clear();
            pnlSoftTags.Children.Clear();

            // Load technical skills
            if (CVDataStore.Profile.TechnicalSkills != null)
            {
                foreach (var skill in CVDataStore.Profile.TechnicalSkills)
                {
                    pnlTechTags.Children.Add(CreateSkillTag(skill, true));
                }
            }

            // Load soft skills
            if (CVDataStore.Profile.SoftSkills != null)
            {
                foreach (var skill in CVDataStore.Profile.SoftSkills)
                {
                    pnlSoftTags.Children.Add(CreateSkillTag(skill, false));
                }
            }
        }

        // --- ADD PROJECT LOGIC ---
        private void btnAddProject_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtProjectTitle.Text))
            {
                ProjectList.Add(new ProjectItem
                {
                    Title = txtProjectTitle.Text,
                    Description = txtProjectDesc.Text
                });

                CVDataStore.Save();

                txtProjectTitle.Clear();
                txtProjectDesc.Clear();
            }
            else
            {
                MessageBox.Show("Please enter a Project Title.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // --- REMOVE PROJECT LOGIC ---
        private void RemoveProject_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var item = button?.DataContext as ProjectItem;

            if (item != null)
            {
                ProjectList.Remove(item);
                CVDataStore.Save();
            }
        }

        // --- ADD TECHNICAL SKILL LOGIC ---
        private void btnAddTechSkill_Click(object sender, RoutedEventArgs e)
        {
            string skill = cmbTechInput.Text.Trim();

            // Check if input is valid and not already in the list
            if (!string.IsNullOrWhiteSpace(skill) && !CVDataStore.Profile.TechnicalSkills.Contains(skill))
            {
                // Save to Data Store
                CVDataStore.Profile.TechnicalSkills.Add(skill);
                CVDataStore.Save();

                // Add tag visually to the UI
                pnlTechTags.Children.Add(CreateSkillTag(skill, true));

                // Clear input
                cmbTechInput.Text = string.Empty;
            }
        }

        // --- ADD SOFT SKILL LOGIC ---
        private void btnAddSoftSkill_Click(object sender, RoutedEventArgs e)
        {
            string skill = cmbSoftInput.Text.Trim();

            // Check if input is valid and not already in the list
            if (!string.IsNullOrWhiteSpace(skill) && !CVDataStore.Profile.SoftSkills.Contains(skill))
            {
                // Save to Data Store
                CVDataStore.Profile.SoftSkills.Add(skill);
                CVDataStore.Save();

                // Add tag visually to the UI
                pnlSoftTags.Children.Add(CreateSkillTag(skill, false));

                // Clear input
                cmbSoftInput.Text = string.Empty;
            }
        }

        // --- HELPER METHOD: CREATE VISUAL TAG ---
        /// <summary>
        /// Creates a rounded UI element (Tag) for a skill with a remove button.
        /// </summary>
        private Border CreateSkillTag(string skillText, bool isTechSkill)
        {
            var brushConverter = new BrushConverter();

            // The outer container (rounded border)
            Border border = new Border
            {
                Background = (Brush)brushConverter.ConvertFromString("#E6F2FF"),
                BorderBrush = (Brush)brushConverter.ConvertFromString("#007ACC"),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(12, 6, 8, 6),
                Margin = new Thickness(0, 0, 10, 10)
            };

            StackPanel stack = new StackPanel { Orientation = Orientation.Horizontal };

            // The text element
            TextBlock text = new TextBlock
            {
                Text = skillText,
                Foreground = (Brush)brushConverter.ConvertFromString("#007ACC"),
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 8, 0)
            };

            // The remove ('X') button
            Button removeBtn = new Button
            {
                Content = "✕",
                Background = Brushes.Transparent,
                Foreground = (Brush)brushConverter.ConvertFromString("#007ACC"),
                BorderThickness = new Thickness(0),
                Cursor = System.Windows.Input.Cursors.Hand,
                VerticalAlignment = VerticalAlignment.Center,
                FontWeight = FontWeights.Bold,
                Padding = new Thickness(0)
            };

            // Logic to remove the skill when 'X' is clicked
            removeBtn.Click += (s, e) =>
            {
                if (isTechSkill)
                {
                    pnlTechTags.Children.Remove(border);
                    CVDataStore.Profile.TechnicalSkills.Remove(skillText);
                }
                else
                {
                    pnlSoftTags.Children.Remove(border);
                    CVDataStore.Profile.SoftSkills.Remove(skillText);
                }
                CVDataStore.Save();
            };

            // Assemble the tag
            stack.Children.Add(text);
            stack.Children.Add(removeBtn);
            border.Child = stack;

            return border;
        }
    }
}