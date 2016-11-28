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
using Mod003263.events;
using Mod003263.events.test;
using Mod003263.wpf;

namespace Mod003263
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, StringPayloadEvent.StringPayloadListener
    {
        public MainWindow() {
            InitializeComponent();
//            SendEmail();
            EventBus.GetInstance().Register(this);
        }

        private async void EmailTask() {
            await new Task(SendEmail);
        }

        private void SendEmail() {
            EmailHandler.GetInstance().Send("HappyTech", "nick.guy@hotmail.co.uk", "Test Email", "testing email sending");
        }

        [Event]
        public void OnStringPayload(StringPayloadEvent e) {
            WPFMessageBoxFactory.Create("String payload", e.Payload, 0).Show();
        }
    }
}
