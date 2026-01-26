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
    /// Interaction logic for LinkProfileTab.xaml
    /// </summary>
    public partial class LinkProfileTabView : UserControl
    {
        public LinkProfileTabView()
        {
        }

        public void AddNoteLink_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is LinkProfileTabViewModel vm)
            {
                vm.AddNoteLink();
            }
        }
        private void AddNoteOnLink_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is LinkProfileTabViewModel vm)
            {
                vm.AddNoteOnLink();
            }
        }
        private void AddNoteOffLink_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is LinkProfileTabViewModel vm)
            {
                vm.AddNoteOffLink();
            }
        }
        private void AddControlChangeLink_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is LinkProfileTabViewModel vm)
            {
                vm.AddControlChangeLink();
            }
        }
    }
}
