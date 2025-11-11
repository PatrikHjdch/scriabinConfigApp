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
    public partial class MainView : Window
    {
        public MainView()
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

        private void AddLink_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as MainViewModel)?.AddLink();
        }

        private void OnDeleteLinkClicked(object sender, EventArgs e)
        {
            if (sender is NoteLinkView noteLinkView)
            {
                (this.DataContext as MainViewModel)?.RemoveLink(noteLinkView.DataContext as AbstractLinkModel);
            }
            else if (sender is NoteOnLinkView noteOnLinkView)
            {
                (this.DataContext as MainViewModel)?.RemoveLink(noteOnLinkView.DataContext as AbstractLinkModel);
            }
            else if (sender is NoteOffLinkView noteOffLinkView)
            {
                (this.DataContext as MainViewModel)?.RemoveLink(noteOffLinkView.DataContext as AbstractLinkModel);
            }
            else if (sender is ControlChangeLinkView controlChangeLinkView)
            {
                (this.DataContext as MainViewModel)?.RemoveLink(controlChangeLinkView.DataContext as AbstractLinkModel);
            }
        }
        private void AddNoteLink_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as MainViewModel)?.AddNoteLink();
        }

        private void AddNoteOnLink_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as MainViewModel)?.AddNoteOnLink();
        }

        private void AddNoteOffLink_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as MainViewModel)?.AddNoteOffLink();
        }

        private void AddControlChangeLink_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as MainViewModel)?.AddControlChangeLink();
        }
    }
}