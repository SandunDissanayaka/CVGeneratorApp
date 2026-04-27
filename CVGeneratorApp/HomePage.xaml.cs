using System;
using System.Windows;
using System.Windows.Controls;

namespace CVGeneratorApp
{
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void btnCreateNew_Click(object sender, RoutedEventArgs e)
        {
            // Find the DashboardWindow that contains this HomePage
            var dashboard = Window.GetWindow(this) as DashboardWindow;

            if (dashboard != null)
            {
                // We want the sidebar highlight to update as well.
                // So we programmatically 'click' the sidebar's Edit Details button.
                dashboard.btnEditDetails.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
    }
}