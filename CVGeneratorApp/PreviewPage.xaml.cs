using System;
using System.Windows;
using System.Windows.Controls;
using CVGeneratorApp.Templates; // Important namespace for your templates

namespace CVGeneratorApp
{
    public partial class PreviewPage : Page
    {
        public PreviewPage()
        {
            InitializeComponent();
        }

        // Shows the Preview Screen when a template is selected
        private void btnSelectTemp1_Click(object sender, RoutedEventArgs e)
        {
            pnlSelection.Visibility = Visibility.Collapsed;
            pnlPreview.Visibility = Visibility.Visible;

            // Load Template 1 into the container
            TemplateContainer.Content = new Template1Control();

            // Logic to fit the CV entirely on the screen without requiring scrolling
            // Uses Dispatcher to calculate the available height only after the UI has fully loaded
            Dispatcher.InvokeAsync(() =>
            {
                double availableHeight = scrollArea.ActualHeight - 40; // 40 is for top/bottom margins
                if (availableHeight > 0)
                {
                    // Standard A4 height is 1123 pixels. Calculate the scale accordingly.
                    double fitScale = availableHeight / 1123.0;

                    // Cap the zoom levels to keep it within sensible bounds
                    if (fitScale > 1.0) fitScale = 1.0;
                    if (fitScale < 0.2) fitScale = 0.2;

                    zoomSlider.Value = fitScale; // Auto-adjust the zoom slider
                }
            }, System.Windows.Threading.DispatcherPriority.Loaded);
        }

        // Navigates back to the Template Selection screen
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            pnlPreview.Visibility = Visibility.Collapsed;
            pnlSelection.Visibility = Visibility.Visible;

            // Set content to null to clear memory when leaving the preview
            TemplateContainer.Content = null;
        }

        // Handles the PDF Export button click
        private void btnExportPDF_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("The PDF Export feature will be added in the next step!", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}