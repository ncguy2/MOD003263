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
using Mod003263.events.test;
using Mod003263.events.ui;
using Mod003263.interview;
using Mod003263.wpf.controls;
using Utils.Tree;
using Utils.Tree.Builder;

/**
 * Author: Nick Guy
 * Date: 28/11/2016
 * Contains: QuestionEditor
 */
namespace Mod003263.controllerview.view {
    /// <summary>
    /// Interaction logic for QuestionEditor.xaml
    /// </summary>
    public partial class QuestionEditor : UserControl, SelectQuestionEvent.SelectQuestionListener {

        private Question selectedQuestion;

        public QuestionEditor()
        {
            InitializeComponent();
            try {
                PropertiesManager propertiesManager = PropertiesManager.GetInstance();
                List<Question> qData = DatabaseAccessor.GetInstance().PullQuestionData();
                VisitableTree<TreeObjectWrapper<Question>> tree =
                    new VisitableTree<TreeObjectWrapper<Question>>(new TreeObjectWrapper<Question>(""));
                TreePopulator.Populate(tree, qData, '/', q => q.Path());
                tr_Questions.Items.Add(tree);
            }catch (Exception e) {
                File.WriteAllText("Exception.txt", e.Source+": "+e.Message);
            }
        }

        private void addAnsBtn_Click(object sender, RoutedEventArgs e) {
            if (selectedQuestion == null) return;
            AddAnswer(new Answer(-1));
        }

        private void AddAnswer(Answer a) {
            AnswerRow row = new AnswerRow(a);
            row.SetButtonClicked(OnRowBtnClick);
            AddAnswerRow(row);
        }

        /// <summary>
        /// Add answer row to the answer table
        /// </summary>
        /// <param name="row"></param>
        private void AddAnswerRow(AnswerRow row) {
            if (selectedQuestion == null) return;
            selectedQuestion.GetAnswers().Add(row.GetAnswer());
            tbl_AnswerTable.Items.Add(row);
        }

        private void OnRowBtnClick(AnswerRow ansRow) {
            selectedQuestion?.GetAnswers().Remove(ansRow.GetAnswer());
            tbl_AnswerTable.Items.Remove(ansRow);
        }

        private void tr_Questions_OnSelectedItemChanged(Object sender, RoutedPropertyChangedEventArgs<Object> e) {
            Question q = tr_Questions.SelectedItem as Question;
            new SelectQuestionEvent(q, SelectQuestionScopes.QUESTION_EDITOR).Fire();
        }

        [Event]
        public void OnSelectQuestion(SelectQuestionEvent e) {
            if (!e.Scope.Equals(SelectQuestionScopes.QUESTION_EDITOR)) return;
            SelectQuestion(e.Question);
        }

        private void SelectQuestion(Question q) {
            this.selectedQuestion = q;
            if (this.selectedQuestion == null) return;
            txt_Category.Text = q.Cat();
            txt_Question.Text = q.Text();
            tbl_AnswerTable.Items.Clear();
            q.GetAnswers().ForEach(AddAnswer);
        }
    }
}
