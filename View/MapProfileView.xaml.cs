using scriabinWPF.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace scriabinWPF
{
    /// <summary>
    /// Interaction logic for MapProfileView.xaml
    /// </summary>
    public partial class MapProfileView : UserControl
    {
        public MapProfileView()
        {
            InitializeComponent();
        }
        void AddNoteLink_Click(object sender, RoutedEventArgs e)
        {
            ((MapProfileViewModel)this.DataContext).AddNoteLink();

        }
        void AddNoteOnLink_Click(object sender, RoutedEventArgs e)
        {
            ((MapProfileViewModel)this.DataContext).AddNoteOnLink();
        }
        void AddNoteOffLink_Click(object sender, RoutedEventArgs e)
        {
            ((MapProfileViewModel)this.DataContext).AddNoteOffLink();
        }
        void AddControlChangeLink_Click(object sender, RoutedEventArgs e)
        {
            ((MapProfileViewModel)this.DataContext).AddControlChangeLink();
        }
        private void OnDeleteLinkClicked(object sender, EventArgs e)
        {
            if (sender is NoteLinkView noteLinkView)
            {
                ((MapProfileViewModel)this.DataContext).RemoveLink((AbstractLinkViewModel)noteLinkView.DataContext);
            }
            else if (sender is NoteOnLinkView noteOnLinkView)
            {
                ((MapProfileViewModel)this.DataContext).RemoveLink((AbstractLinkViewModel)noteOnLinkView.DataContext);
            }
            else if (sender is NoteOffLinkView noteOffLinkView)
            {
                ((MapProfileViewModel)this.DataContext).RemoveLink((AbstractLinkViewModel)noteOffLinkView.DataContext);
            }
            else if (sender is NoteOnLinkView controlChangeLinkView)
            {
                ((MapProfileViewModel)this.DataContext).RemoveLink((AbstractLinkViewModel)controlChangeLinkView.DataContext);
            }
        }
    }
}
