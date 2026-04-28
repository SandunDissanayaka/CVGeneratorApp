using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace CVGeneratorApp
{
    // --- SHARED DATA MODELS ---
    // (Moved here from UserControls to avoid errors)

    public class PersonalInfoModel
    {
        public string FullName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string LinkedIn { get; set; } = string.Empty;
        public string GitHub { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string PhotoPath { get; set; } = string.Empty;
    }

    public class EducationItem
    {
        public string MainTitle { get; set; } = string.Empty;
        public string Institution { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }

    public class LanguageItem
    {
        public string LanguageName { get; set; } = string.Empty;
        public string Proficiency { get; set; } = string.Empty;
    }

    public class ExperienceItem
    {
        public string JobTitle { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string DurationLocation { get; set; } = string.Empty;
        public string Responsibilities { get; set; } = string.Empty;
    }

    public class SkillItem { public string Name { get; set; } = string.Empty; }

    public class ProjectItem
    {
        public string Title { get; set; } = string.Empty;
        public string TechStack { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    // --- MAIN PROFILE CONTAINER ---

    public class CVProfile
    {
        public PersonalInfoModel PersonalInfo { get; set; } = new PersonalInfoModel();
        public ObservableCollection<EducationItem> EducationList { get; set; } = new ObservableCollection<EducationItem>();
        public ObservableCollection<ExperienceItem> ExperienceList { get; set; } = new ObservableCollection<ExperienceItem>();
        public ObservableCollection<SkillItem> SkillList { get; set; } = new ObservableCollection<SkillItem>();
        public ObservableCollection<ProjectItem> ProjectList { get; set; } = new ObservableCollection<ProjectItem>();
        public ObservableCollection<LanguageItem> LanguageList { get; set; } = new ObservableCollection<LanguageItem>();
    }

    // --- STATIC STORE WITH SAVE/LOAD ---

    public static class CVDataStore
    {
        private static readonly string filePath = "cv_userdata.json";
        public static CVProfile Profile { get; set; } = new CVProfile();

        public static void Save()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(Profile, options);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error saving: " + ex.Message);
            }
        }

        public static void Load()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    Profile = JsonSerializer.Deserialize<CVProfile>(json) ?? new CVProfile();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error loading: " + ex.Message);
                Profile = new CVProfile();
            }
        }
    }
}