using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CVGeneratorApp
{
    public partial class SkillsControl : UserControl
    {
        // Link to Global Store Collections
        public ObservableCollection<ProjectItem> ProjectList { get; set; }
        public ObservableCollection<SkillItem> SkillList { get; set; }

        public SkillsControl()
        {
            InitializeComponent();

            // 1. Link the local properties to the Global Store
            ProjectList = CVDataStore.Profile.ProjectList;
            SkillList = CVDataStore.Profile.SkillList;

            lstProjects.ItemsSource = ProjectList;

            // 2. Load existing skills from the store and create tags on screen
            LoadExistingSkills();
        }

        private void LoadExistingSkills()
        {
            // Clear current UI panels
            pnlTechTags.Children.Clear();
            pnlSoftTags.Children.Clear();

            // Loop through the saved skills and recreate the UI Tags
            foreach (var skill in SkillList)
            {
                // We determine if it's Tech or Soft skill based on some logic or just add to tech for now
                // For a more advanced version, you can add a 'Type' property to SkillItem
                CreateSkillTag(skill.Name, pnlTechTags, "#E3F2FD", "#1976D2", skill);
            }
        }

        // --- SKILLS LOGIC ---

        private void btnAddTechSkill_Click(object sender, RoutedEventArgs e)
        {
            AddSkill(cmbTechInput, pnlTechTags, "#E3F2FD", "#1976D2");
        }

        private void btnAddSoftSkill_Click(object sender, RoutedEventArgs e)
        {
            AddSkill(cmbSoftInput, pnlSoftTags, "#F1F8E9", "#689F38");
        }

        private void AddSkill(ComboBox input, WrapPanel panel, string bgColor, string borderColor)
        {
            string skillText = input.Text.Trim();
            if (!string.IsNullOrEmpty(skillText))
            {
                // Create the data object
                var newSkill = new SkillItem { Name = skillText };
                SkillList.Add(newSkill);

                // Create the UI Tag
                CreateSkillTag(skillText, panel, bgColor, borderColor, newSkill);

                input.Text = string.Empty;
                CVDataStore.Save(); // 🔴 Save to JSON
            }
        }

        private void CreateSkillTag(string skill, WrapPanel panel, string bgColor, string borderColor, SkillItem dataItem)
        {
            Border tagBorder = new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(bgColor)),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(borderColor)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(12, 5, 12, 5),
                Margin = new Thickness(0, 0, 8, 8)
            };

            StackPanel stack = new StackPanel { Orientation = Orientation.Horizontal };
            TextBlock txt = new TextBlock { Text = skill, Foreground = tagBorder.BorderBrush, VerticalAlignment = VerticalAlignment.Center };

            Button btnDelete = new Button { Content = " ✕", Foreground = Brushes.Gray, Background = Brushes.Transparent, BorderThickness = new Thickness(0), Cursor = System.Windows.Input.Cursors.Hand, Margin = new Thickness(5, 0, 0, 0) };

            // Delete logic
            btnDelete.Click += (s, e) => {
                panel.Children.Remove(tagBorder);
                SkillList.Remove(dataItem); // Remove from Global Store
                CVDataStore.Save(); // 🔴 Save to JSON
            };

            stack.Children.Add(txt);
            stack.Children.Add(btnDelete);
            tagBorder.Child = stack;
            panel.Children.Add(tagBorder);
        }

        // --- PROJECTS LOGIC ---

        private void btnAddProject_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtProjectTitle.Text))
            {
                ProjectList.Add(new ProjectItem
                {
                    Title = txtProjectTitle.Text,
                    Description = txtProjectDesc.Text
                });

                CVDataStore.Save(); // 🔴 Save to JSON

                txtProjectTitle.Clear();
                txtProjectDesc.Clear();
            }
        }

        private void RemoveProject_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.DataContext is ProjectItem item)
            {
                ProjectList.Remove(item);
                CVDataStore.Save(); // 🔴 Save to JSON
            }
        }
    }
}