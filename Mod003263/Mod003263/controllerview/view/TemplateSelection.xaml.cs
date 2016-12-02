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
 *  Date: 26/10/2016
 *  Contains: TemplateSelection
 */
namespace Mod003263.controllerview.view {
    /// <summary>
    /// Interaction logic for TemplateSelection.xaml
    /// </summary>
    public partial class TemplateSelection : UserControl {

        public TemplateSelection() {
            EventBus.GetInstance().Register(this);
            InitializeComponent();
        }

        private void tr_TemplateSelect_OnSelectedItemChanged(Object sender, RoutedPropertyChangedEventArgs<Object> e) {
            InterviewFoundation template = tr_TemplateSelect.SelectedItem as InterviewFoundation;
            if (template == null) return;
            new SelectTemplateEvent(template, SelectTemplateScopes.TEMPLATE_USAGE).Fire();
        }

        [Event]
        public void OnBack(BackEvent e) {
            Interview_Reverse_BeginStoryboard.Storyboard.Begin();
        }
    }
}
