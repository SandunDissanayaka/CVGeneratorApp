using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace CVGeneratorApp
{
    // --- GLOBAL DATA STORE ---
    public static class CVDataStore
    {
        public static CVProfile Profile { get; set; } = new CVProfile();

        private static readonly string FilePath = "cv_userdata.json";

        public static void Load()
        {
            if (File.Exists(FilePath))
            {
                try
                {
                    string json = File.ReadAllText(FilePath);
                    Profile = JsonSerializer.Deserialize<CVProfile>(json) ?? new CVProfile();
                }
                catch
                {
                    Profile = new CVProfile();
                }
            }
        }

        public static void Save()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(Profile, options);
            File.WriteAllText(FilePath, json);
        }
    }

    // --- MAIN PROFILE STRUCTURE ---
    public class CVProfile
    {
        public PersonalInfo PersonalInfo { get; set; } = new PersonalInfo();

        public ObservableCollection<EducationItem> EducationList { get; set; } = new ObservableCollection<EducationItem>();
        public ObservableCollection<ExperienceItem> ExperienceList { get; set; } = new ObservableCollection<ExperienceItem>();
        public ObservableCollection<ProjectItem> ProjectList { get; set; } = new ObservableCollection<ProjectItem>();
        public ObservableCollection<LanguageItem> LanguageList { get; set; } = new ObservableCollection<LanguageItem>();

        public List<string> TechnicalSkills { get; set; } = new List<string>();
        public List<string> SoftSkills { get; set; } = new List<string>();
    }

    // --- DATA MODELS FOR EACH SECTION ---

    public class PersonalInfo
    {
        public string FullName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string LinkedIn { get; set; } = string.Empty;
        public string GitHub { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string PhotoPath { get; set; } = string.Empty;
    }

    public class EducationItem
    {
        // Core data fields
        public string Level { get; set; } = string.Empty;
        public string Institution { get; set; } = string.Empty;
        public string StartYear { get; set; } = string.Empty;
        public string EndYear { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public string GPA { get; set; } = string.Empty;

        // UI display properties
        public string MainTitle { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }

    public class ExperienceItem
    {
        // Core data fields
        public string JobTitle { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string StartDate { get; set; } = string.Empty;
        public string EndDate { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // UI display properties
        public string MainTitle { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string DurationLocation { get; set; } = string.Empty;
        public string Responsibilities { get; set; } = string.Empty;
    }

    public class ProjectItem
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class LanguageItem
    {
        public string LanguageName { get; set; } = string.Empty;
        public string Proficiency { get; set; } = string.Empty;
    }
}