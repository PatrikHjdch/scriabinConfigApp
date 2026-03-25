using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

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
        private bool LogScrollViewer_AutoScroll = true;
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
        private void DownloadConfig_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Download Config Clicked");
        }
        private void UploadConfig_Click(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)DataContext).StartUpload();
        }
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)DataContext).HandleConnect();
        }

        private void LogScrollViewer_ScrollChanged(object sender, System.Windows.Controls.ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange == 0) // vyska se nezmenila
            {
                LogScrollViewer_AutoScroll = LogScrollViewer.VerticalOffset == LogScrollViewer.ScrollableHeight; // pokud jsme na konci, autoscroll je zapnuty
            }
            if (LogScrollViewer_AutoScroll && e.ExtentHeightChange != 0)
            {
                LogScrollViewer.ScrollToBottom();
            }
        }

        private void LogFolderOpen_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer", "logs");
        }
    }
}