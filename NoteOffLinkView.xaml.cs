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
    /// Interaction logic for NoteOffLinkView.xaml
    /// </summary>
    public partial class NoteOffLinkView : UserControl
    {
        //private Binding midiChannelBinding = new Binding("MidiChannel")
        //{
        //    Mode = BindingMode.TwoWay,
        //    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        //};
        //private Binding pitchBinding = new Binding("Pitch")
        //{
        //    Mode = BindingMode.TwoWay,
        //    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        //};
        //private Binding dmxChannelBinding = new Binding("DmxChannel")
        //{
        //    Mode = BindingMode.TwoWay,
        //    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        //};
        //private Binding dmxValueBinding = new Binding("DmxValue")
        //{
        //    Mode = BindingMode.TwoWay,
        //    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        //};
        public NoteOffLinkView()
        {
            InitializeComponent();
            //MidiChannelUpDown.SetBinding(TextBox.TextProperty, midiChannelBinding);
            //PitchUpDown.SetBinding(TextBox.TextProperty, pitchBinding);
            //DmxChannelUpDown.SetBinding(TextBox.TextProperty, dmxChannelBinding);
            //DmxValueUpDown.SetBinding(TextBox.TextProperty, dmxValueBinding);
        }

        public event EventHandler? DeleteLinkClicked;

        private void DeleteLink_Click(object sender, RoutedEventArgs e)
        {
            DeleteLinkClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
