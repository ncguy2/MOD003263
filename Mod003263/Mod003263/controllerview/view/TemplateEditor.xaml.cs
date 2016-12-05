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
using Mod003263.events.io;
using Mod003263.visitor;
using Mod003263.wpf;
using Mod003263.wpf.controls;

/**
 *  Author: Ryan Cowell
 *  Date: 16/11/2016
 *  Contains: TemplateEditor
 */
namespace Mod003263.controllerview.view{
    /// <summary>
    /// Interaction logic for TemplateEditor.xaml
    /// </summary>
    public partial class TemplateEditor : UserControl, SelectTemplateEvent.SelectTemplateListener,
        DeleteFoundationEvent.DeleteFoundationListener, IInitializable {

        private List<InterviewFoundation> foundations;
        private List<AvailablePosition> positions;
        private VisitableTree<TreeObjectWrapper<InterviewFoundation>> foundationsTree;
        private InterviewFoundation selectedTemplate;
        private List<Question> questions;

        public TemplateEditor() {
            EventBus.GetInstance().Register(this);
            InitializeComponent();
        }

        public void OnInitialization() {
            try {
                DatabaseAccessor.GetInstance().UsingInterviewFoundations(tData => {
                    foundations = tData;
                    BuildTree();
                });
                DatabaseAccessor.GetInstance().UsingQuestionData(qData => {
                    questions = qData;
                    PopulateQuestionLists();
                });
                DatabaseAccessor.GetInstance().UsingAllPositions(pData => {
                    positions = pData;
                    PopulatePositions();
                });
            }catch (Exception e) {}
        }

        private void PopulatePositions() {
            positions.ForEach(pos => cmb_Position.Items.Add(pos));
        }

        private void BuildTree() {
            BuildTree(foundations);
        }

        private void BuildTree(List<InterviewFoundation> tData) {
            foundationsTree =
                new VisitableTree<TreeObjectWrapper<InterviewFoundation>>(
                    new TreeObjectWrapper<InterviewFoundation>(""));
            TreePopulator.Populate(foundationsTree, tData, '/', f => f.Path(), (f, s) => {
                if(s.Equals(f.Name())) return new TreeObjectWrapper<InterviewFoundation>(f, s);
                return new TreeObjectWrapper<InterviewFoundation>(s.Replace("/", "_")+"/");
            });
            TreeViewItem root = new TreeViewItem{Header = "/"};
            tr_Templates.Items.Clear();
            foundationsTree.Accept(new TreeViewPopulator<TreeObjectWrapper<InterviewFoundation>>(tr_Templates, root));
            try {
                TreeViewItem r1 = root.Items[0] as TreeViewItem;
                if (r1 == null) {
                    AddItemsToTree(tr_Templates, root);
                    return;
                }
                root.Items.Remove(r1);
                AddItemsToTree(tr_Templates, r1);
            }catch (Exception e) {
                WPFMessageBoxFactory.CreateErrorAndShow(e);
            }
        }

        private void PopulateQuestionLists() {
            VisitableTree<TreeObjectWrapper<Question>> tree =
                new VisitableTree<TreeObjectWrapper<Question>>(
                    new TreeObjectWrapper<Question>(""));
            TreePopulator.Populate(tree, questions, '/', q => q.Path(), (q, s) => {
                if(s.Equals(q.Text())) return new TreeObjectWrapper<Question>(q, s);
                return new TreeObjectWrapper<Question>(s.Replace("/", "_")+"/");
            });
            TreeViewItem root = new TreeViewItem{Header = "/"};
            tr_Questions.Items.Clear();
            tree.Accept(new TreeViewPopulator<TreeObjectWrapper<Question>>(tr_Questions, root));
            try {
                TreeViewItem r1 = root.Items[0] as TreeViewItem;
                if (r1 == null) {
                    AddItemsToTree(tr_Questions, root);
                    return;
                }
                root.Items.Remove(r1);
                AddItemsToTree(tr_Questions, r1);
            }catch (Exception e) {
                WPFMessageBoxFactory.CreateErrorAndShow(e);
            }
        }

        private void PopulateSelectedQuestionList() {
            lst_InsertQuestions.Items.Clear();
            if (selectedTemplate == null) return;
            foreach (KeyValuePair<Question, int> pair in selectedTemplate.GetQuestions())
                lst_InsertQuestions.Items.Add(new RowData {Question = pair.Key, Metric = pair.Value});
        }

        private void AddItemsToTree(TreeView view, TreeViewItem item) {
            List<object> lst = item.Items.Cast<object>().ToList();
            lst.ForEach(i => {
                item.Items.Remove(i);
                view.Items.Add(i);
            });
        }

        private void tr_Templates_OnSelectedItemChanged(Object sender, RoutedPropertyChangedEventArgs<Object> e) {
            TreeViewItem t = tr_Templates.SelectedItem as TreeViewItem;
            TreeObjectWrapper<InterviewFoundation> wrapper = t?.Header as TreeObjectWrapper<InterviewFoundation>;
            InterviewFoundation template = wrapper?.Object;
            if (template == null) return;
            new SelectTemplateEvent(template, SelectTemplateScopes.TEMPLATE_EDITOR).Fire();
        }

        private void CascadeData() {
            if (selectedTemplate == null) return;
            selectedTemplate.SetCat(txt_Category.Text);
            selectedTemplate.SetName(txt_TemplateName.Text);
            selectedTemplate.Position = (cmb_Position.SelectedItem as AvailablePosition)?.Position;
        }

        [Event]
        public void OnSelectTemplate(SelectTemplateEvent e) {
            if (!e.Scope.Equals(SelectTemplateScopes.TEMPLATE_EDITOR)) return;
            SelectTemplate(e.Template);
        }

        private void SelectTemplate(InterviewFoundation t) {
            this.selectedTemplate = t;
            if (this.selectedTemplate == null) return;
            txt_Category.Text = t.Cat();
            txt_TemplateName.Text = t.Name();
            foreach (AvailablePosition pos in positions) {
                if (pos.Position.Equals(t?.Position))
                    cmb_Position.SelectedItem = pos;
            }
            PopulateSelectedQuestionList();
        }

        private void btn_Create_Click(object sender, RoutedEventArgs e) {
            new SelectTemplateEvent(new InterviewFoundation(-1, "", ""), SelectTemplateScopes.TEMPLATE_EDITOR).Fire();
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e) {
            CascadeData();
            if (this.selectedTemplate != null) {
                new SaveFoundationEvent(this.selectedTemplate).Fire();
                foundations.Add(this.selectedTemplate);
                BuildTree();
            }
            SelectTemplate(null);
        }

        private void Btn_AddQuestion_OnClick(object sender, RoutedEventArgs e) {
            if (selectedTemplate == null) return;
            TreeViewItem t = tr_Questions.SelectedItem as TreeViewItem;
            TreeObjectWrapper<Question> wrapper = t?.Header as TreeObjectWrapper<Question>;
            Question q = wrapper?.Object;
            if (q == null) return;
            if (!selectedTemplate.GetQuestions().ContainsKey(q))
                selectedTemplate.GetQuestions().Add(q, 1);
            PopulateSelectedQuestionList();
        }

        private void Btn_RemoveQuestion_OnClick(object sender, RoutedEventArgs e) {
            if (selectedTemplate == null) return;
            TreeViewItem t = tr_Questions.SelectedItem as TreeViewItem;
            TreeObjectWrapper<Question> wrapper = t?.Header as TreeObjectWrapper<Question>;
            Question q = wrapper?.Object;
            if (q == null) return;
            if (selectedTemplate.GetQuestions().ContainsKey(q))
                selectedTemplate.GetQuestions().Remove(q);
            PopulateSelectedQuestionList();
        }

        private void Spinner_OnSpinnerValueChanged(object sender, EventArgs e) {
            if (selectedTemplate == null) return;
            Spinner s = sender as Spinner;
            RowData d = s?.DataContext as RowData;
            if (d == null) return;
            d.Metric = s.GetNumericValue();
            Dictionary<Question, int> qs = selectedTemplate.GetQuestions();
            qs[d.Question] = d.Metric;
        }

        private void FrameworkElement_OnRequestBringIntoView(object sender, RoutedEventArgs routedEventArgs) {
            Spinner s = sender as Spinner;
            RowData d = s?.DataContext as RowData;
            if (d == null) return;
            s.NUDTextBox.Text = d.Metric + "";
        }

        private void Delete(InterviewFoundation f) {
            SelectTemplate(null);
            new DeleteFoundationEvent(f).Fire();
        }

        private void Btn_Delete_OnClick(object sender, RoutedEventArgs e) {
            if (selectedTemplate == null) return;
            WPFMessageBoxFactoryData data = new WPFMessageBoxFactoryData {
                Header = $"Deleting template {selectedTemplate.Name()}",
                Content = "Are you sure you wish to delete this template. This operation is irreversable.",
                Mask = WPFMessageBoxForm.MID | WPFMessageBoxForm.RIGHT,
                OnMid = form => form.Hide(),
                OnRight = form => {
                    Delete(selectedTemplate);
                    form.Hide();
                },
                Mid = "Cancel",
                Right = "Delete"
            };
            WPFMessageBoxFactory.CreateAndShow(data);
        }

        [Event]
        public void OnDeleteFoundation(DeleteFoundationEvent e) {
            if (e.Foundation == null) return;
            foundations.Remove(e.Foundation);
            BuildTree();
        }
    }

}
