using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CVGeneratorApp
{
    public partial class SkillsControl : UserControl
    {
        // Collection to store project items for the list
        public ObservableCollection<ProjectItem> ProjectList { get; set; } = new ObservableCollection<ProjectItem>();

        public SkillsControl()
        {
            InitializeComponent();
            lstProjects.ItemsSource = ProjectList; // Bind collection to the ItemsControl
        }

        // --- SKILLS LOGIC ---

        private void btnAddTechSkill_Click(object sender, RoutedEventArgs e)
        {
            AddSkill(cmbTechInput, pnlTechTags, "#E3F2FD", "#1976D2"); // Blue style for Tech
        }

        private void btnAddSoftSkill_Click(object sender, RoutedEventArgs e)
        {
            AddSkill(cmbSoftInput, pnlSoftTags, "#F1F8E9", "#689F38"); // Green style for Soft Skills
        }

        // Generic method to add a skill tag
        private void AddSkill(ComboBox input, WrapPanel panel, string bgColor, string borderColor)
        {
            string skillText = input.Text.Trim();
            if (!string.IsNullOrEmpty(skillText))
            {
                CreateSkillTag(skillText, panel, bgColor, borderColor);
                input.Text = string.Empty;
            }
        }

        private void CreateSkillTag(string skill, WrapPanel panel, string bgColor, string borderColor)
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
            btnDelete.Click += (s, e) => panel.Children.Remove(tagBorder);

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
            }
        }
    }

    public class ProjectItem
    {
        // Adding '= string.Empty' tells C# these won't be null
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}