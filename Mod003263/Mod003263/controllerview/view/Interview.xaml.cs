using Mod003263.events.ui;
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
using Mod003263.events.io;
using Mod003263.interview;
using Mod003263.interview.metric;
using Mod003263.wpf;

/**
 *  Author: Ryan Cowell
 *  Date: 28/11/2016
 *  Contains: Interview
 */
namespace Mod003263.controllerview.view
{
    /// <summary>
    /// Interaction logic for Interview.xaml
    /// </summary>
    public partial class Interview : UserControl, IInitializable, SelectTemplateEvent.SelectTemplateListener,
        SelectApplicantEvent.SelectApplicantListener {

        private interview.Interview interview;
        private InterviewFoundation foundation;
        private Applicant applicant;
        private List<Question> orderedQuestions;

        private Question currentQuestion;
        private Answer currentAnswer;

        private int currentId;
        private int maxId;

        public Interview() {
            EventBus.GetInstance().Register(this);
            InitializeComponent();
        }

        private void btn_Confirm_Click(object sender, RoutedEventArgs e) {
            Dictionary<Question, Answer> m = interview.GetFoundationInstance().GetAnswerMap();
            m.Add(currentQuestion, currentAnswer);
            Advance();
        }

        private void Advance() {
            SetQuestion(currentId+1);
        }

        private void SetQuestion(int i) {
            currentId = i;
            if (currentId >= maxId) {
                Finish();
                return;
            }
            lbl_Question.Content = $"Question {i+1} of {maxId}";
            currentQuestion = null;
            lv_Answers.UnselectAll();
            Question q = orderedQuestions[currentId];
            currentQuestion = q;
            SetCurrentAnswer(null);
            txt_Question.Text = q.Text();
            lv_Answers.Items.Clear();
            foreach (Answer a in q.GetAnswers())
                lv_Answers.Items.Add(a);
        }

        public void OnInitialization() {
            interview = new interview.Interview(-1, foundation) {
                Flag = 0,
                Subject=applicant
            };
            orderedQuestions = new List<Question>(foundation.GetQuestions().Keys);
            maxId = orderedQuestions.Count;
            SetQuestion(0);
        }

        private void Finish() {
            interview.Feedback = txt_Feedback.Text;
            interview.Subject.Flag = ApplicantFlags.COMPLETE;
            WPFMessageBoxFactoryData d = new WPFMessageBoxFactoryData {
                Header = "Interview complete",
                Content = "What do you wish to do next?",
                Mask = WPFMessageBoxForm.LEFT | WPFMessageBoxForm.MID | WPFMessageBoxForm.RIGHT,
                Left = "Interview another applicant with the same question set",
                Mid = "Interview another applicant with a different question set",
                Right = "Begin review process",
                OnLeft = AnotherInterview_SameSet,
                OnMid = AnotherInterview_DifferentSet,
                OnRight = ReviewInterviews
            };
            new MetricCalculator().CalculateMetric(interview);
            new SaveInterviewEvent(interview).Fire();
            new SaveApplicantEvent(applicant).Fire();
            WPFMessageBoxFactory.CreateAndShow(d);
        }

        private void AnotherInterview_SameSet(WPFMessageBoxForm form) {
            form.Hide();
            // TODO revert form to applicant selection
            new InterviewToApplicantEvent().Fire();
        }
        private void AnotherInterview_DifferentSet(WPFMessageBoxForm form) {
            form.Hide();
            new InterviewToTemplateEvent().Fire();
            // TODO revert form to template selection
        }
        private void ReviewInterviews(WPFMessageBoxForm form) {
            form.Hide();
            new BackEvent().Fire();
            new OpenLeaderboardEvent().Fire();
        }

        [Event]
        public void OnSelectTemplate(SelectTemplateEvent e) {
            if (!e.Scope.Equals(SelectTemplateScopes.TEMPLATE_USAGE)) return;
            foundation = e.Template;
        }

        [Event]
        public void OnSelectApplicant(SelectApplicantEvent e) {
            if (!e.Scope.Equals(SelectApplicantScopes.APPLICANT_USAGE)) return;
            applicant = e.Selected;
        }

        private void Lv_Answers_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (currentQuestion == null) return;
            Answer a = lv_Answers.SelectedItem as Answer;
            SetCurrentAnswer(a);
        }

        private void SetCurrentAnswer(Answer a) {
            currentAnswer = a;
            btn_Confirm.Content = $"Confirm Selected Answer: {a?.Text}";
        }

    }
}
