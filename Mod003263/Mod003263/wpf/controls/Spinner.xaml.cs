using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

/**
 * Author: Nick guy
 * Date: 28/11/2016
 * Contains: Spinner
 */
namespace Mod003263.wpf.controls {
    /// <summary>
    /// Interaction logic for Spinner.xaml
    /// </summary>
    public partial class Spinner : UserControl {
        public Spinner() {
            InitializeComponent();
            NUDTextBox.Text = StartValue.ToString();
        }

        private void NUDButtonUP_Click(object sender, RoutedEventArgs e)
        {
            int number;
            if (NUDTextBox.Text != "") number = Convert.ToInt32(NUDTextBox.Text);
            else number = 0;
            if (number < MaxValue)
                NUDTextBox.Text = Convert.ToString(number + 1);
        }

        private void NUDButtonDown_Click(object sender, RoutedEventArgs e)
        {
            int number;
            if (NUDTextBox.Text != "") number = Convert.ToInt32(NUDTextBox.Text);
            else number = 0;
            if (number > MinValue)
                NUDTextBox.Text = Convert.ToString(number - 1);
        }

        private void NUDTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Up)
            {
                NUDButtonUP.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonUP, new object[] { true });
            }


            if (e.Key != Key.Down) return;
            NUDButtonDown.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonDown, new object[] { true });
        }

        private void NUDTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonUP, new object[] { false });

            if (e.Key == Key.Down)
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonDown, new object[] { false });
        }

        private void NUDTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int number = 0;
            if (NUDTextBox.Text != "")
                if (!int.TryParse(NUDTextBox.Text, out number)) NUDTextBox.Text = StartValue.ToString();
            if (number > MaxValue) NUDTextBox.Text = MaxValue.ToString();
            if (number < MinValue) NUDTextBox.Text = MinValue.ToString();
            NUDTextBox.SelectionStart = NUDTextBox.Text.Length;

            SpinnerValueChanged?.Invoke(this, EventArgs.Empty);
        }

        public int GetNumericValue()
        {
            return int.Parse(NUDTextBox.Text);
        }

        public static readonly DependencyProperty StartValueProperty = DependencyProperty.Register("StartValue", typeof(int), typeof(Spinner));
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinimumValue", typeof(int), typeof(Spinner));
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaximumValue", typeof(int), typeof(Spinner));
        public event EventHandler SpinnerValueChanged;

        public int StartValue {
            get { return (int)GetValue(StartValueProperty); }
            set { SetValue(StartValueProperty, value); }
        }
        public int MinValue {
            get { return (int)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }
        public int MaxValue {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public string Display {
            get { return NUDTextBox.Text;  }
            set { NUDTextBox.Text = value; }
        }

    }
}
