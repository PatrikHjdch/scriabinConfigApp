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
    /// Interaction logic for ControlChangeLinkView.xaml
    /// </summary>
    public partial class ControlChangeLinkView : UserControl
    {
        private Binding MidiChannelBinding = new Binding("MidiChannel")
        {
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        };
        private Binding ControlNumberBinding = new Binding("Pitch")
        {
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        };
        private Binding DmxChannelBinding = new Binding("DmxChannel")
        {
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        };
        public ControlChangeLinkView()
        {
            InitializeComponent();
            MidiChannelUpDown.SetBinding(TextBox.TextProperty, MidiChannelBinding);
            ControllerNumberUpDown.SetBinding(TextBox.TextProperty, ControlNumberBinding);
            DmxChannelUpDown.SetBinding(TextBox.TextProperty, DmxChannelBinding);
        }

        public event EventHandler? DeleteLinkClicked;

        private void DeleteLink_Click(object sender, RoutedEventArgs e)
        {
            DeleteLinkClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
