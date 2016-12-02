using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
 using Mod003263.controllerview.controller;
 using Mod003263.email;
using Mod003263.events;
using Mod003263.events.test;
 using Mod003263.interview;
using Mod003263.threading;
using Mod003263.wpf;
 using Utils.Tree;
 using Utils.Tree.Builder;
 using Utils.Tree.Visitor;

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
            ThreadFactory.GetInstance().CreateManagedThread(SendEmail, "Email").Start();
            EventBus.GetInstance().Register(this);

//            VisitableTree<TreeObjectWrapper<InterviewFoundation>> visitableTree = new TemplateSelectionController().PopulateTree().GetTemplates();
//            StringBuilder sb = new StringBuilder();
//            visitableTree.Accept(new PrintIndentedVisitor<TreeObjectWrapper<InterviewFoundation>>(0, s => sb.Append(s)));
//            File.WriteAllText("test.txt", sb.ToString());
        }

        private async void EmailTask() {
            await new Task(SendEmail);
        }

        private void SendEmail() {
            EmailHandler.GetInstance().Send("ssmithtech60@gmail.com", "nick.guy@hotmail.co.uk", "Test Email", "testing email sending");
        }

        [Event]
        public void OnStringPayload(StringPayloadEvent e) {
            WPFMessageBoxFactory.Create("String payload", e.Payload, 0).Show();
        }
    }
}
