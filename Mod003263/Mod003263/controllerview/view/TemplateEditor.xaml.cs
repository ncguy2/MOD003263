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
using Mod003263.events;
using Mod003263.events.ui;
using Mod003263.interview;

/**
 *  Author: Ryan Cowell
 *  Date: 16/11/2016
 *  Contains: TemplateEditor
 */
namespace Mod003263.controllerview.view
{
    /// <summary>
    /// Interaction logic for TemplateEditor.xaml
    /// </summary>
    public partial class TemplateEditor : UserControl, SelectTemplateEvent.SelectTemplateListener {

        public TemplateEditor() {
            EventBus.GetInstance().Register(this);
            InitializeComponent();
        }

        private void tr_Templates_OnSelectedItemChanged(Object sender, RoutedPropertyChangedEventArgs<Object> e) {
            InterviewFoundation template = tr_Templates.SelectedItem as InterviewFoundation;
            if (template == null) return;
            new SelectTemplateEvent(template, SelectTemplateScopes.TEMPLATE_EDITOR).Fire();
        }

        [Event]
        public void OnSelectTemplate(SelectTemplateEvent e) {
            // TODO Ryan to implement here
        }
    }
}
