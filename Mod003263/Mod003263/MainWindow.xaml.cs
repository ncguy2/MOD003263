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
using Mod003263.email;

namespace Mod003263
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() {
            InitializeComponent();
//            SendEmail();
        }

        private async void EmailTask() {
            await new Task(SendEmail);
        }

        private void SendEmail() {
            EmailHandler.GetInstance().Send("HappyTech", "nick.guy@hotmail.co.uk", "Test Email", "testing email sending");
        }
    }
}
