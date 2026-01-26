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

namespace scriabinWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }

        private void NewFile_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("New File Clicked");
        }
        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Open File Clicked");
        }
        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Save File Clicked");
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("About This App Clicked");
        }

        private void RefreshComPorts_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as MainViewModel)?.RefreshComPorts();
        }

        private void UploadConfig_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as MainViewModel)?.UploadMap();
        }
        private void DownloadConfig_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Download Config Clicked");
        }
        private void UpdateComPort(object sender, RoutedEventArgs e)
        {
            (this.DataContext as MainViewModel)?.UpdateComPort();
        }

        private void TestConnection_Click(object sender, RoutedEventArgs e)
        {
            int response = (this.DataContext as MainViewModel)?.TestConnection() ?? -1;
            switch (response)
            {
                case 0:
                    MessageBox.Show("Connection successful.");
                    break;
                case 1:
                    MessageBox.Show("Connection timed out.");
                    break;
                case 2:
                    MessageBox.Show("Device returned an error.");
                    break;
                case 3:
                    MessageBox.Show("Unexpected response from device.");
                    break;
                default:
                    MessageBox.Show("Unknown error.");
                    break;
            }
        }
    }
}