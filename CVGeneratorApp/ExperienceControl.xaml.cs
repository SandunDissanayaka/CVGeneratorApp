using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CVGeneratorApp
{
    public partial class ExperienceControl : UserControl
    {
        public ObservableCollection<ExperienceItem> ExperienceList { get; set; }

        public ExperienceControl()
        {
            InitializeComponent();
            ExperienceList = new ObservableCollection<ExperienceItem>();
            lstExperience.ItemsSource = ExperienceList;
        }

        private void btnAddExperience_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbJobTitle.Text) && !string.IsNullOrEmpty(txtCompany.Text))
            {
                ExperienceList.Add(new ExperienceItem
                {
                    JobTitle = cmbJobTitle.Text,
                    Company = txtCompany.Text,
                    DurationLocation = txtDuration.Text + " | " + txtLocation.Text,
                    Responsibilities = txtResponsibilities.Text
                });

                // Clear fields
                cmbJobTitle.Text = "";
                txtCompany.Clear();
                txtDuration.Clear();
                txtLocation.Clear();
                txtResponsibilities.Clear();
            }
            else
            {
                MessageBox.Show("Please fill the Job Title and Company fields.", "Input Error");
            }
        }

        private void RemoveExperience_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var item = button.DataContext as ExperienceItem;
            if (item != null) ExperienceList.Remove(item);
        }
    }

    public class ExperienceItem
    {
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public string DurationLocation { get; set; }
        public string Responsibilities { get; set; }
    }
}