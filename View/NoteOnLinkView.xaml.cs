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
    /// Interaction logic for NoteOnLinkControl.xaml
    /// </summary>
    public partial class NoteOnLinkView : UserControl
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
        //private Binding useVelocityBinding = new Binding("UsesVelocity")
        //{
        //    Mode = BindingMode.TwoWay,
        //    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        //};
        public NoteOnLinkView()
        {
            InitializeComponent();
            //MidiChannelUpDown.SetBinding(TextBox.TextProperty, midiChannelBinding);
            //PitchUpDown.SetBinding(TextBox.TextProperty, pitchBinding);
            //DmxChannelUpDown.SetBinding(TextBox.TextProperty, dmxChannelBinding);
            //DmxValueUpDown.SetBinding(TextBox.TextProperty, dmxValueBinding);
            //UseVelocityCheckBox.SetBinding(CheckBox.IsCheckedProperty, useVelocityBinding);
            UseVelocityCheckBox.Checked += (s, e) => { DmxValueUpDown.IsEnabled = false; };
            UseVelocityCheckBox.Unchecked += (s, e) => { DmxValueUpDown.IsEnabled = true; };
        }

        public event EventHandler? DeleteLinkClicked;
        private void DeleteLink_Click(object sender, RoutedEventArgs e)
        {
            DeleteLinkClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
