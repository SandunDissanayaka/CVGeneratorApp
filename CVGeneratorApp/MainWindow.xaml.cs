using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CVGeneratorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // App eka close karana code eka
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Window eka mouse eken drag karanna puluwan karana code eka
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        // Create account button eke code eka
        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            // Register window eka haduwama meka pawichchi karamu
            // RegisterWindow regWindow = new RegisterWindow();
            // regWindow.Show();
            // this.Close(); 
        }
    }
}