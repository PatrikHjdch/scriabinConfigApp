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
using scriabinWPF;

namespace scriabinWPF
{
    /// <summary>
    /// Interaction logic for LinkControl.xaml
    /// </summary>
    public partial class NoteLinkView : UserControl
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
        //private Binding dmxValueOnBinding = new Binding("DmxValueOn")
        //{
        //    Mode = BindingMode.TwoWay,
        //    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        //};
        //private Binding dmxValueOffBinding = new Binding("DmxValueOff")
        //{
        //    Mode = BindingMode.TwoWay,
        //    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        //};
        //private Binding timeoutBinding = new Binding("Timeout")
        //{
        //    Mode = BindingMode.TwoWay,
        //    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        //};
        //private Binding useVelocityBinding = new Binding("UsesVelocity")
        //{
        //    Mode = BindingMode.TwoWay,
        //    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        //};
        //private Binding timeoutEnableBinding = new Binding("TimeoutEnabled")
        //{
        //    Mode = BindingMode.TwoWay,
        //    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        //};

        public event EventHandler? DeleteLinkClicked;

        public NoteLinkView()
        {
            InitializeComponent();
            //MidiChannelUpDown.SetBinding(NumericUpDown.ValueProperty, midiChannelBinding);
            //PitchUpDown.SetBinding(NumericUpDown.ValueProperty, pitchBinding);
            //DmxChannelUpDown.SetBinding(NumericUpDown.ValueProperty, dmxChannelBinding);
            //DmxValueOnUpDown.SetBinding(NumericUpDown.ValueProperty, dmxValueOnBinding);
            //DmxValueOffUpDown.SetBinding(NumericUpDown.ValueProperty, dmxValueOffBinding);
            //TimeoutUpDown.SetBinding(NumericUpDown.ValueProperty, timeoutBinding);
            //UseVelocityCheckBox.SetBinding(CheckBox.IsCheckedProperty, useVelocityBinding);
            //TimeoutEnableCheckBox.SetBinding(CheckBox.IsCheckedProperty, timeoutEnableBinding);
            TimeoutEnableCheckBox.Checked += (s, e) => { TimeoutUpDown.IsEnabled = true; };
            TimeoutEnableCheckBox.Unchecked += (s, e) => { TimeoutUpDown.IsEnabled = false; };
            UseVelocityCheckBox.Checked += (s, e) => { DmxValueOnUpDown.IsEnabled = false; };
            UseVelocityCheckBox.Unchecked += (s, e) => { DmxValueOnUpDown.IsEnabled = true; };
        }

        private void DeleteLink_Click(object sender, RoutedEventArgs e)
        {
            DeleteLinkClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
