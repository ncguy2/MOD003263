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
using Mod003263.db;
using Mod003263.events;
using Mod003263.events.ui;
using Mod003263.interview;
using Mod003263.interview.metric;
using Mod003263.visitor;
using Mod003263.wpf;
using Utils.Tree;
using Utils.Tree.Builder;

/**
 *  Author: Ryan Cowell
 *  Date: 26/10/2016
 *  Contains: TemplateSelection
 */
namespace Mod003263.controllerview.view {
    /// <summary>
    /// Interaction logic for TemplateSelection.xaml
    /// </summary>
    public partial class TemplateSelection : UserControl, SelectTemplateEvent.SelectTemplateListener, IInitializable,
        BackEvent.BackListener, InterviewToTemplateEvent.InterviewToTemplateListener {

        private VisitableTree<TreeObjectWrapper<InterviewFoundation>> foundations;
        private InterviewFoundation selected;

        public TemplateSelection() {
            EventBus.GetInstance().Register(this);
            InitializeComponent();
        }

        public void OnInitialization() {
            DatabaseAccessor.GetInstance().UsingInterviewFoundations(list => {
                foundations = new VisitableTree<TreeObjectWrapper<InterviewFoundation>>
                    (new TreeObjectWrapper<InterviewFoundation>(""));

                List<InterviewFoundation> l2 = new List<InterviewFoundation>(list);
                foreach (InterviewFoundation f in l2) {
                    f.SetCat(f.Position+"/"+f.Cat());
                }
                TreePopulator.Populate(foundations, l2, '/', f => f.Path(), (f, s) => {
                    if(s.Equals(f.Name())) return new TreeObjectWrapper<InterviewFoundation>(f, s);
                    return new TreeObjectWrapper<InterviewFoundation>(s.Replace("/", "_")+"/");
                });
                TreeViewItem root = new TreeViewItem{Header = "/"};
                tr_TemplateSelect.Items.Clear();
                foundations.Accept(new TreeViewPopulator<TreeObjectWrapper<InterviewFoundation>>(tr_TemplateSelect, root));
                try {
                    TreeViewItem r1 = root.Items[0] as TreeViewItem;
                    if (r1 == null) {
                        AddItemsToTree(tr_TemplateSelect, root);
                        return;
                    }
                    root.Items.Remove(r1);
                    AddItemsToTree(tr_TemplateSelect, r1);
                }catch (Exception e) {
                    WPFMessageBoxFactory.CreateErrorAndShow(e);
                }
            });
        }

        private void AddItemsToTree(TreeView view, TreeViewItem item) {
            List<object> lst = item.Items.Cast<object>().ToList();
            lst.ForEach(i => {
                item.Items.Remove(i);
                view.Items.Add(i);
            });
        }

        private void tr_TemplateSelect_OnSelectedItemChanged(Object sender, RoutedPropertyChangedEventArgs<Object> e) {
            TreeViewItem item = tr_TemplateSelect.SelectedItem as TreeViewItem;
            if (item == null) return;
            TreeObjectWrapper<InterviewFoundation> wrapper = item.Header as TreeObjectWrapper<InterviewFoundation>;
            InterviewFoundation template = wrapper?.Object;
            if (template == null) return;
            new SelectTemplateEvent(template, SelectTemplateScopes.TEMPLATE_USAGE).Fire();
        }

        private void Select(InterviewFoundation foundation) {
            this.selected = foundation;
            if (selected == null) return;
            txt_TemplateSelected.Text = selected.Name();
            txt_Metric.Text = selected.GetQuestionsWeight().ToString();
            txt_Position.Text = selected.Position;
            lv_Questions.Items.Clear();
            foreach (KeyValuePair<Question, int> pair in selected.GetQuestions()) {
                RowData data = new RowData{Question = pair.Key, Metric = pair.Value};
                lv_Questions.Items.Add(data);
            }
        }

        [Event]
        public void OnBack(BackEvent e) {
            Interview_Reverse_BeginStoryboard.Storyboard.Begin();
        }

        [Event]
        public void OnSelectTemplate(SelectTemplateEvent e) {
            if (!e.Scope.Equals(SelectTemplateScopes.TEMPLATE_USAGE)) return;
            Select(e.Template);
        }

        private void Btn_Start_OnClick(object sender, RoutedEventArgs e) {
            interviewSub.OnInitialization();
        }

        [Event]
        public void OnInterviewToTemplate(InterviewToTemplateEvent e) {
            Interview_Reverse_BeginStoryboard.Storyboard.Begin();
            OnInitialization();
        }
    }

    public class RowData {

        public RowData() {}

        public Question Question { get; set; }
        public int Metric { get; set; }
        public String QuestionText => Question.Text();
        public String Category => Question.Cat();
    }
}
