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
using System.Windows.Shapes;

namespace Mod003263.wpf
{
    /// <summary>
    /// Interaction logic for WPFMessageBoxErrorForm.xaml
    /// </summary>
    public partial class WPFMessageBoxErrorForm : Window {

        private Exception e;

        public WPFMessageBoxErrorForm() {
            InitializeComponent();
        }

        public WPFMessageBoxErrorForm(string titleSuffix) {
            InitializeComponent();
            Title += titleSuffix;
        }

        public WPFMessageBoxErrorForm SetException(Exception e) {
            this.e = e;
            lbl_Type.Content = e.GetType().Name;
            lbl_Message.Content = e.Message;
            lbl_source.Content = e.Source;
            txt_Trace.Text = e.StackTrace;
            btn_Inner.IsEnabled = this.e.InnerException != null;
            return this;
        }

        private void btn_Inner_Click(object sender, RoutedEventArgs e) {
            if(this.e != null)
            WPFMessageBoxFactory.CreateErrorAndShow(this.e.InnerException);
        }
    }
}
