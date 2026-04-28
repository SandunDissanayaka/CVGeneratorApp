using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CVGeneratorApp
{
    public partial class LanguagesControl : UserControl
    {
        public ObservableCollection<LanguageItem> LanguageList { get; set; } = new ObservableCollection<LanguageItem>();

        public LanguagesControl()
        {
            InitializeComponent();
            lstLanguages.ItemsSource = LanguageList;
        }

        private void btnAddLanguage_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(cmbLanguage.Text))
            {
                MessageBox.Show("Please enter or select a language.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            LanguageList.Add(new LanguageItem
            {
                LanguageName = cmbLanguage.Text,
                Proficiency = ((ComboBoxItem)cmbProficiency.SelectedItem).Content.ToString()
            });

            cmbLanguage.Text = string.Empty;
        }

        private void RemoveLanguage_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.DataContext is LanguageItem item)
            {
                LanguageList.Remove(item);
            }
        }
    }
}