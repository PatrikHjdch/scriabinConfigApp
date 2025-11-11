using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(int), typeof(NumericUpDown), new FrameworkPropertyMetadata(0));
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(NumericUpDown), new FrameworkPropertyMetadata(100));
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(NumericUpDown), new FrameworkPropertyMetadata(0, OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (NumericUpDown)d;
            control.NumericTextBox.Text = e.NewValue.ToString();
        }

        public int MinValue
        {
            get { return (int)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }
        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public NumericUpDown()
        {
            InitializeComponent();
            SetValue(MinValueProperty, MinValue);
            SetValue(MaxValueProperty, MaxValue);
            SetValue(ValueProperty, Value);
            NumericTextBox.Text = GetValue(ValueProperty).ToString();
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            if ((int)GetValue(ValueProperty) < (int)GetValue(MaxValueProperty))
            {
                SetValue(ValueProperty, (int)GetValue(ValueProperty) + 1);
            }
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            if ((int)GetValue(ValueProperty) > (int)GetValue(MinValueProperty))
            {
                SetValue(ValueProperty, (int)GetValue(ValueProperty) - 1);
            }
        }

        private void OnGotKeyboardFocus(object sender, RoutedEventArgs e)
        {
            NumericTextBox.SelectAll();
        }

        private void OnLostKeyboardFocus(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(NumericTextBox.Text, out int newValue))
            {
                if (newValue < (int)GetValue(MinValueProperty))
                {
                    SetValue(ValueProperty, (int)GetValue(MinValueProperty));
                }
                else if (newValue > (int)GetValue(MaxValueProperty))
                {
                    SetValue(ValueProperty, (int)GetValue(MaxValueProperty));
                }
                else
                {
                    SetValue(ValueProperty, newValue);
                }
            }
            else
            {
                // If parsing fails, revert to the current Value
                NumericTextBox.Text = GetValue(ValueProperty).ToString();
            }
            NumericTextBox.Text = GetValue(ValueProperty).ToString();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Keyboard.ClearFocus();
            }
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            NumericTextBox.Text = GetValue(ValueProperty).ToString();
        }
    }
}
