using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CVGeneratorApp
{
    public partial class EducationControl : UserControl
    {
        // This list will hold the education items added by the user
        public ObservableCollection<EducationItem> EducationList { get; set; }

        public EducationControl()
        {
            InitializeComponent();
            EducationList = new ObservableCollection<EducationItem>();
            lstEducation.ItemsSource = EducationList; // Link the list to the UI
        }

        private void btnAddEducation_Click(object sender, RoutedEventArgs e)
        {
            // 1. Get data from inputs
            string university = cmbUniversity.Text;
            string degree = cmbDegree.Text;
            string duration = txtStartYear.Text + " - " + txtEndYear.Text;
            string gpa = txtGPA.Text;

            // 2. Simple validation to check if fields are not empty
            if (!string.IsNullOrEmpty(university) && !string.IsNullOrEmpty(degree))
            {
                // 3. Create a new education item and add it to the list
                EducationList.Add(new EducationItem
                {
                    University = university,
                    Degree = degree,
                    Duration = duration,
                    GPA = gpa
                });

                // 4. Clear inputs for the next entry
                cmbUniversity.Text = "";
                cmbDegree.Text = "";
                txtStartYear.Clear();
                txtEndYear.Clear();
                txtGPA.Clear();
            }
            else
            {
                MessageBox.Show("Please fill at least the University and Degree fields.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RemoveEducation_Click(object sender, RoutedEventArgs e)
        {
            // Find which item's 'Remove' button was clicked
            var button = sender as Button;
            var item = button.DataContext as EducationItem;

            // Remove it from the list
            if (item != null)
            {
                EducationList.Remove(item);
            }
        }
    }

    // A simple class to store education data
    public class EducationItem
    {
        public string University { get; set; }
        public string Degree { get; set; }
        public string Duration { get; set; }
        public string GPA { get; set; }
    }
}