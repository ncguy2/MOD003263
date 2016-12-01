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
using Mod003263.db;
using Utils.Tree.Builder;
using Utils.Tree;

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

        private InterviewFoundation selectedTemplate;

        public TemplateEditor() {
            EventBus.GetInstance().Register(this);
            InitializeComponent();
            PropertiesManager propertiesManager = PropertiesManager.GetInstance();
            List<InterviewFoundation> tData = DatabaseAccessor.GetInstance().PullInterviewFoundationData();
            VisitableTree<TreeObjectWrapper<InterviewFoundation>> tree =
                    new VisitableTree<TreeObjectWrapper<InterviewFoundation>>(new TreeObjectWrapper<InterviewFoundation>(""));
            TreePopulator.Populate(tree, tData, '/', t => t.Path());
            //          tr_Templates.Items.Add(tree);
            tData.ForEach(t => tr_Templates.Items.Add(t));

        }

        private void tr_Templates_OnSelectedItemChanged(Object sender, RoutedPropertyChangedEventArgs<Object> e) {
            InterviewFoundation template = tr_Templates.SelectedItem as InterviewFoundation;
            if (template == null) return;
            new SelectTemplateEvent(template, SelectTemplateScopes.TEMPLATE_EDITOR).Fire();
        }

        [Event]
        public void OnSelectTemplate(SelectTemplateEvent e) {
            if (!e.Scope.Equals(SelectTemplateScopes.TEMPLATE_EDITOR)) return;
            SelectTemplate(e.Template);
        }

        private void SelectTemplate(InterviewFoundation t){

            this.selectedTemplate = t;
            if (this.selectedTemplate == null) return;
            txt_Category.Text = t.Cat();
            txt_TemplateName.Text = t.Text();
            tr_InsertQuestions.Items.Clear();
            foreach (Question question in t.GetQuestions().ToArray())
            {
                
            }


        }
    }
}
