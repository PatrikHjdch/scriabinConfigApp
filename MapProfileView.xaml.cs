using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
