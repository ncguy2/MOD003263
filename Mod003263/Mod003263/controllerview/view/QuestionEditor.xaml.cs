using System;
using System.Collections.Generic;
using System.IO;
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
using Mod003263.events.io;
using Mod003263.events.test;
using Mod003263.events.ui;
using Mod003263.interview;
using Mod003263.wpf.controls;
using Utils.Tree;
using Utils.Tree.Builder;
using Mod003263.visitor;
using Mod003263.wpf;

/**
 * Author: Nick Guy
 * Date: 28/11/2016
 * Contains: QuestionEditor
 */
namespace Mod003263.controllerview.view {
    /// <summary>
    /// Interaction logic for QuestionEditor.xaml
    /// </summary>
    public partial class QuestionEditor : UserControl, SelectQuestionEvent.SelectQuestionListener,
        BackEvent.BackListener, SaveQuestionEvent.SaveQuestionListener {

        private Question selectedQuestion;
        private List<Question> questions;
        private VisitableTree<TreeObjectWrapper<Question>> questionTree;

        public QuestionEditor() {
            EventBus.GetInstance().Register(this);
            InitializeComponent();
            PropertiesManager propertiesManager = PropertiesManager.GetInstance();
            try {
                DatabaseAccessor.GetInstance().UsingQuestionData(list => {
                    questions = list;
                    BuildTree();
                });
            }catch(Exception e) {}
        }

        private void BuildTree() {
            BuildTree(questions);
        }

        private void BuildTree(List<Question> flatList) {
            questionTree =
                new VisitableTree<TreeObjectWrapper<Question>>(new TreeObjectWrapper<Question>(""));
            TreePopulator.Populate(questionTree, flatList, '/', q => q.Path(), (q, s) => {
                if(s.Equals(q.Text())) return new TreeObjectWrapper<Question>(q, s);
                return new TreeObjectWrapper<Question>(s.Replace("/", "_")+"/");
            });
            TreeViewItem root = new TreeViewItem{Header = "/"};
            tr_Questions.Items.Clear();
            questionTree.Accept(new TreeViewPopulator<TreeObjectWrapper<Question>>(tr_Questions, root));
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

        private void AddItemsToTree(TreeView view, TreeViewItem item) {
            List<object> lst = item.Items.Cast<object>().ToList();
            lst.ForEach(i => {
                item.Items.Remove(i);
                view.Items.Add(i);
            });
        }


        private void addAnsBtn_Click(object sender, RoutedEventArgs e) {
            if (selectedQuestion == null) return;
            AddAnswer(new Answer(-1));
        }

        private void AddAnswer(Answer a) {
            if (selectedQuestion == null) return;
            try {
                AnswerRowData data = new AnswerRowData(a) {OnClick = RowData_OnClick};
                tbl_AnswerTable.Items.Add(data);
                if (a.Id == -1) selectedQuestion.AddAnswers(a);
            }catch (Exception e) {
                WPFMessageBoxFactory.CreateErrorAndShow(e);
            }
        }

        private void RowData_OnClick(object sender, RoutedEventArgs args) {
            Button btn = sender as Button;
            AnswerRowData data = btn?.DataContext as AnswerRowData;
            if (data == null) return;
            selectedQuestion?.GetAnswers().Remove(data.Answer);
            tbl_AnswerTable.Items.Remove(data);
        }

        private void tr_Questions_OnSelectedItemChanged(Object sender, RoutedPropertyChangedEventArgs<Object> e) {
            TreeViewItem t = tr_Questions.SelectedItem as TreeViewItem;
            if (t == null) return;
            TreeObjectWrapper<Question> w = t.Header as TreeObjectWrapper<Question>;
            Question q = w?.Object;
            new SelectQuestionEvent(q, SelectQuestionScopes.QUESTION_EDITOR).Fire();
        }

        [Event]
        public void OnSelectQuestion(SelectQuestionEvent e) {
            if (!e.Scope.Equals(SelectQuestionScopes.QUESTION_EDITOR)) return;
            SelectQuestion(e.Question);
        }

        private void SelectQuestion(Question q) {
            this.selectedQuestion = q;

            txt_Category.IsEnabled  = q != null;
            txt_Question.IsEnabled  = q != null;
            btn_AddAnswer.IsEnabled = q != null;
            tbl_AnswerTable.Items.Clear();

            if (q == null) {
                txt_Category.Text = "";
                txt_Question.Text = "";
                return;
            }

            txt_Category.Text = q.Cat();
            txt_Question.Text = q.Text();
            foreach (Answer answer in q.GetAnswers())
                AddAnswer(answer);
        }

        private void CascadeData() {
            if (this.selectedQuestion == null) return;
            this.selectedQuestion.SetCategory(txt_Category.Text);
            this.selectedQuestion.SetText(txt_Question.Text);
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e) {
            CascadeData();
            if (this.selectedQuestion != null)
                new SaveQuestionEvent(this.selectedQuestion).Fire();
            SelectQuestion(null);
        }

        [Event]
        public void OnBack(BackEvent e) {
            CascadeData();
            if (this.selectedQuestion != null)
                new SaveQuestionEvent(this.selectedQuestion).Fire();
            SelectQuestion(null);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
            Button btn = sender as Button;
            AnswerRowData data = btn?.DataContext as AnswerRowData;
            data?.OnClick(sender, e);
        }

        private void RangeBase_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            Slider sld = sender as Slider;
            AnswerRowData data = sld?.DataContext as AnswerRowData;
            data?.WeightChanged(sender, e);
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e) {
            TextBox txt = sender as TextBox;
            AnswerRowData data = txt?.DataContext as AnswerRowData;
            data?.TextChanged(sender, e);
        }

        private void Btn_AddQuestion_OnClick(object sender, RoutedEventArgs e) {
            new SelectQuestionEvent(new Question(-1), SelectQuestionScopes.QUESTION_EDITOR).Fire();
        }

        [Event]
        public void OnSaveQuestion(SaveQuestionEvent e) {
            if(e.Payload.Id == -1) questions.Add(e.Payload);
            BuildTree();
        }
    }

    public class AnswerRowData {

        public Answer Answer { get; private set; }

        public Action<object, RoutedEventArgs> OnClick { get; set; }
        public string Text { get; set; }
        public int Weight { get; set; }

        public Action<object, RoutedEventArgs> TextChanged => txtChange;
        public Action<object, RoutedEventArgs> WeightChanged => weightChange;

        public AnswerRowData(Answer a) {
            this.Answer = a;
            this.Text = a.Text;
            this.Weight = a.Weight;
        }

        public void txtChange(object sender, RoutedEventArgs e) {
            TextBox t = sender as TextBox;
            if (t == null) return;
            Text = t.Text;
            Answer.SetText(Text);
        }

        public void weightChange(object sender, RoutedEventArgs e) {
            Slider s = sender as Slider;
            if (s == null) return;
            Weight = (int) s.Value;
            Answer.SetWeight(Weight);
        }

    }

}
